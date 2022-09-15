using LINGYUN.Abp.Saas.Editions;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantCacheItemInvalidator : ILocalEventHandler<EntityChangedEventData<Tenant>>, ITransientDependency
{
    protected IDistributedCache<TenantCacheItem> Cache { get; }

    public TenantCacheItemInvalidator(IDistributedCache<TenantCacheItem> cache)
    {
        Cache = cache;
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<Tenant> eventData)
    {
        // 需要考虑三种情形下的缓存键
        await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(null, eventData.Entity.Name), considerUow: true);
        await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(eventData.Entity.Id, null), considerUow: true);
        await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(eventData.Entity.Id, eventData.Entity.Name), considerUow: true);
        // 同时移除租户版本缓存
        await Cache.RemoveAsync(EditionCacheItem.CalculateCacheKey(eventData.Entity.Id), considerUow: true);
    }
}
