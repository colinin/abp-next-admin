using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Notifications;

public class StaticNotificationSaver : IStaticNotificationSaver, ITransientDependency
{
    protected IStaticNotificationDefinitionStore StaticStore { get; }
    protected INotificationDefinitionGroupRecordRepository NotificationGroupRepository { get; }
    protected INotificationDefinitionRecordRepository NotificationRepository { get; }
    protected INotificationDefinitionSerializer NotificationSerializer { get; }
    protected IDistributedCache Cache { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpNotificationsOptions NotificationsOptions { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }

    public StaticNotificationSaver(
        IStaticNotificationDefinitionStore staticStore,
        INotificationDefinitionGroupRecordRepository notificationGroupRepository,
        INotificationDefinitionRecordRepository notificationRepository,
        INotificationDefinitionSerializer notificationSerializer,
        IDistributedCache cache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IApplicationInfoAccessor applicationInfoAccessor,
        IAbpDistributedLock distributedLock,
        IOptions<AbpNotificationsOptions> notificationsOptions,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        StaticStore = staticStore;
        NotificationGroupRepository = notificationGroupRepository;
        NotificationRepository = notificationRepository;
        NotificationSerializer = notificationSerializer;
        Cache = cache;
        ApplicationInfoAccessor = applicationInfoAccessor;
        DistributedLock = distributedLock;
        CancellationTokenProvider = cancellationTokenProvider;
        NotificationsOptions = notificationsOptions.Value;
        CacheOptions = cacheOptions.Value;
    }

    [UnitOfWork]
    public async virtual Task SaveAsync()
    {
        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            GetApplicationDistributedLockKey()
        );

        if (applicationLockHandle == null)
        {
            /* Another application instance is already doing it */
            return;
        }

        /* NOTE: This can be further optimized by using 4 cache values for:
         * Groups, notifications, deleted groups and deleted notifications.
         * But the code would be more complex. This is enough for now.
         */

        var cacheKey = GetApplicationHashCacheKey();
        var cachedHash = await Cache.GetStringAsync(cacheKey, CancellationTokenProvider.Token);

        var (notificationGroupRecords, notificationRecords) = await NotificationSerializer.SerializeAsync(
            await StaticStore.GetGroupsAsync()
        );

        var currentHash = CalculateHash(
            notificationGroupRecords,
            notificationRecords,
            NotificationsOptions.DeletedNotificationGroups,
            NotificationsOptions.DeletedNotifications
        );

        if (cachedHash == currentHash)
        {
            return;
        }

        await using (var commonLockHandle = await DistributedLock.TryAcquireAsync(
                         GetCommonDistributedLockKey(),
                         TimeSpan.FromMinutes(5)))
        {
            if (commonLockHandle == null)
            {
                /* It will re-try */
                throw new AbpException("Could not acquire distributed lock for saving static notifications!");
            }

            var hasChangesInGroups = await UpdateChangednotificationGroupsAsync(notificationGroupRecords);
            var hasChangesInnotifications = await UpdateChangednotificationsAsync(notificationRecords);

            if (hasChangesInGroups || hasChangesInnotifications)
            {
                await Cache.SetStringAsync(
                    GetCommonStampCacheKey(),
                    Guid.NewGuid().ToString(),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
                    },
                    CancellationTokenProvider.Token
                );
            }
        }

        await Cache.SetStringAsync(
            cacheKey,
            currentHash,
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
            },
            CancellationTokenProvider.Token
        );
    }

    private async Task<bool> UpdateChangednotificationGroupsAsync(
        IEnumerable<NotificationDefinitionGroupRecord> notificationGroupRecords)
    {
        var newRecords = new List<NotificationDefinitionGroupRecord>();
        var changedRecords = new List<NotificationDefinitionGroupRecord>();

        var notificationGroupRecordsInDatabase = (await NotificationGroupRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var notificationGroupRecord in notificationGroupRecords)
        {
            var notificationGroupRecordInDatabase = notificationGroupRecordsInDatabase.GetOrDefault(notificationGroupRecord.Name);
            if (notificationGroupRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(notificationGroupRecord);
                continue;
            }

            if (notificationGroupRecord.HasSameData(notificationGroupRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            notificationGroupRecordInDatabase.Patch(notificationGroupRecord);
            changedRecords.Add(notificationGroupRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = NotificationsOptions.DeletedNotificationGroups.Any()
            ? notificationGroupRecordsInDatabase.Values
                .Where(x => NotificationsOptions.DeletedNotificationGroups.Contains(x.Name))
                .ToArray()
            : Array.Empty<NotificationDefinitionGroupRecord>();

        if (newRecords.Any())
        {
            await NotificationGroupRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await NotificationGroupRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await NotificationGroupRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private async Task<bool> UpdateChangednotificationsAsync(
        IEnumerable<NotificationDefinitionRecord> notificationRecords)
    {
        var newRecords = new List<NotificationDefinitionRecord>();
        var changedRecords = new List<NotificationDefinitionRecord>();

        var notificationRecordsInDatabase = (await NotificationRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var notificationRecord in notificationRecords)
        {
            var notificationRecordInDatabase = notificationRecordsInDatabase.GetOrDefault(notificationRecord.Name);
            if (notificationRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(notificationRecord);
                continue;
            }

            if (notificationRecord.HasSameData(notificationRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            notificationRecordInDatabase.Patch(notificationRecord);
            changedRecords.Add(notificationRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = new List<NotificationDefinitionRecord>();

        if (NotificationsOptions.DeletedNotifications.Any())
        {
            deletedRecords.AddRange(
                notificationRecordsInDatabase.Values
                    .Where(x => NotificationsOptions.DeletedNotifications.Contains(x.Name))
            );
        }

        if (NotificationsOptions.DeletedNotificationGroups.Any())
        {
            deletedRecords.AddIfNotContains(
                notificationRecordsInDatabase.Values
                    .Where(x => NotificationsOptions.DeletedNotificationGroups.Contains(x.GroupName))
            );
        }

        if (newRecords.Any())
        {
            await NotificationRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await NotificationRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await NotificationRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpNotificationUpdateLock";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpNotificationUpdateLock";
    }

    private string GetApplicationHashCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpNotificationsHash";
    }

    private string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryNotificationCacheStamp";
    }

    private static string CalculateHash(
        NotificationDefinitionGroupRecord[] notificationGroupRecords,
        NotificationDefinitionRecord[] notificationRecords,
        IEnumerable<string> deletednotificationGroups,
        IEnumerable<string> deletednotifications)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("NotificationGroupRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(notificationGroupRecords));

        stringBuilder.Append("NotificationRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(notificationRecords));

        stringBuilder.Append("DeletedNotificationGroups:");
        stringBuilder.AppendLine(deletednotificationGroups.JoinAsString(","));

        stringBuilder.Append("DeletedNotification:");
        stringBuilder.Append(deletednotifications.JoinAsString(","));

        return stringBuilder
            .ToString()
            .ToMd5();
    }
}
