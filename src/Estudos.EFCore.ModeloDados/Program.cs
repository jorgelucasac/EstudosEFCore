using System;
using Estudos.EFCore.ModeloDados.Data;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.ModeloDados
{
    class Program
    {
        static void Main(string[] args)
        {
            //Collations();
            //PropagarDados();
            Schema();
        }

        static void Collations()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        static void PropagarDados()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        static void Schema()
        {
            using var db = new ApplicationDbContext();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }
    }
}
