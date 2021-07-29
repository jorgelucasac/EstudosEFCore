using System;
using System.Linq;
using Estudos.EFCore.Udfs.Data;
using Estudos.EFCore.Udfs.Domain;
using Estudos.EFCore.Udfs.Funcoes;

namespace Estudos.EFCore.Udfs
{
    class Program
    {
        static void Main(string[] args)
        {
           FuncaoLEFT();
        }

        static void FuncaoLEFT()
        {
            CadastrarLivro();

            using var db = new ApplicationDbContext();

            var resultado = db.Livros.Select(p => MinhasFuncoes.Left(p.Titulo, 10));
            foreach (var parteTitulo in resultado)
            {
                Console.WriteLine(parteTitulo);
            }
        }

        static void CadastrarLivro()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "Introdução ao Entity Framework Core",
                        Autor = "Rafael",
                        CadastradoEm = DateTime.Now.AddDays(-1)
                    });

                db.SaveChanges();
            }
        }
    }
}
