using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Notifications;

[Dependency(ReplaceServices = true)]
public class DynamicNotificationDefinitionStore : IDynamicNotificationDefinitionStore, ITransientDependency
{
    protected INotificationDefinitionGroupRecordRepository NotificationGroupRepository { get; }
    protected INotificationDefinitionRecordRepository NotificationRepository { get; }
    protected INotificationDefinitionSerializer NotificationDefinitionSerializer { get; }
    protected IDynamicNotificationDefinitionStoreCache StoreCache { get; }
    protected IDistributedCache DistributedCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpNotificationsManagementOptions NotificationManagementOptions { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }

    public DynamicNotificationDefinitionStore(
        INotificationDefinitionGroupRecordRepository notificationGroupRepository,
        INotificationDefinitionRecordRepository notificationRepository,
        INotificationDefinitionSerializer notificationDefinitionSerializer,
        IDynamicNotificationDefinitionStoreCache storeCache,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<AbpNotificationsManagementOptions> notificationManagementOptions,
        IAbpDistributedLock distributedLock)
    {
        NotificationGroupRepository = notificationGroupRepository;
        NotificationRepository = notificationRepository;
        NotificationDefinitionSerializer = notificationDefinitionSerializer;
        StoreCache = storeCache;
        DistributedCache = distributedCache;
        DistributedLock = distributedLock;
        NotificationManagementOptions = notificationManagementOptions.Value;
        CacheOptions = cacheOptions.Value;
    }

    public async virtual Task<NotificationDefinition> GetOrNullAsync(string name)
    {
        if (!NotificationManagementOptions.IsDynamicNotificationsStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetNotificationOrNull(name);
        }
    }

    public async virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync()
    {
        if (!NotificationManagementOptions.IsDynamicNotificationsStoreEnabled)
        {
            return Array.Empty<NotificationDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetNotifications().ToImmutableList();
        }
    }

    public async virtual Task<NotificationGroupDefinition> GetGroupOrNullAsync(string name)
    {
        if (!NotificationManagementOptions.IsDynamicNotificationsStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetNotificationGroupOrNull(name);
        }
    }

    public async virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync()
    {
        if (!NotificationManagementOptions.IsDynamicNotificationsStoreEnabled)
        {
            return Array.Empty<NotificationGroupDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetGroups().ToImmutableList();
        }
    }

    protected async virtual Task EnsureCacheIsUptoDateAsync()
    {
        if (StoreCache.LastCheckTime.HasValue &&
            DateTime.Now.Subtract(StoreCache.LastCheckTime.Value) < NotificationManagementOptions.NotificationsCacheRefreshInterval)
        {
            /* We get the latest Notification with a small delay for optimization */
            return;
        }

        var stampInDistributedCache = await GetOrSetStampInDistributedCache();

        if (stampInDistributedCache == StoreCache.CacheStamp)
        {
            StoreCache.LastCheckTime = DateTime.Now;
            return;
        }

        await UpdateInMemoryStoreCache();

        StoreCache.CacheStamp = stampInDistributedCache;
        StoreCache.LastCheckTime = DateTime.Now;
    }

    protected async virtual Task UpdateInMemoryStoreCache()
    {
        var NotificationGroupRecords = await NotificationGroupRepository.GetListAsync();
        var NotificationRecords = await NotificationRepository.GetListAsync();

        await StoreCache.FillAsync(NotificationGroupRecords, NotificationRecords);
    }

    protected async virtual Task<string> GetOrSetStampInDistributedCache()
    {
        var cacheKey = GetCommonStampCacheKey();

        var stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache;
        }

        await using (var commonLockHandle = await DistributedLock
            .TryAcquireAsync(GetCommonDistributedLockKey(), NotificationManagementOptions.NotificationsCacheStampTimeOut))
        {
            if (commonLockHandle == null)
            {
                /* This request will fail */
                throw new AbpException(
                    "Could not acquire distributed lock for Notification definition common stamp check!"
                );
            }

            stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
            if (stampInDistributedCache != null)
            {
                return stampInDistributedCache;
            }

            stampInDistributedCache = Guid.NewGuid().ToString();

            await DistributedCache.SetStringAsync(
                cacheKey,
                stampInDistributedCache,
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = NotificationManagementOptions.NotificationsCacheStampExpiration
                }
            );
        }

        return stampInDistributedCache;
    }

    protected virtual string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryNotificationCacheStamp";
    }

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpNotificationUpdateLock";
    }
}
