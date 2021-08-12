using Estudos.EFCore.RepositoryUoW.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.RepositoryUoW.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}
