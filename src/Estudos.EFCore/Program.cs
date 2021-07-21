using System;
using Estudos.EFCore.Domain;

namespace Estudos.EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //EnsureCreate();
            EnsureDeleted();
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


    }
}
