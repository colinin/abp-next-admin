using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationTextStoreCache : ILocalizationTextStoreCache, ISingletonDependency
{
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
        var cacheItem = GetCacheItem(resource, cultureName);

        foreach (var text in cacheItem.Texts)
        {
            dictionary[text.Key] = new LocalizedString(text.Key, text.Value);
        }
    }

    public async virtual Task FillAsync(LocalizationResourceBase resource, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var cacheItem = await GetCacheItemAsync(resource, cultureName);

        foreach (var text in cacheItem.Texts)
        {
            dictionary[text.Key] = new LocalizedString(text.Key, text.Value);
        }
    }

    public virtual LocalizedString GetOrNull(LocalizationResourceBase resource, string cultureName, string name)
    {
        var cacheItem = GetCacheItem(resource, cultureName);

        var value = cacheItem.Texts.GetOrDefault(name);
        if (value.IsNullOrWhiteSpace())
        {
            return null;
        }

        return new LocalizedString(name, value);
    }

    protected virtual LocalizationTextCacheItem GetCacheItem(LocalizationResourceBase resource, string cultureName)
    {
        var cacheKey = LocalizationTextCacheItem.CalculateCacheKey(resource.ResourceName, cultureName);
        var cacheItem = LocalizationTextCache.Get(cacheKey);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        var setTexts = new Dictionary<string, string>();
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var provider = scope.ServiceProvider.GetRequiredService<IEntityChangeTrackingProvider>();
            using (provider.Change(false))
            {
                var repo = scope.ServiceProvider.GetRequiredService<ITextRepository>();
#pragma warning disable CS0618
                var texts = repo.GetList(resource.ResourceName, cultureName);
#pragma warning restore CS0618
                foreach (var text in texts)
                {
                    setTexts[text.Key] = text.Value;
                }
            } 
        }

        cacheItem = new LocalizationTextCacheItem(resource.ResourceName, cultureName, setTexts);

        LocalizationTextCache.Set(cacheKey, cacheItem);

        return cacheItem;
    }

    protected async virtual Task<LocalizationTextCacheItem> GetCacheItemAsync(LocalizationResourceBase resource, string cultureName)
    {
        var cacheKey = LocalizationTextCacheItem.CalculateCacheKey(resource.ResourceName, cultureName);
        var cacheItem = await LocalizationTextCache.GetAsync(cacheKey);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        var setTexts = new Dictionary<string, string>();
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var provider = scope.ServiceProvider.GetRequiredService<IEntityChangeTrackingProvider>();
            using (provider.Change(false))
            {
                var repo = scope.ServiceProvider.GetRequiredService<ITextRepository>();
                var texts = await repo.GetListAsync(resource.ResourceName, cultureName);
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
