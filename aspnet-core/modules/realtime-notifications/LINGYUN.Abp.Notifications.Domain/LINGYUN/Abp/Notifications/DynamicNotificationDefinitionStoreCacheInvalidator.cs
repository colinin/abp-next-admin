using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Notifications;
public class DynamicNotificationDefinitionStoreCacheInvalidator : 
    ILocalEventHandler<EntityChangedEventData<NotificationDefinitionGroupRecord>>,
    ILocalEventHandler<EntityChangedEventData<NotificationDefinitionRecord>>,
    ITransientDependency
{
    private readonly IDynamicNotificationDefinitionStoreCache _storeCache;

    private readonly IClock _clock;
    private readonly IDistributedCache _distributedCache;
    private readonly AbpDistributedCacheOptions _cacheOptions;
    private readonly AbpNotificationsManagementOptions _managementOptions;

    public DynamicNotificationDefinitionStoreCacheInvalidator(
        IClock clock,
        IDistributedCache distributedCache,
        IDynamicNotificationDefinitionStoreCache storeCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<AbpNotificationsManagementOptions> managementOptions)
    {
        _storeCache = storeCache;
        _clock = clock;
        _distributedCache = distributedCache;
        _cacheOptions = cacheOptions.Value;
        _managementOptions = managementOptions.Value;
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<NotificationDefinitionGroupRecord> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<NotificationDefinitionRecord> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    protected async virtual Task RemoveStampInDistributedCacheAsync()
    {
        using (await _storeCache.SyncSemaphore.LockAsync())
        {
            var cacheKey = GetCommonStampCacheKey();

            await _distributedCache.RemoveAsync(cacheKey);

            _storeCache.CacheStamp = Guid.NewGuid().ToString();
            _storeCache.LastCheckTime = _clock.Now.AddMinutes(-_managementOptions.NotificationsCacheRefreshInterval.Minutes - 5);
        }
    }

    protected virtual string GetCommonStampCacheKey()
    {
        return $"{_cacheOptions.KeyPrefix}_AbpInMemoryNotificationCacheStamp";
    }
}
