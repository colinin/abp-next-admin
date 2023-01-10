using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Notifications;

public class DynamicNotificationDefinitionCache : IDynamicNotificationDefinitionCache, ISingletonDependency
{
    private readonly static ConcurrentDictionary<string, NotificationGroupDefinition> _dynamicNotificationGroupL1Cache;
    private readonly static ConcurrentDictionary<string, NotificationDefinition> _dynamicNotificationsL1Cache;

    private readonly static SemaphoreSlim _l2CacheSyncLock;
    protected IMemoryCache DynamicNotificationL2Cache { get; }

    protected IDistributedCache<NotificationDefinitionGroupsCacheItem> DynamicNotificationGroupL3Cache { get; }
    protected IDistributedCache<NotificationDefinitionsCacheItem> DynamicNotificationsL3Cache { get; }


    protected INotificationDefinitionGroupRecordRepository NotificationDefinitionGroupRecordRepository { get; }
    protected INotificationDefinitionRecordRepository NotificationDefinitionRecordRepository { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    static DynamicNotificationDefinitionCache()
    {
        _l2CacheSyncLock = new SemaphoreSlim(1, 1);
        _dynamicNotificationsL1Cache = new ConcurrentDictionary<string, NotificationDefinition>();
        _dynamicNotificationGroupL1Cache = new ConcurrentDictionary<string, NotificationGroupDefinition>();
    }

    public DynamicNotificationDefinitionCache(
        IMemoryCache dynamicNotificationL2Cache, 
        IDistributedCache<NotificationDefinitionGroupsCacheItem> dynamicNotificationGroupL3Cache,
        IDistributedCache<NotificationDefinitionsCacheItem> dynamicNotificationsL3Cache, 
        INotificationDefinitionGroupRecordRepository notificationDefinitionGroupRecordRepository, 
        INotificationDefinitionRecordRepository notificationDefinitionRecordRepository,
        ILocalizableStringSerializer localizableStringSerializer,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        DynamicNotificationL2Cache = dynamicNotificationL2Cache;
        DynamicNotificationGroupL3Cache = dynamicNotificationGroupL3Cache;
        DynamicNotificationsL3Cache = dynamicNotificationsL3Cache;
        NotificationDefinitionGroupRecordRepository = notificationDefinitionGroupRecordRepository;
        NotificationDefinitionRecordRepository = notificationDefinitionRecordRepository;
        LocalizableStringSerializer = localizableStringSerializer;
        StringLocalizerFactory = stringLocalizerFactory;
    }

    public async virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync()
    {
        var cacheKey = NotificationDefinitionGroupsCacheItem.CalculateCacheKey(CultureInfo.CurrentCulture.Name);

        if (!_dynamicNotificationGroupL1Cache.Any())
        {
            var l2GroupCache = await GetGroupsFormL2CacheAsync(cacheKey);
            var notifications = await GetNotificationsFormL2CacheAsync(
                NotificationDefinitionsCacheItem.CalculateCacheKey(CultureInfo.CurrentCulture.Name));

            foreach (var group in l2GroupCache.Groups)
            {
                var groupGroup = NotificationGroupDefinition
                    .Create(
                        group.Name,
                        LocalizableStringSerializer.Deserialize(group.DisplayName),
                        group.AllowSubscriptionToClients);

                var notificationsInThisGroup = notifications.Notifications
                    .Where(p => p.GroupName == groupGroup.Name);

                foreach (var notification in notificationsInThisGroup)
                {
                    var notificationDefine = groupGroup.AddNotification(
                        notification.Name,
                        LocalizableStringSerializer.Deserialize(notification.DisplayName),
                        LocalizableStringSerializer.Deserialize(notification.Description),
                        notification.NotificationType,
                        notification.Lifetime,
                        notification.ContentType,
                        notification.AllowSubscriptionToClients);

                    notificationDefine.Properties.AddIfNotContains(notification.Properties);
                }

                _dynamicNotificationGroupL1Cache.TryAdd(group.Name, groupGroup);
            }

            //NotificationGroupDefinition
            //    .Create(g.Name, new FixedLocalizableString(g.DisplayName), g.AllowSubscriptionToClients)
        }

        return _dynamicNotificationGroupL1Cache.Values.ToImmutableList();
    }

    public async virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync()
    {
        var cacheKey = NotificationDefinitionsCacheItem.CalculateCacheKey(CultureInfo.CurrentCulture.Name);

        if (!_dynamicNotificationsL1Cache.Any())
        {
            var l2cache = await GetNotificationsFormL2CacheAsync(cacheKey);

            foreach (var notification in l2cache.Notifications)
            {
                var notificationDefinition = new NotificationDefinition(
                    notification.Name,
                    LocalizableStringSerializer.Deserialize(notification.DisplayName),
                    LocalizableStringSerializer.Deserialize(notification.Description),
                    notification.NotificationType,
                    notification.Lifetime,
                    notification.ContentType,
                    notification.AllowSubscriptionToClients);

                notificationDefinition.WithProviders(notification.Providers.ToArray());
                notificationDefinition.Properties.AddIfNotContains(notification.Properties);

                _dynamicNotificationsL1Cache.TryAdd(notification.Name, notificationDefinition);
            }
        }

        return _dynamicNotificationsL1Cache.Values.ToImmutableList();
    }

    public async virtual Task<NotificationDefinition> GetOrNullAsync(string name)
    {
        if (_dynamicNotificationsL1Cache.Any())
        {
            return _dynamicNotificationsL1Cache.GetOrDefault(name);
        }
        var notifications = await GetNotificationsAsync();

        return notifications.FirstOrDefault(n => n.Name.Equals(name));
    }

    protected async virtual Task<NotificationDefinitionGroupsCacheItem> GetGroupsFormL2CacheAsync(string cacheKey)
    {
        var cacheItem = DynamicNotificationL2Cache.Get<NotificationDefinitionGroupsCacheItem>(cacheKey);

        if (cacheItem == null)
        {
            using (await _l2CacheSyncLock.LockAsync())
            {
                var l2cache = await GetGroupsFormL3CacheAsync(cacheKey);

                var options = new MemoryCacheEntryOptions();
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                options.SetPriority(CacheItemPriority.High);
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _dynamicNotificationGroupL1Cache.Clear();
                });

                DynamicNotificationL2Cache.Set(cacheKey, l2cache, options);

                return l2cache;
            }
        }

        return cacheItem;
    }

    protected async virtual Task<NotificationDefinitionGroupsCacheItem> GetGroupsFormL3CacheAsync(string cacheKey)
    {
        var cacheItem = await DynamicNotificationGroupL3Cache.GetAsync(cacheKey);

        if (cacheItem == null)
        {
            var records = await GetGroupsFormRepositoryAsync();
            var recordCaches = new List<NotificationDefinitionGroupCacheItem>();

            foreach (var record in records)
            {
                var displayName = record.DisplayName;
                if (!displayName.IsNullOrWhiteSpace())
                {
                    displayName = await LocalizableStringSerializer
                        .Deserialize(displayName)
                        .LocalizeAsync(StringLocalizerFactory);
                }

                var description = record.Description;
                if (!description.IsNullOrWhiteSpace())
                {
                    description = await LocalizableStringSerializer
                        .Deserialize(description)
                        .LocalizeAsync(StringLocalizerFactory);
                }

                recordCaches.Add(new NotificationDefinitionGroupCacheItem(
                    record.Name,
                    displayName,
                    description,
                    record.AllowSubscriptionToClients));
            }

            cacheItem = new NotificationDefinitionGroupsCacheItem(recordCaches.ToArray());

            await DynamicNotificationGroupL3Cache.SetAsync(
                cacheKey,
                cacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
                    SlidingExpiration = TimeSpan.FromHours(1),
                });
        }

        return cacheItem;
    }

    protected async virtual Task<List<NotificationDefinitionGroupRecord>> GetGroupsFormRepositoryAsync()
    {
        var records = await NotificationDefinitionGroupRecordRepository.GetListAsync();

        return records;
    }


    protected async virtual Task<NotificationDefinitionsCacheItem> GetNotificationsFormL2CacheAsync(string cacheKey)
    {
        var cacheItem = DynamicNotificationL2Cache.Get<NotificationDefinitionsCacheItem>(cacheKey);

        if (cacheItem == null)
        {
            using (await _l2CacheSyncLock.LockAsync())
            {
                var l2cache = await GetNotificationsFormL3CacheAsync(cacheKey);

                var options = new MemoryCacheEntryOptions();
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                options.SetPriority(CacheItemPriority.High);
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _dynamicNotificationsL1Cache.Clear();
                });

                DynamicNotificationL2Cache.Set(cacheKey, l2cache, options);

                return l2cache;
            }
        }

        return cacheItem;
    }

    protected async virtual Task<NotificationDefinitionsCacheItem> GetNotificationsFormL3CacheAsync(string cacheKey)
    {
        var cacheItem = await DynamicNotificationsL3Cache.GetAsync(cacheKey);

        if (cacheItem == null)
        {
            var records = await GetNotificationsFormRepositoryAsync();
            var recordCaches = new List<NotificationDefinitionCacheItem>();

            foreach (var record in records)
            {
                var displayName = record.DisplayName;
                if (!displayName.IsNullOrWhiteSpace())
                {
                    displayName = await LocalizableStringSerializer
                        .Deserialize(displayName)
                        .LocalizeAsync(StringLocalizerFactory);
                }

                var description = record.Description;
                if (!description.IsNullOrWhiteSpace())
                {
                    description = await LocalizableStringSerializer
                        .Deserialize(description)
                        .LocalizeAsync(StringLocalizerFactory);
                }

                var providers = new List<string>();
                if (!record.Providers.IsNullOrWhiteSpace())
                {
                    providers = record.Providers.Split(';').ToList();
                }

                var recordCache = new NotificationDefinitionCacheItem(
                    record.Name,
                    record.GroupName,
                    displayName,
                    description,
                    record.NotificationLifetime,
                    record.NotificationType,
                    record.ContentType,
                    providers,
                    record.AllowSubscriptionToClients);
                recordCache.Properties.AddIfNotContains(record.ExtraProperties);

                recordCaches.Add(recordCache);
            }

            cacheItem = new NotificationDefinitionsCacheItem(recordCaches.ToArray());

            await DynamicNotificationsL3Cache.SetAsync(
                cacheKey,
                cacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
                    SlidingExpiration = TimeSpan.FromHours(1),
                });
        }

        return cacheItem;
    }

    protected async virtual Task<List<NotificationDefinitionRecord>> GetNotificationsFormRepositoryAsync()
    {
        var records = await NotificationDefinitionRecordRepository.GetListAsync();

        return records;
    }
}
