using System;
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
            //EnsureCreate();
            //EnsureDeleted();
            //GapDoEnsureCreate();
            HealthCheckBancoDeDados();
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
    }
}
