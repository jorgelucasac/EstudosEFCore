using System;
using System.Linq;
using Estudos.EFCore.Infraestrutura.Data;
using Estudos.EFCore.Infraestrutura.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Infraestrutura
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConsultarDepartamentos();
            //DadosSensiveis();
            //HabilitandoBatchSize();
            TempoComandoGeral();
        }

        static void DadosSensiveis()
        {
            using var db = new ApplicationDbContext();

            var descricao = "Departamento";
            var departamentos = db.Departamentos.Where(p => p.Descricao == descricao).ToArray();
        }

        static void ConsultarDepartamentos()
        {
            using var db = new ApplicationDbContext();

            var departamentos = db.Departamentos.Where(p => p.Id > 0).ToArray();
        }

        static void HabilitandoBatchSize()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            for (var i = 0; i < 50; i++)
            {
                db.Departamentos.Add(
                    new Departamento
                    {
                        Descricao = "Departamento " + i
                    });
            }

            db.SaveChanges();
        }

        static void TempoComandoGeral()
        {
            using var db = new ApplicationDbContext();

            db.Database.SetCommandTimeout(10);

            //db.Database.ExecuteSqlRaw("SELECT 1");
            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07';SELECT 1");
        }


        //deve ser utilizado quando a aplicação estiver com o retry ativado e 
        //a instrução utilizar transação explicita
        static void ExecutarEstrategiaResiliencia()
        {
            using var db = new ApplicationDbContext();

            var strategy = db.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = db.Database.BeginTransaction();

                db.Departamentos.Add(new Departamento { Descricao = "Departamento Transacao" });
                db.SaveChanges();

                transaction.Commit();
            });

        }

    }
}
