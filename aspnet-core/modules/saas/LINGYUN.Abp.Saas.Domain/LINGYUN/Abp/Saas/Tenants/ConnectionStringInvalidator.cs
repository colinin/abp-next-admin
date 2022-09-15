using LINGYUN.Abp.MultiTenancy;
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

    public async virtual Task HandleEventAsync(ConnectionStringCreatedEventData eventData)
    {
        // 需要考虑三种情形下的缓存键
        await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(null, eventData.TenantName), considerUow: true);
        await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(eventData.TenantId, null), considerUow: true);
        await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(eventData.TenantId, eventData.TenantName), considerUow: true);
    }

    public async virtual Task HandleEventAsync(ConnectionStringDeletedEventData eventData)
    {
        // 需要考虑三种情形下的缓存键
        await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(null, eventData.TenantName), considerUow: true);
        await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(eventData.TenantId, null), considerUow: true);
        await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(eventData.TenantId, eventData.TenantName), considerUow: true);
    }
}
