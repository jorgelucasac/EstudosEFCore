using System;
using System.IO;
using Estudos.EFCore.Infraestrutura.Domain;
using Estudos.EFCore.Infraestrutura.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Estudos.EFCore.Infraestrutura.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly StreamWriter _writer;

        public ApplicationDbContext()
        {
            _configuration = ConfigurationHelper.ObterConfiguration();
            var nomeArquivo = DateTime.Now.ToString("dd-MM-yyyy");
            _writer = new StreamWriter($"log_{nomeArquivo}.txt", append: true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_configuration.GetConnectionString("SqlServerConnection"))
                .LogTo(_writer.WriteLine, LogLevel.Information);


            //habilitando a exibição dos logs
            //.LogTo(EscreverLogSql, LogLevel.Information)


            //.LogTo(
            //    //finção que escreve os logs no console
            //    EscreverLogSql,
            //    //tipos de log que deve ser exibidos
            //    new[] { RelationalEventId.CommandExecuted, CoreEventId.ContextInitialized },
            //    //nivel dos logs exibidos
            //    LogLevel.Information,
            //    //opções adicionais de como os logs deve ser exibidos
            //    DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine
            //    )
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

        public override void Dispose()
        {
            base.Dispose();
            _writer.Dispose();
        }
    }
}