using System;
using System.Linq;
using Estudos.EFCore.Transacoes.Data;
using Estudos.EFCore.Transacoes.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Transacoes
{
    class Program
    {
        static void Main(string[] args)
        {
            //ComportamentoPadrao();
            //GerenciandoTransacaoManualmente();
            //ReverterTransacao();
            SalvarPontoTransacao();
        }

        /// <summary>
        /// por padrão, todas as operações são execuatadas dentro de uma transação pelo EFCore
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


        static void GerenciandoTransacaoManualmente()
        {
            CadastrarLivro();

            using (var db = new ApplicationDbContext())
            {

                //iniciando a transação
                var transacao = db.Database.BeginTransaction();

                var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                livro.Autor = "Rafael Almeida";
                db.SaveChanges();

                Console.ReadKey();

                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "Dominando o Entity Framework Core",
                        Autor = "Rafael Almeida"
                    });

                db.SaveChanges();

                //caso não seja chamado o commit o EFCore faz o rollback automaticamente
                transacao.Commit();
            }
        }


        static void ReverterTransacao()
        {
            CadastrarLivro();

            using (var db = new ApplicationDbContext())
            {
                var transacao = db.Database.BeginTransaction();

                try
                {
                    var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                    livro.Autor = "Rafael Almeida";
                    db.SaveChanges();

                    db.Livros.Add(
                        new Livro
                        {
                            Titulo = "Dominando o Entity Framework Core",
                            Autor = "Rafael Almeida".PadLeft(16, '*')
                        });

                    db.SaveChanges();

                    transacao.Commit();
                }
                catch (Exception e)
                {
                    transacao.Rollback();
                }

            }
        }

        static void SalvarPontoTransacao()
        {
            CadastrarLivro();

            using (var db = new ApplicationDbContext())
            {
                using var transacao = db.Database.BeginTransaction();

                try
                {
                    var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                    livro.Autor = "Rafael Almeida";
                    db.SaveChanges();

                    //salva o estado da transção
                    transacao.CreateSavepoint("desfazer_apenas_insercao");

                    db.Livros.Add(
                        new Livro
                        {
                            Titulo = "ASP.NET Core Enterprise Applications",
                            Autor = "Eduardo Pires"
                        });
                    db.SaveChanges();

                    db.Livros.Add(
                        new Livro
                        {
                            Titulo = "Dominando o Entity Framework Core",
                            Autor = "Rafael Almeida".PadLeft(16, '*')
                        });
                    db.SaveChanges();

                    transacao.Commit();
                }
                catch (DbUpdateException e)
                {
                    //realizando o rollback para um ponto especifico
                    transacao.RollbackToSavepoint("desfazer_apenas_insercao");

                    //caso exista entidades add, realiza o commit 
                    if (e.Entries.Count(p => p.State == EntityState.Added) == e.Entries.Count)
                    {
                        transacao.Commit();
                    }
                }

            }
        }
    }
}
