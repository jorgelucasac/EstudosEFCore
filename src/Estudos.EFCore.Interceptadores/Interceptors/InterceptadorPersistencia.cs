using Estudos.EFCore.Interceptadores.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.EFCore.Interceptadores.Interceptors
{
    public class InterceptadorPersistencia : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            //exibindo os objetos que estão sendo alterados
            Console.WriteLine(eventData.Context.ChangeTracker.DebugView.LongView);
            Console.ResetColor();

            var dbContext = eventData?.Context;

            AddAuditInfo(dbContext);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(eventData.Context.ChangeTracker.DebugView.LongView);
            Console.ResetColor();

            var dbContext = eventData?.Context;

            AddAuditInfo(dbContext);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void AddAuditInfo(DbContext dbContext)
        {
            if (dbContext is null)
            {
                return;
            }

            var audits = dbContext.ChangeTracker.Entries<IAuditEntity>();

            foreach (var entity in audits)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Property(x => x.DataCadastro).CurrentValue = DateTime.Now;
                }

                if (entity.State == EntityState.Modified)
                {
                    entity.Property(x => x.DataAtualizacao).CurrentValue = DateTime.Now;
                }
            }
        }
    }
}