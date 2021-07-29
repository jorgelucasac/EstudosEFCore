using System;
using System.Linq;
using System.Reflection;
using Estudos.EFCore.Udfs.Domain;
using Estudos.EFCore.Udfs.Funcoes;
using Estudos.EFCore.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Estudos.EFCore.Udfs.Data
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
            //registrando as funções
            //MinhasFuncoes.RegistarFuncoes(modelBuilder);

            modelBuilder.HasDbFunction(_funcaoLeftDb)
                .HasName("LEFT")//informa o nome da função no banco
                .IsBuiltIn();// informa que é uma função nativa do BD

            modelBuilder
                .HasDbFunction(_letrasMaiusculas)
                .HasName("ConverterParaLetrasMaiusculas")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(_dateDiff)
                .HasName("DATEDIFF")
                .HasTranslation(p =>
                {
                    var argumentos = p.ToList();

                    var contante = (SqlConstantExpression)argumentos[0];
                    argumentos[0] = new SqlFragmentExpression(contante.Value.ToString());

                    return new SqlFunctionExpression("DATEDIFF", argumentos, false, new[] { false, false, false }, typeof(int), null);

                })
                .IsBuiltIn();



        }

        public DbSet<Livro> Livros { get; set; }


        private static readonly MethodInfo _funcaoLeftDb = 
            typeof(MinhasFuncoes).GetRuntimeMethod(nameof(MinhasFuncoes.Left),
            new[] {typeof(string), typeof(int)})!;

        private static readonly MethodInfo _letrasMaiusculas = typeof(MinhasFuncoes)
            .GetRuntimeMethod(nameof(MinhasFuncoes.LetrasMaiusculas), new[] { typeof(string) });

        private static MethodInfo _dateDiff = typeof(MinhasFuncoes)
            .GetRuntimeMethod(nameof(MinhasFuncoes.DateDiff),
                new[] { typeof(string), typeof(DateTime), typeof(DateTime) });

        //informa que será traduzido para a função LEFT do banco de dados
        //[DbFunction(name: "LEFT", IsBuiltIn = true)]
        //public static string Left(string dados, int quantidade)
        //{
        //    throw new NotImplementedException();
        //}

        public void EscreverLogSql(string sql)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(sql);
            Console.WriteLine("\n\n");
            Console.ResetColor();
        }

    }
}