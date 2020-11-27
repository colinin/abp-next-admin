using LINGYUN.Abp.BackendAdmin.Tests;

namespace Microsoft.AspNetCore.Builder
{
    public static class ProxyConnectTestMiddlewareExtensions
    {
        public static IApplicationBuilder UseProxyConnectTest(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ProxyConnectTestMiddleware>();
        }
    }
}
