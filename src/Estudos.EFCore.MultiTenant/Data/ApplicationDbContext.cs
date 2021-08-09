using System;
using Estudos.EFCore.MultiTenant.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Estudos.EFCore.MultiTenant.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(conf =>
            {
                conf.HasKey(p => p.Id);
                conf.Property(p => p.Nome)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);// irá criar tipo varchar
            });
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