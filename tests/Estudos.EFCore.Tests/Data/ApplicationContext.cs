using Estudos.EFCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Tests.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
    }
}