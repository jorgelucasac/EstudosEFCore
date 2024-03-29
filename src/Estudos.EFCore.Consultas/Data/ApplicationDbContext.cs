﻿using System;
using Estudos.EFCore.Consultas.Domain;
using Estudos.EFCore.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Estudos.EFCore.Consultas.Data
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
                .UseSqlServer(
                    _configuration.GetConnectionString("SqlServerConnection")
                    , builder => builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))

                .EnableSensitiveDataLogging()
                //.LogTo(Console.WriteLine, LogLevel.Information);
                .LogTo(EscreverSql, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //informa que não deve ser retornado os registros excluídos
            //modelBuilder.Entity<Departamento>().HasQueryFilter(p => !p.Excluido);
        }

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        public void EscreverSql(string sql)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(sql);
            Console.WriteLine("\n\n");
            Console.ResetColor();
        }
    }
}