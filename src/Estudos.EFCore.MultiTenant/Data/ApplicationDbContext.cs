using System;
using Estudos.EFCore.MultiTenant.Domain;
using Estudos.EFCore.MultiTenant.Provider;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.MultiTenant.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly TenantData _tenantData;

        public readonly TenantData TenantData;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, TenantData tenantData) : base(options)
        {
            //forma 01 - campos nas tabelas
            _tenantData = tenantData;

            //forma 02
            TenantData = tenantData;
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(TenantData.TenantId);

            modelBuilder.Entity<Pessoa>().HasData(
                new Pessoa { Id = 1, Nome = "Pessoa 1", TenantId = "tenant-1" },
                new Pessoa { Id = 2, Nome = "Pessoa 2", TenantId = "tenant-2" },
                new Pessoa { Id = 3, Nome = "Pessoa 3", TenantId = "tenant-2" });

            modelBuilder.Entity<Produto>().HasData(
                new Produto { Id = 1, Descricao = "Descricao 1", TenantId = "tenant-1" },
                new Produto { Id = 2, Descricao = "Descricao 2", TenantId = "tenant-2" },
                new Produto { Id = 3, Descricao = "Descricao 3", TenantId = "tenant-2" });

            //forma 01
            //modelBuilder.Entity<Pessoa>().HasQueryFilter(p => p.TenantId == _tenantData.TenantId);
            //modelBuilder.Entity<Produto>().HasQueryFilter(p => p.TenantId == _tenantData.TenantId);

        }


        public void EscreverLogSql(string sql)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(sql);
            Console.WriteLine("\n\n");
            Console.ResetColor();
        }

    }
}