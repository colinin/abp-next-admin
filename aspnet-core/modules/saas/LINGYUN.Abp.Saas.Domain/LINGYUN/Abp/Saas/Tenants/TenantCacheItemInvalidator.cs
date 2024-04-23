using LINGYUN.Abp.Saas.Editions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantCacheItemInvalidator :
    IDistributedEventHandler<EntityCreatedEto<TenantEto>>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
    IDistributedEventHandler<EntityUpdatedEto<TenantEto>>,
    ILocalEventHandler<EntityChangedEventData<Tenant>>,
    ITransientDependency
{
    protected IDistributedCache<TenantCacheItem> Cache { get; }

    public TenantCacheItemInvalidator(IDistributedCache<TenantCacheItem> cache)
    {
        Cache = cache;
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<TenantEto> eto)
    {
        await RemoveTenantCache(eto.Entity.Id, eto.Entity.Name);
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<TenantEto> eto)
    {
        await RemoveTenantCache(eto.Entity.Id, eto.Entity.Name);
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<TenantEto> eto)
    {
        await RemoveTenantCache(eto.Entity.Id, eto.Entity.Name);
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<Tenant> eventData)
    {
        await RemoveTenantCache(eventData.Entity.Id, eventData.Entity.Name);
    }

    protected async virtual Task RemoveTenantCache(Guid tenantId, string tenantName = null)
    {
        var keys = new string[]
        {
            TenantCacheItem.CalculateCacheKey(null, tenantName),
            TenantCacheItem.CalculateCacheKey(tenantId, null),
            TenantCacheItem.CalculateCacheKey(tenantId, tenantName),

            // 同时移除租户版本缓存
            EditionCacheItem.CalculateCacheKey(tenantId)
        };
        await Cache.RemoveManyAsync(keys, considerUow: true);
    }
}
