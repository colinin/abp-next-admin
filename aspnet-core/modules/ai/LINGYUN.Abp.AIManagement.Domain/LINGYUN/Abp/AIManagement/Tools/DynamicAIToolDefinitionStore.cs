using JetBrains.Annotations;
using LINGYUN.Abp.AI.Tools;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.AIManagement.Tools;

[Dependency(ReplaceServices = true)]
public class DynamicAIToolDefinitionStore : IDynamicAIToolDefinitionStore, ITransientDependency
{
    protected IAIToolDefinitionRecordRepository AIToolDefinitionRecordRepository { get; }
    protected IAIToolDefinitionSerializer AIToolDefinitionSerializer { get; }
    protected IDynamicAIToolDefinitionStoreInMemoryCache StoreCache { get; }
    protected IDistributedCache DistributedCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    public AIManagementOptions AIManagementOptions { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }

    public DynamicAIToolDefinitionStore(
        IAIToolDefinitionRecordRepository aiToolDefinitionRecordRepository,
        IAIToolDefinitionSerializer aiToolDefinitionSerializer,
        IDynamicAIToolDefinitionStoreInMemoryCache storeCache,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<AIManagementOptions> aiManagementOptions,
        IAbpDistributedLock distributedLock)
    {
        AIToolDefinitionRecordRepository = aiToolDefinitionRecordRepository;
        AIToolDefinitionSerializer = aiToolDefinitionSerializer;
        StoreCache = storeCache;
        DistributedCache = distributedCache;
        DistributedLock = distributedLock;
        AIManagementOptions = aiManagementOptions.Value;
        CacheOptions = cacheOptions.Value;
    }

    public async virtual Task<IReadOnlyList<AIToolDefinition>> GetAllAsync()
    {
        if (!AIManagementOptions.IsDynamicAIToolStoreEnabled)
        {
            return Array.Empty<AIToolDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetAITools();
        }
    }

    public async virtual Task<AIToolDefinition> GetAsync([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined AITool: " + name);
    }

    public async virtual Task<AIToolDefinition?> GetOrNullAsync([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        if (!AIManagementOptions.IsDynamicAIToolStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetAIToolOrNull(name);
        }
    }
    protected virtual async Task EnsureCacheIsUptoDateAsync()
    {
        if (StoreCache.LastCheckTime.HasValue &&
            DateTime.Now.Subtract(StoreCache.LastCheckTime.Value).TotalSeconds < 30)
        {
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
        var workspaces = await AIToolDefinitionRecordRepository.GetListAsync();

        await StoreCache.FillAsync(workspaces);
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
            .TryAcquireAsync(GetCommonDistributedLockKey(), TimeSpan.FromMinutes(2)))
        {
            if (commonLockHandle == null)
            {
                throw new AbpException(
                    "Could not acquire distributed lock for AITool definition common stamp check!"
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
                    SlidingExpiration = TimeSpan.FromDays(30)
                }
            );
        }

        return stampInDistributedCache;
    }

    protected virtual string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryAIToolCacheStamp";
    }

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpAIToolUpdateLock";
    }
}
