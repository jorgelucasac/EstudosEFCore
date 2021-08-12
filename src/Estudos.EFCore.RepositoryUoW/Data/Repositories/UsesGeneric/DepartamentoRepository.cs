using System.Threading.Tasks;
using Estudos.EFCore.RepositoryUoW.Data.Repositories.Base;
using Estudos.EFCore.RepositoryUoW.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.RepositoryUoW.Data.Repositories.UsesGeneric
{
    public class DepartamentoGenericGenericRepository : GenericRepository<Departamento>, IDepartamentoGenericRepository
    {
        public DepartamentoGenericGenericRepository(ApplicationContext context) : base(context) { }
    }
}