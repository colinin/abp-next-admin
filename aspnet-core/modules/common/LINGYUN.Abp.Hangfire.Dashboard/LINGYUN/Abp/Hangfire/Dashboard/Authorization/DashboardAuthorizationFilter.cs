using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Hangfire.Dashboard.Authorization
{
    public class DashboardAuthorizationFilter : IDashboardAsyncAuthorizationFilter
    {
        private readonly string[] _requiredPermissionNames;

        public DashboardAuthorizationFilter(params string[] requiredPermissionNames)
        {
            _requiredPermissionNames = requiredPermissionNames;
        }

        public async Task<bool> AuthorizeAsync(DashboardContext context)
        {
            if (!IsLoggedIn(context))
            {
                return false;
            }

            if (_requiredPermissionNames.IsNullOrEmpty())
            {
                return true;
            }

            return await IsPermissionGrantedAsync(context, _requiredPermissionNames);
        }

        private static bool IsLoggedIn(DashboardContext context)
        {
            var currentUser = context.GetHttpContext().RequestServices.GetRequiredService<ICurrentUser>();
            return currentUser.IsAuthenticated;
        }

        private static async Task<bool> IsPermissionGrantedAsync(DashboardContext context, string[] requiredPermissionNames)
        {
            var permissionChecker = context.GetHttpContext().RequestServices.GetRequiredService<IDashboardPermissionChecker>();
            return await permissionChecker.IsGrantedAsync(context, requiredPermissionNames);
        }
    }
}
