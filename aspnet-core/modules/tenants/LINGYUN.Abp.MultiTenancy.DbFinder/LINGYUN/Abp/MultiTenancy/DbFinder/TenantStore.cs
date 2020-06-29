using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.MultiTenancy.DbFinder
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(ITenantStore))]
    public class TenantStore : ITenantStore
    {
        public ILogger<TenantStore> Logger { protected get; set; }
        private readonly IDistributedCache<TenantConfigurationCacheItem> _cache;

        private readonly ITenantRepository _tenantRepository;

        public TenantStore(
            ITenantRepository tenantRepository,
            IDistributedCache<TenantConfigurationCacheItem> cache)
        {
            _cache = cache;
            _tenantRepository = tenantRepository;

            Logger = NullLogger<TenantStore>.Instance;
        }

        public virtual TenantConfiguration Find(string name)
        {
            var tenantCacheItem = AsyncHelper.RunSync(async () => await 
                GetCacheItemByNameAsync(name));

            return new TenantConfiguration(tenantCacheItem.Id, tenantCacheItem.Name)
            {
                ConnectionStrings = tenantCacheItem.ConnectionStrings
            };
        }

        public virtual TenantConfiguration Find(Guid id)
        {
            var tenantCacheItem = AsyncHelper.RunSync(async () => await
                GetCacheItemByIdAsync(id));

            return new TenantConfiguration(tenantCacheItem.Id, tenantCacheItem.Name)
            {
                ConnectionStrings = tenantCacheItem.ConnectionStrings
            };
        }

        public virtual async Task<TenantConfiguration> FindAsync(string name)
        {
            var tenantCacheItem = await GetCacheItemByNameAsync(name);

            return new TenantConfiguration(tenantCacheItem.Id, tenantCacheItem.Name)
            {
                ConnectionStrings = tenantCacheItem.ConnectionStrings
            };
        }

        public virtual async Task<TenantConfiguration> FindAsync(Guid id)
        {
            var tenantCacheItem = await GetCacheItemByIdAsync(id);

            return new TenantConfiguration(tenantCacheItem.Id, tenantCacheItem.Name)
            {
                ConnectionStrings = tenantCacheItem.ConnectionStrings
            };
        }

        protected virtual async Task<TenantConfigurationCacheItem> GetCacheItemByIdAsync(Guid id)
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
                throw new AbpException($"Can not found tenant by id: {id}");
            }
            var connectionStrings = new ConnectionStrings();
            foreach (var tenantConnectionString in tenant.ConnectionStrings)
            {
                connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
            }
            cacheItem = new TenantConfigurationCacheItem(tenant.Id, tenant.Name, connectionStrings);

            Logger.LogDebug($"Setting the cache item: {cacheKey}");
            await _cache.SetAsync(cacheKey, cacheItem);
            Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

            return cacheItem;
        }
        protected virtual async Task<TenantConfigurationCacheItem> GetCacheItemByNameAsync(string name)
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
                throw new AbpException($"Can not found tenant by name: {name}");
            }
            var connectionStrings = new ConnectionStrings();
            foreach (var tenantConnectionString in tenant.ConnectionStrings)
            {
                connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
            }
            cacheItem = new TenantConfigurationCacheItem(tenant.Id, tenant.Name, connectionStrings);

            Logger.LogDebug($"Setting the cache item: {cacheKey}");
            await _cache.SetAsync(cacheKey, cacheItem);
            Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

            return cacheItem;
        }
    }
}
