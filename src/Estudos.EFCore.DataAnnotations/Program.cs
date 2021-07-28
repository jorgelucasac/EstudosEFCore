using System;
using Estudos.EFCore.DataAnnotations.Data;
using Estudos.EFCore.DataAnnotations.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.DataAnnotations
{
    class Program
    {
        static void Main(string[] args)
        {
            Atributos();
        }

        static void Atributos()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var script = db.Database.GenerateCreateScript();

                Console.WriteLine(script);

                //db.Atributos.Add(new Atributo
                //{
                //    Descricao = "Exemplo",
                //    Observacao = "Observacao"
                //});

                //db.SaveChanges();
            }
        }
    }
}
