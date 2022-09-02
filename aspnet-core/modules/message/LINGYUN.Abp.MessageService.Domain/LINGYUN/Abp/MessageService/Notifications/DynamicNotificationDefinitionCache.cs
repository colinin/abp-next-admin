using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
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

namespace LINGYUN.Abp.MessageService.Notifications;

public class DynamicNotificationDefinitionCache : IDynamicNotificationDefinitionCache, ISingletonDependency
{
    private readonly static ConcurrentDictionary<string, NotificationDefinitionGroupsCacheItem> _dynamicNotificationGroupL1Cache;
    private readonly static ConcurrentDictionary<string, NotificationDefinitionsCacheItem> _dynamicNotificationsL1Cache;

    private readonly static SemaphoreSlim _l2CacheSyncLock;
    protected IMemoryCache DynamicNotificationL2Cache { get; }

    protected IDistributedCache<NotificationDefinitionGroupsCacheItem> DynamicNotificationGroupL3Cache { get; }
    protected IDistributedCache<NotificationDefinitionsCacheItem> DynamicNotificationsL3Cache { get; }


    protected INotificationDefinitionGroupRecordRepository NotificationDefinitionGroupRecordRepository { get; }
    protected INotificationDefinitionRecordRepository NotificationDefinitionRecordRepository { get; }

    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    static DynamicNotificationDefinitionCache()
    {
        _l2CacheSyncLock = new SemaphoreSlim(1, 1);
        _dynamicNotificationsL1Cache = new ConcurrentDictionary<string, NotificationDefinitionsCacheItem>();
        _dynamicNotificationGroupL1Cache = new ConcurrentDictionary<string, NotificationDefinitionGroupsCacheItem>();
    }

    public DynamicNotificationDefinitionCache(
        IMemoryCache dynamicNotificationL2Cache, 
        IDistributedCache<NotificationDefinitionGroupsCacheItem> dynamicNotificationGroupL3Cache,
        IDistributedCache<NotificationDefinitionsCacheItem> dynamicNotificationsL3Cache, 
        INotificationDefinitionGroupRecordRepository notificationDefinitionGroupRecordRepository, 
        INotificationDefinitionRecordRepository notificationDefinitionRecordRepository, 
        IOptions<AbpLocalizationOptions> localizationOptions, 
        IStringLocalizerFactory stringLocalizerFactory)
    {
        DynamicNotificationL2Cache = dynamicNotificationL2Cache;
        DynamicNotificationGroupL3Cache = dynamicNotificationGroupL3Cache;
        DynamicNotificationsL3Cache = dynamicNotificationsL3Cache;
        NotificationDefinitionGroupRecordRepository = notificationDefinitionGroupRecordRepository;
        NotificationDefinitionRecordRepository = notificationDefinitionRecordRepository;
        LocalizationOptions = localizationOptions.Value;
        StringLocalizerFactory = stringLocalizerFactory;
    }

    public async virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync()
    {
        var cacheKey = NotificationDefinitionGroupsCacheItem.CalculateCacheKey(CultureInfo.CurrentCulture.Name);

        if (!_dynamicNotificationGroupL1Cache.TryGetValue(cacheKey, out var cacheItem))
        {
            cacheItem = await GetGroupsFormL2CacheAsync(cacheKey);
        }

        return cacheItem.Groups
            .Select(g => NotificationGroupDefinition
                .Create(g.Name, new FixedLocalizableString(g.DisplayName), g.AllowSubscriptionToClients))
            .ToImmutableList();
    }

    public async virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync()
    {
        var cacheKey = NotificationDefinitionsCacheItem.CalculateCacheKey(CultureInfo.CurrentCulture.Name);

        if (!_dynamicNotificationsL1Cache.TryGetValue(cacheKey, out var cacheItem))
        {
            cacheItem = await GetNotificationsFormL2CacheAsync(cacheKey);
        }

        return cacheItem.Notifications
            .Select(n => new NotificationDefinition(
                n.Name, 
                new FixedLocalizableString(n.DisplayName), 
                new FixedLocalizableString(n.Description),
                n.NotificationType,
                n.Lifetime,
                n.AllowSubscriptionToClients))
            .ToImmutableList();
    }

    public async virtual Task<NotificationDefinition> GetOrNullAsync(string name)
    {
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
                var description = record.Description;

                if (!record.ResourceName.IsNullOrWhiteSpace() && !record.Localization.IsNullOrWhiteSpace())
                {
                    var resource = GetResourceOrNull(record.ResourceName);
                    if (resource != null)
                    {
                        var localizer = StringLocalizerFactory.Create(resource.ResourceType);

                        displayName = localizer[$"DisplayName:{record.Localization}"];
                        description = localizer[$"Description:{record.Localization}"];
                    }
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
                var displayName = record.Name;
                var description = record.Name;
                var providers = new List<string>();
                if (!record.Providers.IsNullOrWhiteSpace())
                {
                    providers = record.Providers.Split(';').ToList();
                }

                if (record.ResourceName.IsNullOrWhiteSpace() && record.Localization.IsNullOrWhiteSpace())
                {
                    var resource = GetResourceOrNull(record.ResourceName);
                    if (resource != null)
                    {
                        var localizer = StringLocalizerFactory.Create(resource.ResourceType);

                        displayName = localizer[$"DisplayName:{record.Localization}"];
                        description = localizer[$"Description:{record.Localization}"];
                    }
                }

                var recordCache = new NotificationDefinitionCacheItem(
                    record.Name,
                    displayName,
                    description,
                    record.NotificationLifetime,
                    record.NotificationType,
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

    protected virtual LocalizationResource GetResourceOrNull(string resourceName)
    {
        return LocalizationOptions.Resources.Values
            .FirstOrDefault(x => x.ResourceName.Equals(resourceName));
    }
}
