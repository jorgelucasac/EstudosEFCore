using System.Threading.Tasks;
using Estudos.EFCore.RepositoryUoW.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.RepositoryUoW.Data.Repositories
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly ApplicationContext _context;

        public DepartamentoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Departamento> GetByIdAsync(int id)
        {
            return await _context.Departamentos
                .Include(p => p.Colaboradores)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Add(Departamento departamento)
        {
            await _context.Departamentos.AddAsync(departamento);
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}