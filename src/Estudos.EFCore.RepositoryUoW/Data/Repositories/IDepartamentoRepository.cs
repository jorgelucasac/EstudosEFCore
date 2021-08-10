using System.Threading.Tasks;
using Estudos.EFCore.RepositoryUoW.Domain;

namespace Estudos.EFCore.RepositoryUoW.Data.Repositories
{
    public interface IDepartamentoRepository
    {
        Task<Departamento> GetByIdAsync(int id);
        Task Add(Departamento departamento);
        Task<bool> Save();
    }
}
