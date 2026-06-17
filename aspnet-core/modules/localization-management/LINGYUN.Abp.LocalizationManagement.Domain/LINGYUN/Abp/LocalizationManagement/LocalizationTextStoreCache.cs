using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationTextStoreCache : ILocalizationTextStoreCache, ISingletonDependency
{
    private static readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);
    private static readonly LocalizationResourceDictionary _staticCache = new LocalizationResourceDictionary();
    protected IDistributedCache<LocalizationTextCacheItem> LocalizationTextCache { get; }
    public LocalizationTextStoreCache(
        IDistributedCache<LocalizationTextCacheItem> localizationTextCache)
    {
        LocalizationTextCache = localizationTextCache;
    }

    public virtual void Fill(LocalizationResourceBase resource, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        if (_staticCache.TryGetValue(resource.ResourceName, out var cultureLocalCache) &&
            cultureLocalCache.TryGetValue(cultureName, out var textLocalCache))
        {
            foreach (var text in textLocalCache)
            {
                dictionary[text.Key] = new LocalizedString(text.Key, text.Value);
            }
        }
    }

    public async virtual Task FillAsync(LocalizationResourceBase resource, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var cacheItem = await GetCacheItemAsync(resource.ResourceName, cultureName);
        if (cacheItem != null)
        {
            foreach (var text in cacheItem.Texts)
            {
                var localizedString = new LocalizedString(text.Key, text.Value);
                dictionary[text.Key] = localizedString;
            }
        }
    }

    public virtual LocalizedString GetOrNull(LocalizationResourceBase resource, string cultureName, string name)
    {
        if (_staticCache.TryGetValue(resource.ResourceName, out var cultureLocalCache) &&
            cultureLocalCache.TryGetValue(cultureName, out var textLocalCache))
        {
            return textLocalCache.GetOrDefault(name);
        }
        return null;
    }

    internal async Task UpdateCache(LocalizationResourceBase resource, string cultureName)
    {
        await UpdateCache(resource.ResourceName, cultureName);
    }

    internal async Task UpdateCache(string resourceName, string cultureName)
    {
        using (await _cacheLock.LockAsync())
        {
            var cacheItem = await GetCacheItemAsync(resourceName, cultureName);
            if (cacheItem != null)
            {
                var textDic = _staticCache
                .GetOrAdd(resourceName, _ => new LocalizationCultureDictionary())
                .GetOrAdd(cultureName, _ => new LocalizationTextDictionary());

                foreach (var text in cacheItem.Texts)
                {
                    textDic[text.Key] = new LocalizedString(text.Key, text.Value);
                }
            }
        }
    }

    protected async virtual Task<LocalizationTextCacheItem> GetCacheItemAsync(string resourceName, string cultureName)
    {
        var cacheKey = LocalizationTextCacheItem.CalculateCacheKey(resourceName, cultureName);
        return await LocalizationTextCache.GetAsync(cacheKey);
    }
}
