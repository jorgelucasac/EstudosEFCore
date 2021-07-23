using System;
using Estudos.EFCore.Consultas.Domain;
using Estudos.EFCore.Helper;
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
                .UseSqlServer(_configuration.GetConnectionString("SqlServerConnection"))
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
    }
}