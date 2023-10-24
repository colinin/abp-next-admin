using LINGYUN.Abp.Saas.Tenants;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MultiTenancy.Saas;

public class TenantCacheItemInvalidator :
    IDistributedEventHandler<EntityUpdatedEto<TenantEto>>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
    IDistributedEventHandler<TenantConnectionStringUpdatedEto>,
    ITransientDependency
{
    protected IDistributedCache<TenantCacheItem> Cache { get; }

    public TenantCacheItemInvalidator(IDistributedCache<TenantCacheItem> cache)
    {
        Cache = cache;
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<TenantEto> eventData)
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

    public async virtual Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
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

    public async virtual Task HandleEventAsync(TenantConnectionStringUpdatedEto eventData)
    {
        await Cache.RemoveAsync(
            TenantCacheItem.CalculateCacheKey(
                eventData.Id,
                eventData.Name),
            considerUow: true);
    }
}
