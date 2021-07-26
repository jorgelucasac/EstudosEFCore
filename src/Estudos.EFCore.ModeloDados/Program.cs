using System;
using Estudos.EFCore.ModeloDados.Data;

namespace Estudos.EFCore.ModeloDados
{
    class Program
    {
        static void Main(string[] args)
        {
            Collations();
        }

        static void Collations()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
