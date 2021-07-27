using System;
using Estudos.EFCore.ModeloDados.Conversores;
using Estudos.EFCore.ModeloDados.Domain;
using Estudos.EFCore.ModeloDados.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            

            //criando im indice
            modelBuilder.Entity<Departamento>()
                .HasIndex(d => new { d.Descricao, d.Ativo })
                .IsUnique()//infoma que os calores do indice não se repetem
                .HasFilter("Descricao IS NOT NULL")// indexa apenas os valores não nulos
                .HasFillFactor(80)// o quanto da folha de indexação será utilizada
                .HasDatabaseName("idx_meu_indice_composto");

            //setando registros iniciais para a tabela estado
            modelBuilder.Entity<Estado>().HasData(new[]
            {
                new Estado { Id = 1, Nome = "Sao Paulo"},
                new Estado { Id = 2, Nome = "Sergipe"}
            });
           

            //definindo os schemas
            modelBuilder.HasDefaultSchema("cadastros");
            modelBuilder.Entity<Estado>().ToTable("Estados", "SegundoSchema");
             */

            //forma 3
            var conversao =
                new ValueConverter<Versao, string>(p => p.ToString(), p => (Versao)Enum.Parse(typeof(Versao), p));

            //Conversores disponiveis 
            //Microsoft.EntityFrameworkCore.Storage.ValueConversion.

            var conversao2 = new EnumToStringConverter<Versao>();


            modelBuilder.Entity<Conversor>()
                .Property(p => p.Versao)
                .HasConversion(conversao);
            //.HasConversion(conversao1);
            //forma 1
            //.HasConversion<string>();
            // forma 2
            //.HasConversion(p => p.ToString(), p => (Versao)Enum.Parse(typeof(Versao), p));

            modelBuilder.Entity<Conversor>()
                .Property(p => p.Status)
                .HasConversion(new ConversorCustomizado());

            //definindo uma propriedade de sombra
            modelBuilder.Entity<Departamento>().Property<DateTime>("UltimaAtualizacao");
        }

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Conversor> Conversores { get; set; }

        public void EscreverLogSql(string sql)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(sql);
            Console.WriteLine("\n\n");
            Console.ResetColor();
        }

    }
}