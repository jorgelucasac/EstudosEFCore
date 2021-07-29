using System;
using System.Linq;
using Estudos.EFCore.Performance.Data;
using Estudos.EFCore.Performance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup();
            ConsultaRastreada();
            //ConsultaNaoRastreada();
            //ConsultaComResolucaoDeIdentidade();
        }

        /// <summary>
        /// Terá apenas uma instancia de Departamento em memória
        /// </summary>
        static void ConsultaRastreada()
        {
            using var db = new ApplicationDbContext();

            var funcionarios = db.Funcionarios
                .Include(p => p.Departamento)
                .ToList();
        }

        /// <summary>
        /// terá 100 instancias de departamento em memória, consumindo assim, mais recurso
        /// que a consulta rastreada
        /// </summary>
        static void ConsultaNaoRastreada()
        {
            using var db = new ApplicationDbContext();

            var funcionarios = db.Funcionarios.
                AsNoTracking()
                .Include(p => p.Departamento).ToList();

        }

        /// <summary>
        /// Terá apenas uma instancia de Departamento em memória
        /// e não ira rastrear as entidades, consumindo assim, menos recusos
        /// </summary>
        static void ConsultaComResolucaoDeIdentidade()
        {
            using var db = new ApplicationDbContext();

            var funcionarios = db.Funcionarios
                .AsNoTrackingWithIdentityResolution()
                .Include(p => p.Departamento)
                .ToList();
        }

        static void ConsultaCustomizada()
        {
            using var db = new ApplicationDbContext();

            //alterando o tracking para a instância do DbContext
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var funcionarios = db.Funcionarios
                //alterando o tracking para a consulta
                //.AsTracking()
                //.AsNoTracking()
                //.AsNoTrackingWithIdentityResolution()
                .Include(p => p.Departamento)
                .ToList();
        }

        static void Setup()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.Add(new Departamento
            {
                Descricao = "Departamento Teste",
                Ativo = true,
                Funcionarios = Enumerable.Range(1, 100).Select(p => new Funcionario
                {
                    CPF = p.ToString().PadLeft(11, '0'),
                    Nome = $"Funcionario {p}",
                    RG = p.ToString()
                }).ToList()
            });

            db.SaveChanges();
        }
    }
}
