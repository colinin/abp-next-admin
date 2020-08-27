using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUglify.Helpers;
using System.Linq;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.MessageService.Authorization
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        protected string[] AllowGrantPath { get; }
        public HangfireDashboardAuthorizationFilter()
        {
            AllowGrantPath = new string[] { "/css", "/js", "/fonts", "/stats" };
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            // 本地请求
            if (LocalRequestOnlyAuthorize(context))
            {
                return true;
            }

            // 放行路径
            if (AllowGrantPath.Contains(context.Request.Path))
            {
                return true;
            }
            var httpContext = context.GetHttpContext();

            var options = httpContext.RequestServices.GetService<IOptions<HangfireDashboardRouteOptions>>()?.Value;

            if (options != null)
            {
                // 请求路径对应的权限检查
                // TODO: 怎么来传递用户身份令牌?
                var permission = options.GetPermission(context.Request.Path);
                if (!permission.IsNullOrWhiteSpace())
                {
                    var permissionChecker = httpContext.RequestServices.GetRequiredService<IPermissionChecker>();
                    return AsyncHelper.RunSync(async () => await permissionChecker.IsGrantedAsync(permission));
                }
            }

            return false;
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

        protected virtual bool LocalRequestOnlyAuthorize(DashboardContext context)
        {
            if (string.IsNullOrEmpty(context.Request.RemoteIpAddress))
            {
                return false;
            }

            if (context.Request.RemoteIpAddress == "127.0.0.1" || context.Request.RemoteIpAddress == "::1")
            {
                return true;
            }

            if (context.Request.RemoteIpAddress == context.Request.LocalIpAddress)
            {
                return true;
            }

            return false;
        }
    }
}
