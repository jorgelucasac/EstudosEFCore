using System;
using Estudos.EFCore.Dicas.Data;
using Estudos.EFCore.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Estudos.EFCore.Dicas.Domain
{
    public class ApplicationContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationContext()
        {
            _configuration = ConfigurationHelper.ObterConfiguration();
        }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<UsuarioFuncao> UsuarioFuncoes { get; set; }
        public DbSet<DepartamentoRelatorio> DepartamentoRelatorio { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_configuration.GetConnectionString("SqlServerConnection"))
                //habilitando detalhes de erros
                .EnableDetailedErrors()
                //habilitando visualização de dados sensiveis
                .EnableSensitiveDataLogging()
                //habilitando a exibição dos logs
                .LogTo(EscreverLogSql, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //informando que não possui chave primária
            //modelBuilder.Entity<UsuarioFuncao>().HasNoKey();


            modelBuilder.Entity<DepartamentoRelatorio>(e =>
            {
                e.HasNoKey();

                e.ToView("vw_departamento_relatorio");

                e.Property(p => p.Departamento).HasColumnName("Descricao");
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