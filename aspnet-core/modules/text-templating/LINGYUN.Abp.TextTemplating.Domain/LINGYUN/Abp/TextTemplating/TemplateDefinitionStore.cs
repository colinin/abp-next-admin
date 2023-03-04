using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.TextTemplating;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.TextTemplating;

public class TemplateDefinitionStore : ITemplateDefinitionStore, ITransientDependency
{
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected AbpTextTemplatingCachingOptions TemplatingCachingOptions { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IDistributedCache DistributedCache { get; }
    protected ITextTemplateDefinitionRepository TextTemplateDefinitionRepository { get; }
    protected ITemplateDefinitionManager TemplateDefinitionManager { get; }
    protected ITemplateDefinitionStoreCache TemplateDefinitionStoreCache { get; }

    public TemplateDefinitionStore(
        IOptions<AbpDistributedCacheOptions> cacheOptions, 
        IOptions<AbpTextTemplatingCachingOptions> templatingCachingOptions, 
        IAbpDistributedLock distributedLock,
        IDistributedCache distributedCache, 
        ITextTemplateDefinitionRepository textTemplateDefinitionRepository, 
        ITemplateDefinitionManager templateDefinitionManager, 
        ITemplateDefinitionStoreCache templateDefinitionStoreCache)
    {
        CacheOptions = cacheOptions.Value;
        TemplatingCachingOptions = templatingCachingOptions.Value;
        DistributedLock = distributedLock;
        DistributedCache = distributedCache;
        TextTemplateDefinitionRepository = textTemplateDefinitionRepository;
        TemplateDefinitionManager = templateDefinitionManager;
        TemplateDefinitionStoreCache = templateDefinitionStoreCache;
    }

    public async virtual Task CreateAsync(TextTemplateDefinition template)
    {
        await TextTemplateDefinitionRepository.InsertAsync(template);

        TemplateDefinitionStoreCache.LastCheckTime = DateTime.Now;
    }

    public async virtual Task UpdateAsync(TextTemplateDefinition template)
    {
        await TextTemplateDefinitionRepository.UpdateAsync(template);

        TemplateDefinitionStoreCache.LastCheckTime = DateTime.Now;
    }

    public async virtual Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        if (!TemplatingCachingOptions.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return;
        }

        using (await TemplateDefinitionStoreCache.SyncSemaphore.LockAsync())
        {
            var templateDefinitionRecord = await TextTemplateDefinitionRepository.FindByNameAsync(name);
            if (templateDefinitionRecord != null)
            {
                await TextTemplateDefinitionRepository.DeleteAsync(templateDefinitionRecord);
                // 及时更新便于下次检索刷新缓存
                TemplateDefinitionStoreCache.LastCheckTime = DateTime.Now;
            }
        }
    }

    public async virtual Task<TemplateDefinition> GetAsync([NotNull] string name, CancellationToken cancellationToken = default)
    {
        if (!TemplatingCachingOptions.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return TemplateDefinitionManager.Get(name);
        }

        using (await TemplateDefinitionStoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();

            var templateDefinition = TemplateDefinitionStoreCache.GetOrNull(name);
            templateDefinition ??= TemplateDefinitionManager.Get(name);

            return templateDefinition;
        }
    }

    public async virtual Task<IReadOnlyList<TemplateDefinition>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (!TemplatingCachingOptions.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return TemplateDefinitionManager.GetAll();
        }

        using (await TemplateDefinitionStoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();

            return TemplateDefinitionStoreCache.GetAll();
        }
    }

    public async virtual Task<TemplateDefinition> GetOrNullAsync(string name, CancellationToken cancellationToken = default)
    {
        if (!TemplatingCachingOptions.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return TemplateDefinitionManager.GetOrNull(name);
        }

        using (await TemplateDefinitionStoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();

            var templateDefinition = TemplateDefinitionStoreCache.GetOrNull(name);

            return templateDefinition;
        }
    }

    protected async virtual Task EnsureCacheIsUptoDateAsync()
    {
        if (TemplateDefinitionStoreCache.LastCheckTime.HasValue &&
            DateTime.Now.Subtract(TemplateDefinitionStoreCache.LastCheckTime.Value) < TemplatingCachingOptions.TemplateDefinitionsCacheRefreshInterval)
        {
            return;
        }

        var stampInDistributedCache = await GetOrSetStampInDistributedCache();

        if (stampInDistributedCache == TemplateDefinitionStoreCache.CacheStamp)
        {
            TemplateDefinitionStoreCache.LastCheckTime = DateTime.Now;
            return;
        }

        await UpdateInMemoryStoreCache();

        TemplateDefinitionStoreCache.CacheStamp = stampInDistributedCache;
        TemplateDefinitionStoreCache.LastCheckTime = DateTime.Now;
    }

    protected async virtual Task UpdateInMemoryStoreCache()
    {
        var templateDefinitions = TemplateDefinitionManager.GetAll();
        var textTemplateDefinitions = await TextTemplateDefinitionRepository.GetListAsync(includeDetails: false);

        await TemplateDefinitionStoreCache.FillAsync(textTemplateDefinitions, templateDefinitions);
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
            .TryAcquireAsync(GetCommonDistributedLockKey(), TemplatingCachingOptions.TemplateDefinitionsCacheStampTimeOut))
        {
            if (commonLockHandle == null)
            {
                /* This request will fail */
                throw new AbpException(
                    "Could not acquire distributed lock for template definition common stamp check!"
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
                    SlidingExpiration = TemplatingCachingOptions.TemplateDefinitionsCacheStampExpiration
                }
            );
        }

        return stampInDistributedCache;
    }

    protected virtual string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryTemplateDefinitionCacheStamp";
    }

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpTemplateDefinitionUpdateLock";
    }
}
