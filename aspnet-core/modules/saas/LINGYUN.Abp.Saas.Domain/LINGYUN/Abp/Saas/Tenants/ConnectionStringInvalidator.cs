using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Saas.Tenants;
public class ConnectionStringInvalidator :
    IDistributedEventHandler<TenantConnectionStringUpdatedEto>,
    ITransientDependency
{
    protected IDistributedCache<TenantCacheItem> Cache { get; }

    public ConnectionStringInvalidator(IDistributedCache<TenantCacheItem> cache)
    {
        Cache = cache;
    }

    public async virtual Task HandleEventAsync(TenantConnectionStringUpdatedEto eventData)
    {
        await RemoveTenantCache(eventData.Id, eventData.Name);
    }

    protected async virtual Task RemoveTenantCache(Guid tenantId, string tenantName = null)
    {
        var keys = new string[]
        {
            TenantCacheItem.CalculateCacheKey(null, tenantName),
            TenantCacheItem.CalculateCacheKey(tenantId, null),
            TenantCacheItem.CalculateCacheKey(tenantId, tenantName),
        };
        await Cache.RemoveManyAsync(keys, considerUow: true);
    }
}
