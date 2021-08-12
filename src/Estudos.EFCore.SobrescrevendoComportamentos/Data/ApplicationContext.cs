using System;
using Estudos.EFCore.SobrescrevendoComportamentos.Entities;
using Estudos.EFCore.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Estudos.EFCore.SobrescrevendoComportamentos.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Departamento> Departamentos { get; set; }

        public ApplicationContext()
        {
            _configuration = ConfigurationHelper.ObterConfiguration();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_configuration.GetConnectionString("SqlServerConnection"),
                    opt =>
                        //habilitando a quantidade de dados por comando enviados ao banco de dados
                        opt.MaxBatchSize(50)
                            //forcando o uso do SplitQuery
                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                //informando o comportamento do Trackin para todas as consultas
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
                //habilitando detalhes de erros
                .EnableDetailedErrors()
                //habilitando visualização de dados sensiveis
                .EnableSensitiveDataLogging()
                //habilitando a exibição dos logs
                .LogTo(EscreverLogSql, LogLevel.Information)
                
                //usando os geradores de código
                .ReplaceService<IQuerySqlGeneratorFactory, MySqlServerQuerySqlGeneratorFactory>();
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