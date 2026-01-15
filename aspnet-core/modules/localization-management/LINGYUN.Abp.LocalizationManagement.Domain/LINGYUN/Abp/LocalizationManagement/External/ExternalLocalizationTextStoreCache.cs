using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.LocalizationManagement.External;

public class ExternalLocalizationTextStoreCache : IExternalLocalizationTextStoreCache, ISingletonDependency
{
    protected ConcurrentDictionary<string, LocalizationTextMemoryCacheItem> MemoryCache { get; }

    protected IAbpDistributedLock DistributedLock { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IDistributedCache<ExternalLocalizationTextCacheItem> DistributedCache { get; }
    protected IDistributedCache<ExternalLocalizationTextStampCacheItem> StampCache { get; }

    public ExternalLocalizationTextStoreCache(
        IAbpDistributedLock distributedLock,
        IServiceScopeFactory serviceScopeFactory, 
        IDistributedCache<ExternalLocalizationTextCacheItem> distributedCache,
        IDistributedCache<ExternalLocalizationTextStampCacheItem> stampCache)
    {
        DistributedLock = distributedLock;
        ServiceScopeFactory = serviceScopeFactory;
        DistributedCache = distributedCache;
        StampCache = stampCache;

        MemoryCache = new ConcurrentDictionary<string, LocalizationTextMemoryCacheItem>();
    }

    public async virtual Task RemoveAsync(string resourceName, string cultureName)
    {
        var cacheKey = ExternalLocalizationTextCacheItem.CalculateCacheKey(resourceName, cultureName);
        var stampCacheKey = ExternalLocalizationTextStampCacheItem.CalculateCacheKey(resourceName, cultureName);

        await using var lockHandle = await DistributedLock.TryAcquireAsync(cacheKey, TimeSpan.FromMinutes(1d));

        await DistributedCache.RemoveAsync(cacheKey);

        await StampCache.SetAsync(stampCacheKey, new ExternalLocalizationTextStampCacheItem(Guid.NewGuid().ToString()));
    }

    public Dictionary<string, string> GetTexts(LocalizationResourceBase resource, string cultureName)
    {
        var cacheKey = ExternalLocalizationTextCacheItem.CalculateCacheKey(resource.ResourceName, cultureName);
        var memoryCacheItem = MemoryCache.GetOrDefault(cacheKey);
        if (memoryCacheItem != null && !IsShouldCheck(memoryCacheItem))
        {
            return memoryCacheItem.Texts;
        }

        return new Dictionary<string, string>();
    }

    public async virtual Task<Dictionary<string, string>> GetTextsAsync(LocalizationResourceBase resource, string cultureName)
    {
        var cacheKey = ExternalLocalizationTextCacheItem.CalculateCacheKey(resource.ResourceName, cultureName);
        var memoryCacheItem = MemoryCache.GetOrDefault(cacheKey);
        if (memoryCacheItem != null && !IsShouldCheck(memoryCacheItem))
        {
            return memoryCacheItem.Texts;
        }

        using (await KeyedLock.LockAsync(cacheKey))
        {
            memoryCacheItem = MemoryCache.GetOrDefault(cacheKey);
            if (memoryCacheItem != null && !IsShouldCheck(memoryCacheItem))
            {
                return memoryCacheItem.Texts;
            }

            var stampCacheKey = ExternalLocalizationTextStampCacheItem.CalculateCacheKey(resource.ResourceName, cultureName);
            var stampCacheItem = await StampCache.GetAsync(stampCacheKey);

            if (memoryCacheItem != null && memoryCacheItem.CacheStamp == stampCacheItem?.Stamp)
            {
                memoryCacheItem.LastCheckTime = DateTime.Now;
                return memoryCacheItem.Texts;
            }

            var distributeCacheItem = await DistributedCache.GetAsync(cacheKey);
            if (stampCacheItem != null && distributeCacheItem != null)
            {
                MemoryCache[cacheKey] = new LocalizationTextMemoryCacheItem(distributeCacheItem.Texts, stampCacheItem.Stamp);

                return distributeCacheItem.Texts;
            }

            await using var lockHandle = await DistributedLock.TryAcquireAsync(cacheKey, TimeSpan.FromMinutes(1d));

            if (lockHandle == null)
            {
                return new Dictionary<string, string>();
            }

            distributeCacheItem = await CreateCacheItemAsync(resource, cultureName);

            await DistributedCache.SetAsync(cacheKey, distributeCacheItem);

            stampCacheItem = new ExternalLocalizationTextStampCacheItem(Guid.NewGuid().ToString());
            await StampCache.SetAsync(stampCacheKey, stampCacheItem);

            MemoryCache[cacheKey] = new LocalizationTextMemoryCacheItem(distributeCacheItem.Texts, stampCacheItem.Stamp);

            return distributeCacheItem.Texts;
        }
    }
    private static bool IsShouldCheck(LocalizationTextMemoryCacheItem memoryCacheItem)
    {
        return DateTime.Now.Subtract(memoryCacheItem.LastCheckTime).TotalSeconds >= 30.0;
    }

    protected async virtual Task<ExternalLocalizationTextCacheItem> CreateCacheItemAsync(LocalizationResourceBase resource, string cultureName)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var unitOfWork = unitOfWorkManager.Begin(requiresNew: true);
        var repo = scope.ServiceProvider.GetRequiredService<ITextRepository>();

        var texts = await repo.GetListAsync(resource.ResourceName, cultureName);

        var fillTexts = new Dictionary<string, string>();

        foreach (var text in texts)
        {
            fillTexts[text.Key] = text.Value;
        }

        return new ExternalLocalizationTextCacheItem(fillTexts);
    }
}
