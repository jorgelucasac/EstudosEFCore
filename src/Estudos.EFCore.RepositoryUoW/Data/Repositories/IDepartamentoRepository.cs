using System.Threading.Tasks;
using Estudos.EFCore.RepositoryUoW.Domain;

namespace Estudos.EFCore.RepositoryUoW.Data.Repositories
{
    public interface IDepartamentoRepository
    {
        Task<Departamento> GetByIdAsync(int id);
        Task AddAsync(Departamento departamento);
        Task<bool> SaveAsync();
    }
}
