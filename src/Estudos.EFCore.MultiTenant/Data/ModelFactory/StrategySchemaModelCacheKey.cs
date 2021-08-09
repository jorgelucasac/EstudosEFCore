using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Estudos.EFCore.MultiTenant.Data.ModelFactory
{
    //public class StrategySchemaModelCacheKey : IModelCacheKeyFactory
    //{
    //    public object Create(DbContext context)
    //    {
    //        var model = new
    //        {
    //            Type = context.GetType(),
    //            //altera o schema
    //            Schema = (context as ApplicationDbContext)?.TenantData.TenantId
    //        };

    //        return model;
    //    }
    //}
}