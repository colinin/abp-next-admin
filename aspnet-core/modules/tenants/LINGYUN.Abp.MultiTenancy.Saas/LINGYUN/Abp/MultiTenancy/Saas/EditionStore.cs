using JetBrains.Annotations;
using LINGYUN.Abp.MultiTenancy.Editions;
using LINGYUN.Abp.Saas.Tenants;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MultiTenancy.Saas;

public class EditionStore : IEditionStore, ITransientDependency
{
    protected ITenantAppService TenantAppService { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedCache<EditionCacheItem> Cache { get; }

    public EditionStore(
        ITenantAppService tenantAppService,
        ICurrentTenant currentTenant,
        IDistributedCache<EditionCacheItem> cache)
    {
        TenantAppService = tenantAppService;
        CurrentTenant = currentTenant;
        Cache = cache;
    }

    public async virtual Task<EditionInfo> FindByTenantAsync(Guid tenantId)
    {
        return (await GetCacheItemAsync(tenantId)).Value;
    }

    protected async virtual Task<EditionCacheItem> GetCacheItemAsync(Guid tenantId)
    {
        var cacheKey = CalculateCacheKey(tenantId);

        var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        using (CurrentTenant.Change(null))
        {
            var tenant = await TenantAppService.GetAsync(tenantId);
            return await SetCacheAsync(cacheKey, tenant);
        }
    }

    protected async virtual Task<EditionCacheItem> SetCacheAsync(string cacheKey, [CanBeNull] TenantDto tenant)
    {
        EditionInfo editionInfo = null;
        if (tenant != null && tenant.EditionId.HasValue && !tenant.EditionName.IsNullOrWhiteSpace())
        {
            editionInfo = new EditionInfo(tenant.EditionId.Value, tenant.EditionName);
        }
        var cacheItem = new EditionCacheItem(editionInfo);
        await Cache.SetAsync(cacheKey, cacheItem, considerUow: true);
        return cacheItem;
    }

    protected virtual string CalculateCacheKey(Guid tenantId)
    {
        return EditionCacheItem.CalculateCacheKey(tenantId);
    }
}
