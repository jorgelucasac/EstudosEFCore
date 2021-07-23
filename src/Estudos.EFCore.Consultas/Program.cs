using System;
using System.Data;
using System.Linq;
using Estudos.EFCore.Consultas.Data;
using Estudos.EFCore.Consultas.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Consultas
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            //FiltroGlobal();
            //IgnorarFiltroGlobal();
            //ConsultaProjetada();
            //ConsultaParametrizada();
            //ConsultaInterolada();
            ConsultaComTag();



            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// utilizando um filtro global para retornar apenas os registros não excluídos
        /// </summary>
        static void FiltroGlobal()
        {
            using var db = new ApplicationDbContext();
            Setup(db);

            var departamentos = db.Departamentos.Where(p => p.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }

        /// <summary>
        /// Ignorando o filtro global definido para o mapeamento
        /// </summary>
        static void IgnorarFiltroGlobal()
        {
            using var db = new ApplicationDbContext();
            Setup(db);

            var departamentos = db.Departamentos
                .IgnoreQueryFilters().Where(p => p.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }

        //informa os campos que devem ser retonados na consulta
        static void ConsultaProjetada()
        {
            using var db = new ApplicationDbContext();
            Setup(db);

            var departamentos = db.Departamentos
                .Where(p => p.Id > 0)
                .Select(p => new { p.Descricao, Funcionarios = p.Funcionarios.Select(a => a.Nome) })
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\t Nome: {funcionario}");
                }
            }
        }

        /// <summary>
        /// utilizando sql para montar a consulta
        /// </summary>
        static void ConsultaParametrizada()
        {
            using var db = new ApplicationDbContext();
            //Setup(db);

            //forma 1
            //var id = 0;

            //forma 2
            var id = new SqlParameter
            {
                Value = 1,
                SqlDbType = SqlDbType.Int
            };
            var departamentos = db.Departamentos
                .FromSqlRaw("SELECT * FROM Departamentos WHERE Id>{0}", id)
                .Where(p => !p.Excluido)
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        /// <summary>
        /// utilizando string interpolada para informar um comando sql com parametros
        /// </summary>
        static void ConsultaInterolada()
        {
            using var db = new ApplicationDbContext();
            //Setup(db);

            //forma 1
            var id = 0;

            //forma 2
            //var id = new SqlParameter
            //{
            //    Value = 1,
            //    SqlDbType = SqlDbType.Int
            //};
            var departamentos = db.Departamentos
                .FromSqlInterpolated($"SELECT * FROM Departamentos WHERE Id>{id}")
                .Where(p => !p.Excluido)
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        /// <summary>
        /// enviando comentários para o servidor
        /// </summary>
        static void ConsultaComTag()
        {
            using var db = new ApplicationDbContext();
            Setup(db);

            var departamentos = db.Departamentos
                .TagWith(@"Estou enviando um comentario para o servidor
                
                Segundo comentario
                Terceiro comentario")
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        static void Setup(ApplicationDbContext db)
        {
            //db.Database.EnsureDeleted();
            if (db.Database.EnsureCreated())
            {
                db.Departamentos.AddRange(
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 01",
                        Funcionarios = new System.Collections.Generic.List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Rafael Almeida",
                                CPF = "99999999911",
                                RG= "2100062"
                            }
                        },
                        Excluido = true
                    },
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 02",
                        Funcionarios = new System.Collections.Generic.List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Bruno Brito",
                                CPF = "88888888811",
                                RG= "3100062"
                            },
                            new Funcionario
                            {
                                Nome = "Eduardo Pires",
                                CPF = "77777777711",
                                RG= "1100062"
                            }
                        }
                    });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
    }
}
