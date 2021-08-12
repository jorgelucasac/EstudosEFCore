using System;
using System.Linq;
using Estudos.EFCore.Dicas.Data;
using Estudos.EFCore.Dicas.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Dicas
{
    class Program
    {
        static void Main(string[] args)
        {
            //using var db = new ApplicationContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();
            //ToQueryString();
            //DebugView();
            //Clear();
            //ConsultaFiltrada();
            //SingleOrDefaultVsFirstOrDefault();
            ToView();
        }

        /// <summary>
        /// retorna a sql que será executada, mas não executa
        /// </summary>
        static void ToQueryString()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureCreated();

            var query = db.Departamentos.Where(p => p.Id > 2);

            var sql = query.ToQueryString();

            Console.WriteLine(sql);
        }

        /// <summary>
        /// DebugView é uma propriedade que pode ser analisada em modo debug
        /// </summary>
        static void DebugView()
        {
            using var db = new ApplicationContext();

            //db.ChangeTracker.DebugView;
            
            db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });


            var query = db.Departamentos.Where(p => p.Id > 2);

            //somente em debug
            //query.DebugView
        }

        /// <summary>
        /// Redefinindo o estado do contexto
        /// </summary>
        static void Clear()
        {
            using var db = new ApplicationContext();

            db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });

            //limpar o track
            //descartando as entidades que estão sendo rastreadas
            db.ChangeTracker.Clear();
        }

        /// <summary>
        /// Include com consultas filtradas
        /// </summary>
        static void ConsultaFiltrada()
        {
            using var db = new ApplicationContext();

            //filtra pela propriedade de navegação incluida
            var sql = db
                .Departamentos
                .Include(p => p.Colaboradores.Where(c => c.Nome.Contains("Teste")))
                .ToQueryString();

            Console.WriteLine(sql);
        }

        /// <summary>
        /// SingleOrDefault vs FirstOrDefault
        /// </summary>
        static void SingleOrDefaultVsFirstOrDefault()
        {
            using var db = new ApplicationContext();

            Console.WriteLine("SingleOrDefault:");

            //utiliza 'TOP (2)' na consulta para verificar se há duplicidade
            // util quando não pode existir dois registro com o mesmo dado
            _ = db.Departamentos.SingleOrDefault(p => p.Id > 2);

            Console.WriteLine("FirstOrDefault:");

            //utiliza 'TOP (1)' e retorna o primeiro item
            _ = db.Departamentos.FirstOrDefault(p => p.Id > 2);
        }

        /// <summary>
        /// consultando em tabelas sem chave primaria
        /// não é possível inserir/editar ou deletar dados em tabelas sem chaves
        /// </summary>
        static void SemChavePrimaria()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var usuarioFuncoes = db.UsuarioFuncoes.Where(p => p.UsuarioId == Guid.NewGuid()).ToArray();
        }

        /// <summary>
        /// realizando consulta em uma view
        /// </summary>
        static void ToView()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Database.ExecuteSqlRaw(
                @"CREATE VIEW vw_departamento_relatorio AS
                SELECT
                    d.Descricao, count(c.Id) as Colaboradores
                FROM Departamentos d 
                LEFT JOIN Colaboradores c ON c.DepartamentoId=d.Id
                GROUP BY d.Descricao");

            var departamentos = Enumerable.Range(1, 10)
                .Select(p => new Departamento
                {
                    Descricao = $"Departamento {p}",
                    Colaboradores = Enumerable.Range(1, p)
                        .Select(c => new Colaborador
                        {
                            Nome = $"Colaborador {p}-{c}"
                        }).ToList()
                });

            var departamento = new Departamento
            {
                Descricao = $"Departamento Sem Colaborador"
            };

            db.Departamentos.Add(departamento);
            db.Departamentos.AddRange(departamentos);
            db.SaveChanges();

            var relatorio = db.DepartamentoRelatorio
                .Where(p => p.Colaboradores < 20)
                .OrderBy(p => p.Departamento)
                .ToList();

            foreach (var dep in relatorio)
            {
                Console.WriteLine($"{dep.Departamento} [ Colaboradores: {dep.Colaboradores}]");
            }
        }

    }
}
