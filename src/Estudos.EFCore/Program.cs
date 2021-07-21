using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Estudos.EFCore.Data;
using Estudos.EFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Estudos.EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("INICIANDO...");
            //EnsureCreate();
            //EnsureDeleted();
            //GapDoEnsureCreate();
            //HealthCheckBancoDeDados();

            //var any = new ApplicationDbContext().Departamentos.Any();
            //GerenciarEstadoDaConexao(false);
            //GerenciarEstadoDaConexao(true);

            //ExecuteSQL();
            //SqlInjection();

            //MigracoesPendentes();
            //AplicarMigracaoEmTempodeExecucao();
            TodasMigracoes();
        }

        static void EnsureCreate()
        {
            using var db = new ApplicationDbContext();
            //garante que o banco exista
            //cria o banco caso não exista
            db.Database.EnsureCreated();
        }

        static void EnsureDeleted()
        {
            using var db = new ApplicationDbContext();
            //garante que o banco foi apagado
            //dropa o banco caso exista
            db.Database.EnsureDeleted();
        }

        static void GapDoEnsureCreate()
        {
            using var db1 = new ApplicationDbContext();
            using var db2 = new ApplicationDbContextCidade();

            db1.Database.EnsureCreated();
            //não cria as tabelas pq o banco já existe
            db2.Database.EnsureCreated();

            //forcando a criação das tabelas do contexto
            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }

        static void HealthCheckBancoDeDados()
        {
            using var db = new ApplicationDbContext();
            //forma 3 (nova)
            var possoConectar = db.Database.CanConnect();

            Console.WriteLine(possoConectar ? "posso me connectar" : "Não posso me conectar:");


            //formas antigas
            //try
            //{
            //    // forma 1
            //    var connection = db.Database.GetDbConnection();
            //    connection.Open();

            //    //forma 2
            //    var any = db.Funcionarios.Any();

            //    Console.WriteLine("posso me connectar");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Não posso me conectar: " + e.Message);
            //}
        }

        static int _count;
        static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao)
        {
            using var db = new ApplicationDbContext();
            var time = Stopwatch.StartNew();

            var conexao = db.Database.GetDbConnection();

            //evento disparado sempre que a conexão muda de estado
            conexao.StateChange += (_, args) =>
            {
                if (args.CurrentState == ConnectionState.Open)
                    _count++;
            }; 

            // verifica se se deve abrir a conexão prematuramente
            //ou deixar o EF controlar
            if (gerenciarEstadoConexao)
                conexao.Open();

            for (var i = 0; i < 200; i++)
            {
                db.Departamentos.AsNoTracking().Any();
            }

            time.Stop();
            var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, {_count}";

           Console.WriteLine(mensagem);
            _count = 0;
        }

        static void ExecuteSQL()
        {
            using var db = new ApplicationDbContext();

            // Primeira Opcao
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                var linhasAfetadas1 = cmd.ExecuteNonQuery();
            }

            // Segunda Opcao
            var descricao = "TESTE";
            var linhasAfetadas2 = db.Database.ExecuteSqlRaw("update departamentos set descricao={0} where id=1", descricao);

            //Terceira Opcao
            var linhasAfetadas3 = db.Database.ExecuteSqlInterpolated($"update departamentos set descricao={descricao} where id=1");
        }

        static void SqlInjection()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.AddRange(
                new Departamento
                {
                    Descricao = "Departamento 01"
                },
                new Departamento
                {
                    Descricao = "Departamento 02"
                });
            db.SaveChanges();

            var descricao = "Teste ' or 1='1";
            
            //correta
            //db.Database.ExecuteSqlRaw("update departamentos set descricao='AtaqueSqlInjection' where descricao={0}",descricao);

            //inseguro
            db.Database.ExecuteSqlRaw($"update departamentos set descricao='AtaqueSqlInjection' where descricao='{descricao}'");
            foreach (var departamento in db.Departamentos.AsNoTracking())
            {
                Console.WriteLine($"Id: {departamento.Id}, Descricao: {departamento.Descricao}");
            }

            Console.WriteLine($"update departamentos set descricao='AtaqueSqlInjection' where descricao='{descricao}'");
        }

        static void MigracoesPendentes()
        {
            using var db = new ApplicationDbContext();

            var migracoesPendentes = db.Database.GetPendingMigrations().ToList();

            Console.WriteLine($"Total: {migracoesPendentes.Count}");

            foreach (var migracao in migracoesPendentes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }

        static void AplicarMigracaoEmTempodeExecucao()
        {
            EnsureDeleted();
            using var db = new ApplicationDbContext();

            db.Database.Migrate();
        }

        static void TodasMigracoes()
        {
            using var db = new ApplicationDbContext();

            var migracoes = db.Database.GetMigrations().ToList();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }
    }
}
