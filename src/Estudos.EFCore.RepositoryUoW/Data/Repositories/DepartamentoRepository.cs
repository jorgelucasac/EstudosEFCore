using System.Threading.Tasks;
using Estudos.EFCore.RepositoryUoW.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.RepositoryUoW.Data.Repositories
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<Departamento> _dbSet;

        public DepartamentoRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Departamento>();
        }

        public async Task<Departamento> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Colaboradores)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Departamento departamento)
        {
            await _dbSet.AddAsync(departamento);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}