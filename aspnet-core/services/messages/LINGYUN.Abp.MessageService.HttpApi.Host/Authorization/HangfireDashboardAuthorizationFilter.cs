using Hangfire.Annotations;
using Hangfire.Dashboard;
using LINGYUN.Abp.MessageService.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.MessageService.Authorization
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var permissionChecker = httpContext.RequestServices.GetService<IPermissionChecker>();

            if (permissionChecker != null)
            {
                // 可以详细到每个页面授权,这里就免了
                return AsyncHelper.RunSync(async () => await permissionChecker.IsGrantedAsync(AbpMessageServicePermissions.Hangfire.ManageQueue));
            }
            return new LocalRequestsOnlyAuthorizationFilter().Authorize(context);
        }

        public override int GetHashCode()
        {
            // 类型相同就行了
            return GetType().FullName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            // 类型相同就行了
            if (GetType().Equals(obj.GetType()))
            {
                return true;
            }
            return base.Equals(obj);
        }
    }
}
