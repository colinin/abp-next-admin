using LINGYUN.Abp.Saas.Tenants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LY.MicroService.PlatformManagement.MultiTenancy;

public class TenantConfigurationCache : ITenantConfigurationCache, ITransientDependency
{
    protected ITenantRepository TenantRepository { get; }
    protected IDistributedCache<TenantConfigurationCacheItem> TenantCache { get; }

    public TenantConfigurationCache(
        ITenantRepository tenantRepository, 
        IDistributedCache<TenantConfigurationCacheItem> tenantCache)
    {
        TenantRepository = tenantRepository;
        TenantCache = tenantCache;
    }

    public async virtual Task<List<TenantConfiguration>> GetTenantsAsync()
    {
        return (await GetForCacheItemAsync()).Tenants;
    }

    protected async virtual Task<TenantConfigurationCacheItem> GetForCacheItemAsync()
    {
        var cacheKey = "_Abp_Tenant_Configuration";
        var cacheItem = await TenantCache.GetAsync(cacheKey);
        if (cacheItem == null)
        {
            var allActiveTenants = await TenantRepository.GetListAsync();

            cacheItem = new TenantConfigurationCacheItem(
                allActiveTenants
                .Where(t => t.IsActive)
                .Select(t => new TenantConfiguration(t.Id, t.Name)
                {
                    IsActive = t.IsActive,
                }).ToList());

            await TenantCache.SetAsync(cacheKey, cacheItem);
        }

        return cacheItem;
    }
}
