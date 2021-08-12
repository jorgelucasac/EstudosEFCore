using System;
using System.Threading.Tasks;
using Estudos.EFCore.RepositoryUoW.Data.Repositories;

namespace Estudos.EFCore.RepositoryUoW.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync();
        IDepartamentoRepository DepartamentoRepository { get; }

    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private IDepartamentoRepository _departamentoRepository;
        public IDepartamentoRepository DepartamentoRepository => _departamentoRepository ??= new DepartamentoRepository(_context);

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}