using LINGYUN.Abp.Saas.Tenants;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.MultiTenancy.Saas;

public class TenantCacheItemInvalidator :
    IDistributedEventHandler<EntityUpdatedEto<TenantEto>>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
    IDistributedEventHandler<ConnectionStringCreatedEventData>,
    IDistributedEventHandler<ConnectionStringDeletedEventData>,
    ITransientDependency
{
    protected IDistributedCache<TenantCacheItem> Cache { get; }

    public TenantCacheItemInvalidator(IDistributedCache<TenantCacheItem> cache)
    {
        Cache = cache;
    }

    public virtual async Task HandleEventAsync(EntityUpdatedEto<TenantEto> eventData)
    {
        await Cache.RemoveAsync(
            TenantCacheItem.CalculateCacheKey(
                eventData.Entity.Id,
                eventData.Entity.Name),
            considerUow: true);

        await Cache.RemoveAsync(
            EditionCacheItem.CalculateCacheKey(
                eventData.Entity.Id),
            considerUow: true);
    }

    public virtual async Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
    {
        await Cache.RemoveAsync(
            TenantCacheItem.CalculateCacheKey(
                eventData.Entity.Id,
                eventData.Entity.Name),
            considerUow: true);

        await Cache.RemoveAsync(
            EditionCacheItem.CalculateCacheKey(
                eventData.Entity.Id),
            considerUow: true);
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
