using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Hangfire.Dashboard.Authorization
{
    public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        internal readonly static string[] AllowRoutePrefixs = new string[]
        {
            "/stats",
            "/js",
            "/css",
            "/fonts"
        };
        public bool Authorize([NotNull] DashboardContext context)
        {
            if (AllowRoutePrefixs.Any(url => context.Request.Path.StartsWith(url)))
            {
                return true;
            }

            var httpContext = context.GetHttpContext();
            var permissionChecker = httpContext.RequestServices.GetRequiredService<IPermissionChecker>();
            return AsyncHelper.RunSync(async () => 
                await permissionChecker.IsGrantedAsync(httpContext.User, HangfireDashboardPermissions.Dashboard.Default));
        }
    }
}
