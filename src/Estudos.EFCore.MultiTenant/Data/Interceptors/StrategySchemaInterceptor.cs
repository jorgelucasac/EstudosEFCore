using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Estudos.EFCore.MultiTenant.Provider;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Estudos.EFCore.MultiTenant.Data.Interceptors
{
    public class StrategySchemaInterceptor : DbCommandInterceptor
    {
        private readonly TenantData _tenantData;

        public StrategySchemaInterceptor(TenantData tenantData)
        {
            _tenantData = tenantData;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command, 
            CommandEventData eventData, 
            InterceptionResult<DbDataReader> result)
        {
            ReplaceSchema(command);
            
            return base.ReaderExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            ReplaceSchema(command);
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        private void ReplaceSchema(DbCommand command)
        {
            // FROM PRODUCTS -> FROM [tenant-1].PRODUCTS
            command.CommandText = command.CommandText
                .Replace("FROM ", $" FROM [{_tenantData.TenantId}].")
                .Replace("JOIN ", $" JOIN [{_tenantData.TenantId}].");
        }
    }
}