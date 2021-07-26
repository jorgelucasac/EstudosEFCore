using System;
using Estudos.EFCore.ModeloDados.Domain;
using Estudos.EFCore.ModeloDados.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Estudos.EFCore.ModeloDados.Data
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
            /*
            //SQL_Latin1_General: designador de agrupamentos - (especifica ás regras básicas de agrupamento)
            //CP1 : codificação ANSI
            //CI  : case insensitive
            //CS  : case sensitive
            //AI  : acentuação insensitive 
            //AS  : acentuação sensitive 
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            //setando configurações de collation para um propriedade especifica
            modelBuilder.Entity<Departamento>().Property(p => p.Descricao).UseCollation("SQL_Latin1_General_CP1_CS_AS");


            //tipos aceitos MsSQL: int, long, byte, decimal
            modelBuilder.HasSequence<int>("MinhaSequencia", "sequencias")
                .StartsAt(100)
                .IncrementsBy(2)
                .HasMin(100)
                .HasMax(110)
                .IsCyclic();// reinicia o contador da sequencia

            //utilizando a sequencia
            modelBuilder.Entity<Departamento>().Property(p => p.Id).HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia");
            */

            //criando im indice
            modelBuilder.Entity<Departamento>()
                .HasIndex(d => new { d.Descricao, d.Ativo })
                .IsUnique()//infoma que os calores do indice não se repetem
                .HasFilter("Descricao IS NOT NULL")// indexa apenas os valores não nulos
                .HasFillFactor(80)// o quanto da folha de indexação será utilizada
                .HasDatabaseName("idx_meu_indice_composto");
        }

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        public void EscreverLogSql(string sql)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(sql);
            Console.WriteLine("\n\n");
            Console.ResetColor();
        }

    }
}