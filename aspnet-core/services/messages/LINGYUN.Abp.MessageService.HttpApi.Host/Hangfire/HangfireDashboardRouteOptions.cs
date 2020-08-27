using LINGYUN.Abp.MessageService.Permissions;
using System.Collections.Generic;
using System.Linq;

namespace Hangfire
{
    public class HangfireDashboardRouteOptions
    {
        public IList<string> AllowFrameOrigins { get; }
        public IDictionary<string, string> RoutePermissions { get; }
        public HangfireDashboardRouteOptions()
        {
            AllowFrameOrigins = new List<string>();
            RoutePermissions = new Dictionary<string, string>();
            InitDefaultRoutes();
        }

        public void WithOrigins(params string[] origins)
        {
            AllowFrameOrigins.AddIfNotContains(origins);
        }

        public void WithPermission(string route, string permission)
        {
            RoutePermissions.Add(route, permission);
        }

        public string GetPermission(string route)
        {
            var permission = RoutePermissions
                .Where(x => x.Key.StartsWith(route))
                .Select(x => x.Value)
                .FirstOrDefault();

            return permission;
        }

        private void InitDefaultRoutes()
        {
            WithPermission("/hangfire", AbpMessageServicePermissions.Hangfire.Default);
            WithPermission("/stats", AbpMessageServicePermissions.Hangfire.Default);
            WithPermission("/servers", AbpMessageServicePermissions.Hangfire.Default);
            WithPermission("/retries", AbpMessageServicePermissions.Hangfire.Default);
            WithPermission("/recurring", AbpMessageServicePermissions.Hangfire.Default);
            WithPermission("/jobs/enqueued", AbpMessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/processing", AbpMessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/scheduled", AbpMessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/failed", AbpMessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/deleted", AbpMessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/awaiting", AbpMessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/actions", AbpMessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/details", AbpMessageServicePermissions.Hangfire.ManageQueue);
        }
    }
}
