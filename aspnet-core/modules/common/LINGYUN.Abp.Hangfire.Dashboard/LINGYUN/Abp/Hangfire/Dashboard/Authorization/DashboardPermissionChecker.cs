using Hangfire.Dashboard;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Hangfire.Dashboard.Authorization
{
    public class DashboardPermissionChecker : IDashboardPermissionChecker, ITransientDependency
    {
        // 仪表板属于高频访问, 设定有效期的二级权限缓存
        private readonly IMemoryCache _memoryCache;
        private readonly IPermissionChecker _permissionChecker;

        public DashboardPermissionChecker(
            IMemoryCache memoryCache,
            IPermissionChecker permissionChecker)
        {
            _memoryCache = memoryCache;
            _permissionChecker = permissionChecker;
        }

        public virtual async Task<bool> IsGrantedAsync(DashboardContext context, string[] requiredPermissionNames)
        {
            var localPermissionKey = $"_HDPS:{requiredPermissionNames.JoinAsString(";")}";

            if (_memoryCache.TryGetValue(localPermissionKey, out MultiplePermissionGrantResult cacheItem))
            {
                return cacheItem.AllGranted;
            }

            cacheItem = await _permissionChecker.IsGrantedAsync(requiredPermissionNames);

            _memoryCache.Set(
                localPermissionKey,
                cacheItem,
                new MemoryCacheEntryOptions
                {
                    // 5分钟过期
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5d),
                });

            return cacheItem.AllGranted;
        }
    }
}
