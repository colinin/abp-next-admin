using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
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

[Dependency(ServiceLifetime.Transient)]
[ExposeServices(
    typeof(ITemplateDefinitionStore),
    typeof(IDynamicTemplateDefinitionStore))]
public class TemplateDefinitionStore : ITemplateDefinitionStore, IDynamicTemplateDefinitionStore, ITransientDependency
{
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected AbpTextTemplatingCachingOptions TemplatingCachingOptions { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IDistributedCache DistributedCache { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected ITextTemplateDefinitionRepository TextTemplateDefinitionRepository { get; }
    protected IStaticTemplateDefinitionStore StaticTemplateDefinitionStore { get; }
    protected ITemplateDefinitionStoreCache TemplateDefinitionStoreCache { get; }

    public TemplateDefinitionStore(
        IOptions<AbpDistributedCacheOptions> cacheOptions, 
        IOptions<AbpTextTemplatingCachingOptions> templatingCachingOptions, 
        IAbpDistributedLock distributedLock,
        IDistributedCache distributedCache,
        ICancellationTokenProvider cancellationTokenProvider,
        ITextTemplateDefinitionRepository textTemplateDefinitionRepository,
        IStaticTemplateDefinitionStore staticTemplateDefinitionStore, 
        ITemplateDefinitionStoreCache templateDefinitionStoreCache)
    {
        CacheOptions = cacheOptions.Value;
        TemplatingCachingOptions = templatingCachingOptions.Value;
        DistributedLock = distributedLock;
        DistributedCache = distributedCache;
        CancellationTokenProvider = cancellationTokenProvider;
        TextTemplateDefinitionRepository = textTemplateDefinitionRepository;
        StaticTemplateDefinitionStore = staticTemplateDefinitionStore;
        TemplateDefinitionStoreCache = templateDefinitionStoreCache;
    }

    public async virtual Task CreateAsync(TextTemplateDefinition template)
    {
        await TextTemplateDefinitionRepository.InsertAsync(template);

        TemplateDefinitionStoreCache.LastCheckTime = DateTime.Now.Add(-TemplatingCachingOptions.TemplateDefinitionsCacheRefreshInterval);
    }

    public async virtual Task UpdateAsync(TextTemplateDefinition template)
    {
        await TextTemplateDefinitionRepository.UpdateAsync(template);

        TemplateDefinitionStoreCache.LastCheckTime = DateTime.Now.Add(-TemplatingCachingOptions.TemplateDefinitionsCacheRefreshInterval);
    }

    public async virtual Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        if (!TemplatingCachingOptions.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return;
        }

        using (await TemplateDefinitionStoreCache.SyncSemaphore.LockAsync())
        {
            var templateDefinitionRecord = await TextTemplateDefinitionRepository.FindByNameAsync(name, GetCancellationToken(cancellationToken));
            if (templateDefinitionRecord != null)
            {
                await TextTemplateDefinitionRepository.DeleteAsync(templateDefinitionRecord, cancellationToken: GetCancellationToken(cancellationToken));
                // 及时更新便于下次检索刷新缓存
                TemplateDefinitionStoreCache.LastCheckTime = DateTime.Now.Add(-TemplatingCachingOptions.TemplateDefinitionsCacheRefreshInterval);
            }
        }
    }

    public async virtual Task<TemplateDefinition> GetAsync([NotNull] string name, CancellationToken cancellationToken = default)
    {
        if (!TemplatingCachingOptions.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return null;
        }

        using (await TemplateDefinitionStoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();

            return TemplateDefinitionStoreCache.GetOrNull(name);
        }
    }
    public async virtual Task<TemplateDefinition> GetAsync(string name)
    {
        return await GetAsync(name, GetCancellationToken());
    }

    public async virtual Task<IReadOnlyList<TemplateDefinition>> GetAllAsync()
    {
        return await GetAllAsync(GetCancellationToken());
    }

    public async virtual Task<TemplateDefinition> GetOrNullAsync(string name)
    {
        return await GetOrNullAsync(name, GetCancellationToken());
    }

    public async virtual Task<IReadOnlyList<TemplateDefinition>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (!TemplatingCachingOptions.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return Array.Empty<TemplateDefinition>();
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
            return null;
        }

        using (await TemplateDefinitionStoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();

            return TemplateDefinitionStoreCache.GetOrNull(name);
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
        var templateDefinitions = await StaticTemplateDefinitionStore.GetAllAsync();

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

    protected virtual CancellationToken GetCancellationToken(CancellationToken cancellationToken = default)
    {
        return CancellationTokenProvider.FallbackToProvider(cancellationToken);
    }
}
