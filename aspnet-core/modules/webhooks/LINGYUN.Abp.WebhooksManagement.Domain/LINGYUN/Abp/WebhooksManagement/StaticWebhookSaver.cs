using LINGYUN.Abp.Webhooks;
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

namespace LINGYUN.Abp.WebhooksManagement;
public class StaticWebhookSaver : IStaticWebhookSaver, ITransientDependency
{
    protected IStaticWebhookDefinitionStore StaticStore { get; }
    protected IWebhookGroupDefinitionRecordRepository WebhookGroupRepository { get; }
    protected IWebhookDefinitionRecordRepository WebhookRepository { get; }
    protected IWebhookDefinitionSerializer WebhookSerializer { get; }
    protected IDistributedCache Cache { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpWebhooksOptions WebhookOptions { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }

    public StaticWebhookSaver(
        IStaticWebhookDefinitionStore staticStore,
        IWebhookGroupDefinitionRecordRepository webhookGroupRepository,
        IWebhookDefinitionRecordRepository webhookRepository,
        IWebhookDefinitionSerializer webhookSerializer,
        IDistributedCache cache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IApplicationInfoAccessor applicationInfoAccessor,
        IAbpDistributedLock distributedLock,
        IOptions<AbpWebhooksOptions> webhookOptions,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        StaticStore = staticStore;
        WebhookGroupRepository = webhookGroupRepository;
        WebhookRepository = webhookRepository;
        WebhookSerializer = webhookSerializer;
        Cache = cache;
        ApplicationInfoAccessor = applicationInfoAccessor;
        DistributedLock = distributedLock;
        CancellationTokenProvider = cancellationTokenProvider;
        WebhookOptions = webhookOptions.Value;
        CacheOptions = cacheOptions.Value;
    }

    [UnitOfWork]
    public virtual async Task SaveAsync()
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
         * Groups, webhooks, deleted groups and deleted webhooks.
         * But the code would be more complex. This is enough for now.
         */

        var cacheKey = GetApplicationHashCacheKey();
        var cachedHash = await Cache.GetStringAsync(cacheKey, CancellationTokenProvider.Token);

        var (webhookGroupRecords, webhookRecords) = await WebhookSerializer.SerializeAsync(
            await StaticStore.GetGroupsAsync()
        );

        var currentHash = CalculateHash(
            webhookGroupRecords,
            webhookRecords,
            WebhookOptions.DeletedWebhookGroups,
            WebhookOptions.DeletedWebhooks
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
                throw new AbpException("Could not acquire distributed lock for saving static webhooks!");
            }

            var hasChangesInGroups = await UpdateChangedWebhookGroupsAsync(webhookGroupRecords);
            var hasChangesInWebhooks = await UpdateChangedWebhooksAsync(webhookRecords);

            if (hasChangesInGroups || hasChangesInWebhooks)
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

    private async Task<bool> UpdateChangedWebhookGroupsAsync(
        IEnumerable<WebhookGroupDefinitionRecord> webhookGroupRecords)
    {
        var newRecords = new List<WebhookGroupDefinitionRecord>();
        var changedRecords = new List<WebhookGroupDefinitionRecord>();

        var webhookGroupRecordsInDatabase = (await WebhookGroupRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var webhookGroupRecord in webhookGroupRecords)
        {
            var webhookGroupRecordInDatabase = webhookGroupRecordsInDatabase.GetOrDefault(webhookGroupRecord.Name);
            if (webhookGroupRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(webhookGroupRecord);
                continue;
            }

            if (webhookGroupRecord.HasSameData(webhookGroupRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            webhookGroupRecordInDatabase.Patch(webhookGroupRecord);
            changedRecords.Add(webhookGroupRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = WebhookOptions.DeletedWebhookGroups.Any()
            ? webhookGroupRecordsInDatabase.Values
                .Where(x => WebhookOptions.DeletedWebhookGroups.Contains(x.Name))
                .ToArray()
            : Array.Empty<WebhookGroupDefinitionRecord>();

        if (newRecords.Any())
        {
            await WebhookGroupRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await WebhookGroupRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await WebhookGroupRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private async Task<bool> UpdateChangedWebhooksAsync(
        IEnumerable<WebhookDefinitionRecord> webhookRecords)
    {
        var newRecords = new List<WebhookDefinitionRecord>();
        var changedRecords = new List<WebhookDefinitionRecord>();

        var webhookRecordsInDatabase = (await WebhookRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var webhookRecord in webhookRecords)
        {
            var webhookRecordInDatabase = webhookRecordsInDatabase.GetOrDefault(webhookRecord.Name);
            if (webhookRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(webhookRecord);
                continue;
            }

            if (webhookRecord.HasSameData(webhookRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            webhookRecordInDatabase.Patch(webhookRecord);
            changedRecords.Add(webhookRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = new List<WebhookDefinitionRecord>();

        if (WebhookOptions.DeletedWebhooks.Any())
        {
            deletedRecords.AddRange(
                webhookRecordsInDatabase.Values
                    .Where(x => WebhookOptions.DeletedWebhooks.Contains(x.Name))
            );
        }

        if (WebhookOptions.DeletedWebhookGroups.Any())
        {
            deletedRecords.AddIfNotContains(
                webhookRecordsInDatabase.Values
                    .Where(x => WebhookOptions.DeletedWebhookGroups.Contains(x.GroupName))
            );
        }

        if (newRecords.Any())
        {
            await WebhookRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await WebhookRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await WebhookRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpWebhookUpdateLock";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpWebhookUpdateLock";
    }

    private string GetApplicationHashCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpWebhooksHash";
    }

    private string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryWebhookCacheStamp";
    }

    private static string CalculateHash(
        WebhookGroupDefinitionRecord[] webhookGroupRecords,
        WebhookDefinitionRecord[] webhookRecords,
        IEnumerable<string> deletedWebhookGroups,
        IEnumerable<string> deletedWebhooks)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("WebhookGroupRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(webhookGroupRecords));

        stringBuilder.Append("WebhookRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(webhookRecords));

        stringBuilder.Append("DeletedWebhookGroups:");
        stringBuilder.AppendLine(deletedWebhookGroups.JoinAsString(","));

        stringBuilder.Append("DeletedWebhook:");
        stringBuilder.Append(deletedWebhooks.JoinAsString(","));

        return stringBuilder
            .ToString()
            .ToMd5();
    }
}
