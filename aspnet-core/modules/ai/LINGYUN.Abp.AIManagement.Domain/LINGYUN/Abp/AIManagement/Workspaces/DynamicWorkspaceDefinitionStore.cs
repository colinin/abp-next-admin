using JetBrains.Annotations;
using LINGYUN.Abp.AI.Workspaces;
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

namespace LINGYUN.Abp.AIManagement.Workspaces;

[Dependency(ReplaceServices = true)]
public class DynamicWorkspaceDefinitionStore : IDynamicWorkspaceDefinitionStore, ITransientDependency
{
    protected IWorkspaceDefinitionRecordRepository WorkspaceDefinitionRecordRepository { get; }
    protected IWorkspaceDefinitionSerializer WorkspaceDefinitionSerializer { get; }
    protected IDynamicWorkspaceDefinitionStoreInMemoryCache StoreCache { get; }
    protected IDistributedCache DistributedCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    public AIManagementOptions AIManagementOptions { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }

    public DynamicWorkspaceDefinitionStore(
        IWorkspaceDefinitionRecordRepository workspaceRepository,
        IWorkspaceDefinitionSerializer workspaceDefinitionSerializer,
        IDynamicWorkspaceDefinitionStoreInMemoryCache storeCache,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<AIManagementOptions> aiManagementOptions,
        IAbpDistributedLock distributedLock)
    {
        WorkspaceDefinitionRecordRepository = workspaceRepository;
        WorkspaceDefinitionSerializer = workspaceDefinitionSerializer;
        StoreCache = storeCache;
        DistributedCache = distributedCache;
        DistributedLock = distributedLock;
        AIManagementOptions = aiManagementOptions.Value;
        CacheOptions = cacheOptions.Value;
    }

    public async virtual Task<IReadOnlyList<WorkspaceDefinition>> GetAllAsync()
    {
        if (!AIManagementOptions.IsDynamicWorkspaceStoreEnabled)
        {
            return Array.Empty<WorkspaceDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetWorkspaces();
        }
    }

    public async virtual Task<WorkspaceDefinition> GetAsync([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined workspace: " + name);
    }

    public async virtual Task<WorkspaceDefinition?> GetOrNullAsync([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        if (!AIManagementOptions.IsDynamicWorkspaceStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetWorkspaceOrNull(name);
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
        var workspaces = await WorkspaceDefinitionRecordRepository.GetListAsync();

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
                    "Could not acquire distributed lock for workspace definition common stamp check!"
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
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryWorkspaceCacheStamp";
    }

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpWorkspaceUpdateLock";
    }
}
