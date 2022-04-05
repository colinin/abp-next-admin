using LINGYUN.Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.Saas.Tenants;
public class ConnectionStringInvalidator :
    IDistributedEventHandler<ConnectionStringCreatedEventData>,
    IDistributedEventHandler<ConnectionStringDeletedEventData>,
    ITransientDependency
{
    protected IDistributedCache<TenantCacheItem> Cache { get; }

    public ConnectionStringInvalidator(IDistributedCache<TenantCacheItem> cache)
    {
        Cache = cache;
    }

    public virtual async Task HandleEventAsync(ConnectionStringCreatedEventData eventData)
    {
        await Cache.RemoveAsync(
                    TenantCacheItem.CalculateCacheKey(
                        eventData.TenantId,
                        eventData.TenantName),
                    considerUow: true);
    }

    public virtual async Task HandleEventAsync(ConnectionStringDeletedEventData eventData)
    {
        await Cache.RemoveAsync(
            TenantCacheItem.CalculateCacheKey(
                eventData.TenantId,
                eventData.TenantName),
            considerUow: true);
    }
}
