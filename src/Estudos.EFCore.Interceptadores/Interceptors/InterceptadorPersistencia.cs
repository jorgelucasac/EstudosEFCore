using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(eventData.Context.ChangeTracker.DebugView.LongView);
            Console.ResetColor();
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}