using System;
using System.Linq;
using Estudos.EFCore.Transacoes.Data;
using Estudos.EFCore.Transacoes.Domain;

namespace Estudos.EFCore.Transacoes
{
    class Program
    {
        static void Main(string[] args)
        {
            ComportamentoPadrao();
        }

        /// <summary>
        /// por padrão, todas as operaçãos são execuatadas dentro de uma transação pelo EFCore
        /// </summary>
        static void ComportamentoPadrao()
        {
            CadastrarLivro();

            using (var db = new ApplicationDbContext())
            {
                var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                livro.Autor = "Rafael Almeida";

                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "Dominando o Entity Framework Core",
                        Autor = "Rafael Almeida"
                    });

                db.SaveChanges();
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
                        Autor = "Rafael"
                    });

                db.SaveChanges();
            }
        }
    }
}
