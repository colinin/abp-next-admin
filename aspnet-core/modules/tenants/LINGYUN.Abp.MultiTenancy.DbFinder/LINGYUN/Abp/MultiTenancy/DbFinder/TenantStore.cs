using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Threading;

/*
 * fix bug: 不能在当前租户范围下来查询租户的连接配置信息,否则只会永远执行数据库查询
 * 
 */

namespace LINGYUN.Abp.MultiTenancy.DbFinder
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(ITenantStore))]
    public class TenantStore : ITenantStore
    {
        public ILogger<TenantStore> Logger { protected get; set; }
        private readonly IDistributedCache<TenantConfigurationCacheItem> _cache;
        private readonly ICurrentTenant _currentTenant;
        private readonly ITenantRepository _tenantRepository;

        public TenantStore(
            ICurrentTenant currentTenant,
            ITenantRepository tenantRepository,
            IDistributedCache<TenantConfigurationCacheItem> cache)
        {
            _cache = cache;
            _currentTenant = currentTenant;
            _tenantRepository = tenantRepository;

            Logger = NullLogger<TenantStore>.Instance;
        }

        public virtual TenantConfiguration Find(string name)
        {
            var tenantCacheItem = AsyncHelper.RunSync(async () => await 
                GetCacheItemByNameAsync(name));

            if (tenantCacheItem == null)
            {
                return null;
            }

            return new TenantConfiguration(tenantCacheItem.Id, tenantCacheItem.Name)
            {
                ConnectionStrings = tenantCacheItem.ConnectionStrings
            };
        }

        public virtual TenantConfiguration Find(Guid id)
        {
            var tenantCacheItem = AsyncHelper.RunSync(async () => await
                GetCacheItemByIdAsync(id));

            if (tenantCacheItem == null)
            {
                return null;
            }

            return new TenantConfiguration(tenantCacheItem.Id, tenantCacheItem.Name)
            {
                ConnectionStrings = tenantCacheItem.ConnectionStrings
            };
        }

        public virtual async Task<TenantConfiguration> FindAsync(string name)
        {
            var tenantCacheItem = await GetCacheItemByNameAsync(name);

            if (tenantCacheItem == null)
            {
                return null;
            }

            return new TenantConfiguration(tenantCacheItem.Id, tenantCacheItem.Name)
            {
                ConnectionStrings = tenantCacheItem.ConnectionStrings
            };
        }

        public virtual async Task<TenantConfiguration> FindAsync(Guid id)
        {
            var tenantCacheItem = await GetCacheItemByIdAsync(id);

            if (tenantCacheItem == null)
            {
                return null;
            }

            return new TenantConfiguration(tenantCacheItem.Id, tenantCacheItem.Name)
            {
                ConnectionStrings = tenantCacheItem.ConnectionStrings
            };
        }

        protected virtual async Task<TenantConfigurationCacheItem> GetCacheItemByIdAsync(Guid id)
        {
            using (_currentTenant.Change(null))
            {
                var cacheKey = TenantConfigurationCacheItem.CalculateCacheKey(id.ToString());

                Logger.LogDebug($"TenantStore.GetCacheItemByIdAsync: {cacheKey}");

                var cacheItem = await _cache.GetAsync(cacheKey);

                if (cacheItem != null)
                {
                    Logger.LogDebug($"Found in the cache: {cacheKey}");
                    return cacheItem;
                }
                Logger.LogDebug($"Not found in the cache, getting from the repository: {cacheKey}");

                var tenant = await _tenantRepository.FindAsync(id, true);
                if (tenant == null)
                {
                    Logger.LogWarning($"Can not found tenant by id: {id}");
                    // throw new AbpException($"Can not found tenant by id: {id}");
                    return null;
                }
                var connectionStrings = new ConnectionStrings();
                foreach (var tenantConnectionString in tenant.ConnectionStrings)
                {
                    connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
                }
                cacheItem = new TenantConfigurationCacheItem(tenant.Id, tenant.Name, connectionStrings);

                Logger.LogDebug($"Setting the cache item: {cacheKey}");
                await _cache.SetAsync(cacheKey, cacheItem);

                // 用租户名称再次缓存,以便通过标识查询也能命中缓存
                await _cache.SetAsync(TenantConfigurationCacheItem.CalculateCacheKey(tenant.Name), cacheItem);

                Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

                return cacheItem;
            }
        }
        protected virtual async Task<TenantConfigurationCacheItem> GetCacheItemByNameAsync(string name)
        {
            // 需要切换到最外层以解决查询无效的bug
            using (_currentTenant.Change(null))
            {
                var cacheKey = TenantConfigurationCacheItem.CalculateCacheKey(name);

                Logger.LogDebug($"TenantStore.GetCacheItemByNameAsync: {cacheKey}");

                var cacheItem = await _cache.GetAsync(cacheKey);

                if (cacheItem != null)
                {
                    Logger.LogDebug($"Found in the cache: {cacheKey}");
                    return cacheItem;
                }
                Logger.LogDebug($"Not found in the cache, getting from the repository: {cacheKey}");

                var tenant = await _tenantRepository.FindByNameAsync(name);
                if (tenant == null)
                {
                    Logger.LogWarning($"Can not found tenant by name: {name}");
                    // throw new AbpException($"Can not found tenant by name: {name}");
                    return null;
                }
                var connectionStrings = new ConnectionStrings();
                foreach (var tenantConnectionString in tenant.ConnectionStrings)
                {
                    connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
                }
                cacheItem = new TenantConfigurationCacheItem(tenant.Id, tenant.Name, connectionStrings);

                Logger.LogDebug($"Setting the cache item: {cacheKey}");

                await _cache.SetAsync(cacheKey, cacheItem);

                // 用租户标识再次缓存,以便通过标识查询也能命中缓存
                await _cache.SetAsync(TenantConfigurationCacheItem.CalculateCacheKey(tenant.Id.ToString()), cacheItem);

                Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

                return cacheItem;
            }
        }
    }
}
