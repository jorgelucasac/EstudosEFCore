using System;
using Microsoft.AspNetCore.Http;

namespace Estudos.EFCore.MultiTenant.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetTenantId(this HttpContext httpContext)
        {
            // desenvolvedor.io/tenant-1/product -> " " / "tenant-1" / "product"
            // desenvolvedor.io/product/?tenantId=tenant-1

            if (httpContext.Request.Path.Value == null) return string.Empty;

            var tenant = httpContext.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries)[2];

            return tenant;

        }
        
    }
}