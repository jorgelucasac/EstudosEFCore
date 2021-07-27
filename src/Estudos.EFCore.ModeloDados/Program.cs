using System;
using System.Linq;
using Estudos.EFCore.ModeloDados.Data;
using Estudos.EFCore.ModeloDados.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.ModeloDados
{
    class Program
    {
        static void Main(string[] args)
        {
            //Collations();
            //PropagarDados();
            //Schema();
            //ConversorValores();
            PropriedadesDeSombra();
            TrabalhandoComPropriedadesDeSombra();
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

        static void ConversorValores() => Schema();
        static void ConversorCustomizado()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Conversores.Add(
                new Conversor
                {
                    Status = Status.Devolvido,
                });

            db.SaveChanges();

            var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Analise);

            var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Devolvido);
        }

        static void PropriedadesDeSombra()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        static void TrabalhandoComPropriedadesDeSombra()
        {
            using var db = new ApplicationDbContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //var departamento = new Departamento
            //{
            //    Descricao = "Departamento Propriedade de Sombra"
            //};

            //db.Departamentos.Add(departamento);
            ////atribuindo valor a propriedade de sombra
            //db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;

            //db.SaveChanges();

            //consuntando pela propriedade de sombra
            var departamentos = db.Departamentos.Where(p => EF.Property<DateTime>(p, "UltimaAtualizacao") < DateTime.Now).ToArray();
        }
    }


}
