using System;
using System.Linq;
using Estudos.EFCore.Performance.Data;
using Estudos.EFCore.Performance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup();
            //ConsultaRastreada();
            //ConsultaNaoRastreada();
            //ConsultaComResolucaoDeIdentidade();
            //ConsultaProjetadaERastreada();

            //Inserir_200_Departamentos_Com_1MB();

            ConsultaProjetada();
        }

        /// <summary>
        /// Terá apenas uma instancia de Departamento em memória
        /// </summary>
        static void ConsultaRastreada()
        {
            using var db = new ApplicationDbContext();

            var funcionarios = db.Funcionarios
                .Include(p => p.Departamento)
                .ToList();
        }

        /// <summary>
        /// terá 100 instancias de departamento em memória, consumindo assim, mais recurso
        /// que a consulta rastreada
        /// </summary>
        static void ConsultaNaoRastreada()
        {
            using var db = new ApplicationDbContext();

            var funcionarios = db.Funcionarios.
                AsNoTracking()
                .Include(p => p.Departamento).ToList();

        }

        /// <summary>
        /// Terá apenas uma instancia de Departamento em memória
        /// e não ira rastrear as entidades, consumindo assim, menos recusos
        /// </summary>
        static void ConsultaComResolucaoDeIdentidade()
        {
            using var db = new ApplicationDbContext();

            var funcionarios = db.Funcionarios
                .AsNoTrackingWithIdentityResolution()
                .Include(p => p.Departamento)
                .ToList();
        }

        /// <summary>
        /// configurando o tracking das consultas
        /// </summary>
        static void ConsultaCustomizada()
        {
            using var db = new ApplicationDbContext();

            //alterando o tracking para a instância do DbContext
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var funcionarios = db.Funcionarios
                //alterando o tracking para a consulta
                //.AsTracking()
                //.AsNoTracking()
                //.AsNoTrackingWithIdentityResolution()
                .Include(p => p.Departamento)
                .ToList();
        }


        /// <summary>
        /// popr padrão consultas projetadas não são rastreadas,
        /// 
        /// mas mesmo que seja retornado um objeto anônimo, se uma entidade for retonada
        /// ela será rastreada por ser uma entidade
        /// </summary>
        static void ConsultaProjetadaERastreada()
        {
            using var db = new ApplicationDbContext();

            var departamentos = db.Departamentos
                //.AsTracking()
                .Include(p => p.Funcionarios)
                .Select(p => new
                {
                    //retonarnano a entidade (p)
                    Departamento = p,
                    TotalFuncionarios = p.Funcionarios.Count()
                })
                .ToList();

            departamentos[0].Departamento.Descricao = "Departamento Teste Atualizado " + DateTime.Now;

            db.SaveChanges();
        }

        /// <summary>
        /// reduzindo o consumo de memória com consulta projetada
        /// </summary>
        static void ConsultaProjetada()
        {
            using var db = new ApplicationDbContext();

            //var departamentos = db.Departamentos.ToArray();
            var departamentos = db.Departamentos.Select(p => p.Descricao).ToArray();
            //WorkingSet64 - o processo que esta sendo executado
            // 1024 / 1024 convertendo a memória utilizada para megas
            var memoria = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024) + " MB";

            Console.WriteLine(memoria);
        }

        static void Setup()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.Add(new Departamento
            {
                Descricao = "Departamento Teste",
                Ativo = true,
                Funcionarios = Enumerable.Range(1, 100).Select(p => new Funcionario
                {
                    CPF = p.ToString().PadLeft(11, '0'),
                    Nome = $"Funcionario {p}",
                    RG = p.ToString()
                }).ToList()
            });

            db.SaveChanges();
        }

        static void Inserir_200_Departamentos_Com_1MB()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var random = new Random();

            db.Departamentos.AddRange(Enumerable.Range(1, 200).Select(p =>
                new Departamento
                {
                    Descricao = "Departamento Teste",
                    Image = GetBytes()
                }));

            db.SaveChanges();

            byte[] GetBytes()
            {
                var buffer = new byte[1024 * 1024];
                random.NextBytes(buffer);

                return buffer;
            }
        }
    }
}
