using Hangfire;
using Hangfire.Dashboard;
using LINGYUN.Abp.Hangfire.Dashboard.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Hangfire.Dashboard
{
    public class AbpHangfireDashboardOptionsProvider : ITransientDependency
    {
        public virtual DashboardOptions Get()
        {
            return new DashboardOptions
            {
                Authorization = new IDashboardAuthorizationFilter[]
                {
                    new DashboardAuthorizationFilter()
                },
                IsReadOnlyFunc = (context) =>
                {
                    var httpContext = context.GetHttpContext();
                    var permissionChecker = httpContext.RequestServices.GetRequiredService<IPermissionChecker>();

                    return !AsyncHelper.RunSync(async () =>
                        await permissionChecker.IsGrantedAsync(HangfireDashboardPermissions.Dashboard.ManageJobs));
                }
            };
        }
    }
}
