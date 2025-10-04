using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationTextStoreCache : ILocalizationTextStoreCache, ISingletonDependency
{
    private static readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);
    private static readonly LocalizationResourceDictionary _staticCache = new LocalizationResourceDictionary();
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IDistributedCache<LocalizationTextCacheItem> LocalizationTextCache { get; }
    public LocalizationTextStoreCache(
        IServiceScopeFactory serviceScopeFactory, 
        IDistributedCache<LocalizationTextCacheItem> localizationTextCache)
    {
        ServiceScopeFactory = serviceScopeFactory;
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
        var cacheItem = await GetCacheItemAsync(resource, cultureName);

        foreach (var text in cacheItem.Texts)
        {
            var localizedString = new LocalizedString(text.Key, text.Value);
            dictionary[text.Key] = localizedString;
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

    internal async Task UpdateStaticCache(LocalizationResourceBase resource, string cultureName)
    {
        using (await _cacheLock.LockAsync())
        {
            var cacheItem = await GetCacheItemAsync(resource, cultureName);
            var textDic = _staticCache
                .GetOrAdd(resource.ResourceName, _ => new LocalizationCultureDictionary())
                .GetOrAdd(cultureName, _ => new LocalizationTextDictionary());

            foreach (var text in cacheItem.Texts)
            {
                textDic[text.Key] = new LocalizedString(text.Key, text.Value);
            }
        }
    }

    protected async virtual Task<LocalizationTextCacheItem> GetCacheItemAsync(LocalizationResourceBase resource, string cultureName)
    {
        // 异步本地化函数不受影响
        var cacheKey = LocalizationTextCacheItem.CalculateCacheKey(resource.ResourceName, cultureName);
        var cacheItem = await LocalizationTextCache.GetAsync(cacheKey);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        var setTexts = new Dictionary<string, string>();
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ITextRepository>();
            var texts = await repo.GetListAsync(resource.ResourceName, cultureName);
            using (repo.DisableTracking())
            {
                foreach (var text in texts)
                {
                    setTexts[text.Key] = text.Value;
                }
            }
        }

        cacheItem = new LocalizationTextCacheItem(resource.ResourceName, cultureName, setTexts);

        await LocalizationTextCache.SetAsync(cacheKey, cacheItem);

        return cacheItem;
    }
}
