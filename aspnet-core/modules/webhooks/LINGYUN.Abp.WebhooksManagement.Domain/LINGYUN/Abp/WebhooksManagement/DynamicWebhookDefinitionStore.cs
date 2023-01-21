using LINGYUN.Abp.Webhooks;
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

namespace LINGYUN.Abp.WebhooksManagement;

[Dependency(ReplaceServices = true)]
public class DynamicWebhookDefinitionStore : IDynamicWebhookDefinitionStore, ITransientDependency
{
    protected IWebhookGroupDefinitionRecordRepository WebhookGroupRepository { get; }
    protected IWebhookDefinitionRecordRepository WebhookRepository { get; }
    protected IWebhookDefinitionSerializer WebhookDefinitionSerializer { get; }
    protected IDynamicWebhookDefinitionStoreCache StoreCache { get; }
    protected IDistributedCache DistributedCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected WebhooksManagementOptions WebhookManagementOptions { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    
    public DynamicWebhookDefinitionStore(
        IWebhookGroupDefinitionRecordRepository webhookGroupRepository,
        IWebhookDefinitionRecordRepository webhookRepository,
        IWebhookDefinitionSerializer webhookDefinitionSerializer,
        IDynamicWebhookDefinitionStoreCache storeCache,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<WebhooksManagementOptions> webhookManagementOptions,
        IAbpDistributedLock distributedLock)
    {
        WebhookGroupRepository = webhookGroupRepository;
        WebhookRepository = webhookRepository;
        WebhookDefinitionSerializer = webhookDefinitionSerializer;
        StoreCache = storeCache;
        DistributedCache = distributedCache;
        DistributedLock = distributedLock;
        WebhookManagementOptions = webhookManagementOptions.Value;
        CacheOptions = cacheOptions.Value;
    }

    public virtual async Task<WebhookDefinition> GetOrNullAsync(string name)
    {
        if (!WebhookManagementOptions.IsDynamicWebhookStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetWebhookOrNull(name);
        }
    }

    public virtual async Task<IReadOnlyList<WebhookDefinition>> GetWebhooksAsync()
    {
        if (!WebhookManagementOptions.IsDynamicWebhookStoreEnabled)
        {
            return Array.Empty<WebhookDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetWebhooks().ToImmutableList();
        }
    }

    public virtual async Task<IReadOnlyList<WebhookGroupDefinition>> GetGroupsAsync()
    {
        if (!WebhookManagementOptions.IsDynamicWebhookStoreEnabled)
        {
            return Array.Empty<WebhookGroupDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetGroups().ToImmutableList();
        }
    }

    protected virtual async Task EnsureCacheIsUptoDateAsync()
    {
        if (StoreCache.LastCheckTime.HasValue &&
            DateTime.Now.Subtract(StoreCache.LastCheckTime.Value) < WebhookManagementOptions.WebhooksCacheRefreshInterval)
        {
            /* We get the latest webhook with a small delay for optimization */
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

    protected virtual async Task UpdateInMemoryStoreCache()
    {
        var webhookGroupRecords = await WebhookGroupRepository.GetListAsync();
        var webhookRecords = await WebhookRepository.GetListAsync();

        await StoreCache.FillAsync(webhookGroupRecords, webhookRecords);
    }

    protected virtual async Task<string> GetOrSetStampInDistributedCache()
    {
        var cacheKey = GetCommonStampCacheKey();

        var stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache;
        }

        await using (var commonLockHandle = await DistributedLock
            .TryAcquireAsync(GetCommonDistributedLockKey(), WebhookManagementOptions.WebhooksCacheStampTimeOut))
        {
            if (commonLockHandle == null)
            {
                /* This request will fail */
                throw new AbpException(
                    "Could not acquire distributed lock for webhook definition common stamp check!"
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
                    SlidingExpiration = WebhookManagementOptions.WebhooksCacheStampExpiration
                }
            );
        }

        return stampInDistributedCache;
    }

    protected virtual string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryWebhookCacheStamp";
    }
    
    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpWebhookUpdateLock";
    }
}