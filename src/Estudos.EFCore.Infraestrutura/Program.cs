using System;
using System.Linq;
using Estudos.EFCore.Infraestrutura.Data;
using Estudos.EFCore.Infraestrutura.Domain;

namespace Estudos.EFCore.Infraestrutura
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConsultarDepartamentos();
            //DadosSensiveis();
            HabilitandoBatchSize();
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
    }
}
