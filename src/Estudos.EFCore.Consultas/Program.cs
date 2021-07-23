using System;
using System.Linq;
using Estudos.EFCore.Consultas.Data;
using Estudos.EFCore.Consultas.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Consultas
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            //FiltroGlobal();
            //IgnorarFiltroGlobal();
            ConsultaProjetada();
            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// utilizando um filtro global para retornar apenas os registros não excluídos
        /// </summary>
        static void FiltroGlobal()
        {
            using var db = new ApplicationDbContext();
            Setup(db);

            var departamentos = db.Departamentos.Where(p => p.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }

        /// <summary>
        /// Ignorando o filtro global definido para o mapeamento
        /// </summary>
        static void IgnorarFiltroGlobal()
        {
            using var db = new ApplicationDbContext();
            Setup(db);

            var departamentos = db.Departamentos
                .IgnoreQueryFilters().Where(p => p.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }

        static void ConsultaProjetada()
        {
            using var db = new ApplicationDbContext();
            Setup(db);

            var departamentos = db.Departamentos
                .Where(p => p.Id > 0)
                .Select(p => new {p.Descricao, Funcionarios = p.Funcionarios.Select(a=> a.Nome)})
                .ToList();
                
            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\t Nome: {funcionario}");
                }
            }
        }

        static void Setup(ApplicationDbContext db)
        {
            //db.Database.EnsureDeleted();
            if (db.Database.EnsureCreated())
            {
                db.Departamentos.AddRange(
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 01",
                        Funcionarios = new System.Collections.Generic.List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Rafael Almeida",
                                CPF = "99999999911",
                                RG= "2100062"
                            }
                        },
                        Excluido = true
                    },
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 02",
                        Funcionarios = new System.Collections.Generic.List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Bruno Brito",
                                CPF = "88888888811",
                                RG= "3100062"
                            },
                            new Funcionario
                            {
                                Nome = "Eduardo Pires",
                                CPF = "77777777711",
                                RG= "1100062"
                            }
                        }
                    });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
    }
}
