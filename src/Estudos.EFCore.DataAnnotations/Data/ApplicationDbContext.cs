using System;
using System.Collections.Generic;
using Estudos.EFCore.DataAnnotations.Domain;
using Estudos.EFCore.DataAnnotations.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Estudos.EFCore.DataAnnotations.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext()
        {
            _configuration = ConfigurationHelper.ObterConfiguration();
        }

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
            

            #region Aplicando Configurações

            //forma 1
            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());

            //forma 2
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //forma 3
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);


            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configuracoes", b =>
            {
                b.Property<int>("Id");

                b.Property<string>("Chave")
                    .HasColumnType("VARCHAR(40)")
                    .IsRequired();

                b.Property<string>("Valor")
                    .HasColumnType("VARCHAR(255)")
                    .IsRequired();
            });

            #endregion

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