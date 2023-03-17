using Estudos.EFCore.Consultas.Data;
using Estudos.EFCore.Consultas.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Estudos.EFCore.Consultas
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //FiltroGlobal();
            //IgnorarFiltroGlobal();
            //ConsultaProjetada();
            //ConsultaParametrizada();
            //ConsultaIntepolada();
            //ConsultaComTag();
            EntendendoConsulta1NN1();
            //DivisaoDeConsulta();
            //CriarStoredProcedure();
            //InserirDadosViaProcedure();
            //CriarStoredProcedureDeConsulta();
            //ConsultaViaProcedure();

            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// utilizando um filtro global para retornar apenas os registros não excluídos
        /// </summary>
        private static void FiltroGlobal()
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
        private static void IgnorarFiltroGlobal()
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
        private static void ConsultaProjetada()
        {
            using var db = new ApplicationDbContext();
            Setup(db);

            var departamentos = db.Departamentos
                //.Where(p => p.Id > 0)
                .Where(p => p.Funcionarios.Any(x => x.Idade > 20))
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
        private static void ConsultaParametrizada()
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
        private static void ConsultaIntepolada()
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
        private static void ConsultaComTag()
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

        //diferença em consultas 1xN vs Nx1
        private static void EntendendoConsulta1NN1()
        {
            using var db = new ApplicationDbContext();
            Setup(db);
            // Nx1 -> INNER JOIN
            //retona os funcaionarios e o departamento do funcaionario
            //var funcionarios = db.Funcionarios
            //    .Include(p => p.Departamento)
            //    .ToList();

            //foreach (var funcionario in funcionarios)
            //{
            //    Console.WriteLine($"Nome: {funcionario.Nome} / Descricap Dep: {funcionario.Departamento.Descricao}");
            //}

            // 1xN -> LEFT JOIN
            //retona os departamentos e todos os funcaionarios de cada departamento
            var departamentos = db.Departamentos
                .AsNoTrackingWithIdentityResolution()
                .Include(f => f.Funcionarios.Where(x => x.Idade > 19))
                .Where(x => x.Funcionarios.Any())
                .ToList()
                .Select(x => new Retorno(x.Descricao, x.Funcionarios.Select(u => u.Nome)));

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"Nome: {funcionario}");
                }
            }
        }

        /// <summary>
        /// divide a consulta ao invés de usar join
        /// assim, evitar repetir os dados
        /// Pode ser configurado golbalmente no contexto
        ///
        /// 'AsSingleQuery' força a execução usando join
        /// caso 'SplitQuery' esteja  configurado globalmente
        /// </summary>
        private static void DivisaoDeConsulta()
        {
            using var db = new ApplicationDbContext();
            Setup(db);

            var departamentos = db.Departamentos
                .Include(p => p.Funcionarios)
                .Where(p => p.Id < 3)
                //.AsSplitQuery()
                .AsSingleQuery()
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\tNome: {funcionario.Nome}");
                }
            }
        }

        private static void CriarStoredProcedure()
        {
            var criarDepartamento = @"
            CREATE OR ALTER PROCEDURE CriarDepartamento
                @Descricao VARCHAR(50),
                @Ativo bit
            AS
            BEGIN
                INSERT INTO
                    Departamentos(Descricao, Ativo, Excluido)
                VALUES (@Descricao, @Ativo, 0)
            END
            ";

            using var db = new ApplicationDbContext();

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }

        private static void InserirDadosViaProcedure()
        {
            using var db = new ApplicationDbContext();

            db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", "Departamento Via Procedure", true);
        }

        private static void CriarStoredProcedureDeConsulta()
        {
            var criarDepartamento = @"
            CREATE OR ALTER PROCEDURE GetDepartamentos
                @Descricao VARCHAR(50)
            AS
            BEGIN
                SELECT * FROM Departamentos Where Descricao Like @Descricao + '%'
            END
            ";

            using var db = new ApplicationDbContext();

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }

        private static void ConsultaViaProcedure()
        {
            using var db = new ApplicationDbContext();

            var dep = new SqlParameter("@dep", "Departamento");

            var departamentos = db.Departamentos
                //.FromSqlRaw("EXECUTE GetDepartamentos @dep", dep)
                .FromSqlInterpolated($"EXECUTE GetDepartamentos {dep}")
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine(departamento.Descricao);
            }
        }

        private static void Setup(ApplicationDbContext db)
        {
            //db.Database.EnsureDeleted();
            if (db.Database.EnsureCreated())
            {
                db.Departamentos.AddRange(
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Rafael Almeida",
                                CPF = "99999999911",
                                RG= "2100062",
                                Idade = 20
                            }
                        },
                        Excluido = true
                    },
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 02",
                        Funcionarios = new List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Bruno Brito",
                                CPF = "88888888811",
                                RG= "3100062",
                                Idade = 25
                            },
                            new Funcionario
                            {
                                Nome = "Eduardo Pires",
                                CPF = "77777777711",
                                RG= "1100062",
                                Idade = 18
                            }
                        }
                    });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
    }
}