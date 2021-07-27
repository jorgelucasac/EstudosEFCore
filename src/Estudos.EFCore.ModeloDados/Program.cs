using System;
using System.Linq;
using System.Text.Json;
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
            //PropriedadesDeSombra();
            //TrabalhandoComPropriedadesDeSombra();

            //TiposDePropriedades();
            Relacionamento1Para1();
            muitos;
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
            var departamentos = db.Departamentos
                .Where(p => EF.Property<DateTime>(p, "UltimaAtualizacao") < DateTime.Now).ToArray();
        }


        static void TiposDePropriedades()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var cliente = new Cliente
            {
                Nome = "Fulano de tal",
                Telefone = "(79) 98888-9999",
                Endereco = new Endereco {Bairro = "Centro", Cidade = "Sao Paulo"}
            };

            db.Clientes.Add(cliente);

            db.SaveChanges();

            var clientes = db.Clientes.AsNoTracking().ToList();

            var options = new JsonSerializerOptions {WriteIndented = true};

            clientes.ForEach(cli =>
            {
                var json = JsonSerializer.Serialize(cli, options);

                Console.WriteLine(json);
            });
        }


        static void Relacionamento1Para1()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var estado = new Estado
            {
                Nome = "Sergipe",
                Governador = new Governador {Nome = "Rafael Almeida"}
            };

            db.Estados.Add(estado);

            db.SaveChanges();

            var estados = db.Estados
                //.Include(e=> e.Governador)
                .AsNoTracking().ToList();

            estados.ForEach(est => { Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}"); });
        }

        static void Relacionamento1ParaMuitos()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var estado = new Estado
            {
                Nome = "Sergipe",
                Governador = new Governador {Nome = "Rafael Almeida"}
            };

            estado.Cidades.Add(new Cidade {Nome = "Itabaiana"});

            db.Estados.Add(estado);

            db.SaveChanges();

            var estados = db.Estados.Include(c => c.Cidades).ToList();
        }
    }


}
