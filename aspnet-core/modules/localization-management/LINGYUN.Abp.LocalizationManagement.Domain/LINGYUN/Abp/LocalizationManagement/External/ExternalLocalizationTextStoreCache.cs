using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement.External;

public class ExternalLocalizationTextStoreCache : IExternalLocalizationTextStoreCache, ISingletonDependency
{
    protected ConcurrentDictionary<string, LocalizationTextMemoryCacheItem> MemoryCache { get; }
    protected IDistributedCache<ExternalLocalizationTextCacheItem> DistributedCache { get; }
    protected IDistributedCache<ExternalLocalizationTextStampCacheItem> StampCache { get; }

    public ExternalLocalizationTextStoreCache(
        IDistributedCache<ExternalLocalizationTextCacheItem> distributedCache,
        IDistributedCache<ExternalLocalizationTextStampCacheItem> stampCache)
    {
        DistributedCache = distributedCache;
        StampCache = stampCache;

        MemoryCache = new ConcurrentDictionary<string, LocalizationTextMemoryCacheItem>();
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

            distributeCacheItem = await GetAndRefreshMemoryCacheItemAsync(resource.ResourceName, cultureName);

            if (distributeCacheItem == null)
            {
                return new Dictionary<string, string>();
            }

            return distributeCacheItem.Texts;
        }
    }
    private static bool IsShouldCheck(LocalizationTextMemoryCacheItem memoryCacheItem)
    {
        return DateTime.Now.Subtract(memoryCacheItem.LastCheckTime).TotalSeconds >= 30.0;
    }

    internal async Task UpdateCache(LocalizationResourceBase resource, string cultureName)
    {
        await GetAndRefreshMemoryCacheItemAsync(resource.ResourceName, cultureName);
    }

    internal async Task UpdateCache(string resourceName, string cultureName)
    {
        await GetAndRefreshMemoryCacheItemAsync(resourceName, cultureName);
    }

    protected async virtual Task<ExternalLocalizationTextCacheItem> GetAndRefreshMemoryCacheItemAsync(string resourceName, string cultureName)
    {
        var cacheKey = ExternalLocalizationTextCacheItem.CalculateCacheKey(resourceName, cultureName);
        var cacheItem = await DistributedCache.GetAsync(cacheKey);
        if (cacheItem != null)
        {
            var stampCacheKey = ExternalLocalizationTextStampCacheItem.CalculateCacheKey(resourceName, cultureName);

            var stampCacheItem = new ExternalLocalizationTextStampCacheItem(Guid.NewGuid().ToString());
            await StampCache.SetAsync(stampCacheKey, stampCacheItem);

            MemoryCache[cacheKey] = new LocalizationTextMemoryCacheItem(cacheItem.Texts, stampCacheItem.Stamp);

            return cacheItem;
        }

        return null;
    }
}
