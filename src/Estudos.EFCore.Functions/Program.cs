﻿using System;
using System.Linq;
using Estudos.EFCore.Functions.Data;
using Estudos.EFCore.Functions.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Functions
{
    class Program
    {
        static void Main(string[] args)
        {
            //FuncoesDeDatas();
            //FuncaoLike();
            //FuncaoDataLength();

            //FuncaoProperty();
            FuncaoCollate();
        }


        static void ApagarCriarBancoDeDados()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Funcoes.AddRange(
                new Funcao
                {
                    Data1 = DateTime.Now.AddDays(2),
                    Data2 = "2021-01-01",
                    Descricao1 = "Bala 1 ",
                    Descricao2 = "Bala 1 "
                },
                new Funcao
                {
                    Data1 = DateTime.Now.AddDays(1),
                    Data2 = "XX21-01-01",
                    Descricao1 = "Bola 2",
                    Descricao2 = "Bola 2"
                },
                new Funcao
                {
                    Data1 = DateTime.Now.AddDays(1),
                    Data2 = "XX21-01-01",
                    Descricao1 = "Tela",
                    Descricao2 = "Tela"
                });

            db.SaveChanges();
        }

        static void FuncoesDeDatas()
        {
            ApagarCriarBancoDeDados();

            using (var db = new ApplicationDbContext())
            {
                var script = db.Database.GenerateCreateScript();

                Console.WriteLine(script);

                var dados = db.Funcoes.AsNoTracking().Select(p =>
                    new
                    {
                        Dias = EF.Functions.DateDiffDay(DateTime.Now, p.Data1),
                        Meses = EF.Functions.DateDiffMonth(DateTime.Now, p.Data1),
                        Data = EF.Functions.DateFromParts(2021, 1, 2),
                        DataValida = EF.Functions.IsDate(p.Data2),
                    });

                foreach (var f in dados)
                {
                    Console.WriteLine(f);
                }

            }
        }

        static void FuncaoLike()
        {
            using (var db = new ApplicationDbContext())
            {
                var script = db.Database.GenerateCreateScript();

                Console.WriteLine(script);

                var dados = db
                    .Funcoes
                    .AsNoTracking()
                    //.Where(p => EF.Functions.Like(p.Descricao1, "Bo%"))
                    //tudo que começa com 'Ba' ou 'Bo'
                    .Where(p => EF.Functions.Like(p.Descricao1, "B[ao]%"))
                    .Select(p => p.Descricao1)
                    .ToArray();


                var dadosContain = db
                    .Funcoes
                    .AsNoTracking()
                    //.Where(p => p.Descricao1.StartsWith("Bo"))
                    //não funciona
                    .Where(p => p.Descricao1.StartsWith("B[ao]%"))
                    .Select(p => p.Descricao1)
                    .ToArray();


                Console.WriteLine("Resultado:");
                foreach (var descricao in dados)
                {
                    Console.WriteLine(descricao);
                }
            }
        }

        /// <summary>
        /// consultar o total de bytes
        /// </summary>
        static void FuncaoDataLength()
        {
            using (var db = new ApplicationDbContext())
            {
                var resultado = db
                    .Funcoes
                    .AsNoTracking()
                    .Select(p => new
                    {
                        //DataLength - quantos bytes estão sendo utilizados pelo campo
                        TotalBytesCampoData = EF.Functions.DataLength(p.Data1),
                        TotalBytes1 = EF.Functions.DataLength(p.Descricao1),
                        TotalBytes2 = EF.Functions.DataLength(p.Descricao2),
                        //quantos caracteres estão sendo utilizados pelo campo
                        //sem contar espações nas pontas, como um trim()
                        Total1 = p.Descricao1.Length,
                        Total2 = p.Descricao2.Length
                    })
                    .FirstOrDefault();

                Console.WriteLine("Resultado:");

                Console.WriteLine(resultado);
            }
        }

        static void FuncaoProperty()
        {
            ApagarCriarBancoDeDados();

            using (var db = new ApplicationDbContext())
            {
                //consultando pela propriedade de sombra
                var resultado = db
                    .Funcoes
                    //.AsNoTracking()
                    .FirstOrDefault(p => EF.Property<string>(p, "PropriedadeSombra") == "Teste");

                //obtendo o valor da propriedade sombra
                //só funciona sem o AsNoTracking
                //pq com sem o rastreamento da entidade o EF perde
                //os valores das propriedades de sombra
                var propriedadeSombra = db
                    .Entry(resultado)
                    .Property<string>("PropriedadeSombra")
                    .CurrentValue;

                Console.WriteLine("Resultado:");
                Console.WriteLine(propriedadeSombra);
            }
        }

        static void FuncaoCollate()
        {
            using (var db = new ApplicationDbContext())
            {

                //CS - case sensitive
                var consulta1 = db
                    .Funcoes
                    .FirstOrDefault(p => EF.Functions.Collate(p.Descricao1, "SQL_Latin1_General_CP1_CS_AS") == "tela");

                //CI - case insensitive
                var consulta2 = db
                    .Funcoes
                    .FirstOrDefault(p => EF.Functions.Collate(p.Descricao1, "SQL_Latin1_General_CP1_CI_AS") == "tela");

                Console.WriteLine($"Consulta1: {consulta1?.Descricao1}");

                Console.WriteLine($"Consulta2: {consulta2?.Descricao1}");
            }
        }
    }
}
