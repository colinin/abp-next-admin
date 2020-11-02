using LINGYUN.Abp.MessageService.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangfire
{
    public class HangfireDashboardRouteOptions
    {
        public IList<string> AllowFrameOrigins { get; }
        /// <summary>
        /// 白名单
        /// 添加网关地址
        /// </summary>
        public IList<string> WhiteList { get; }
        public IDictionary<string, string> RoutePermissions { get; }
        public HangfireDashboardRouteOptions()
        {
            WhiteList = new List<string>();
            AllowFrameOrigins = new List<string>();
            RoutePermissions = new Dictionary<string, string>();
            InitDefaultRoutes();
            WithWhite("127.0.0.1");
            WithWhite("::1");
        }

        public bool IpAllow(string ipaddress)
        {
            return WhiteList.Any(ip => ip == ipaddress);
        }

        public void WithWhite(params string[] wgites)
        {
            WhiteList.AddIfNotContains(wgites);
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
            WithPermission("/hangfire", MessageServicePermissions.Hangfire.Default);
            WithPermission("/stats", MessageServicePermissions.Hangfire.Default);
            WithPermission("/servers", MessageServicePermissions.Hangfire.Default);
            WithPermission("/retries", MessageServicePermissions.Hangfire.Default);
            WithPermission("/recurring", MessageServicePermissions.Hangfire.Default);
            WithPermission("/jobs/enqueued", MessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/processing", MessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/scheduled", MessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/failed", MessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/deleted", MessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/awaiting", MessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/actions", MessageServicePermissions.Hangfire.ManageQueue);
            WithPermission("/jobs/details", MessageServicePermissions.Hangfire.ManageQueue);
        }
    }
}
