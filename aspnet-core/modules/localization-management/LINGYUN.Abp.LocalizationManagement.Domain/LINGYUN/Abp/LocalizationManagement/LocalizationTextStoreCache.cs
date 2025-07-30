using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
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
        // 同步本地化函数不执行, 阻塞线程影响性能
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
        // 同步本地化函数不执行, 阻塞线程影响性能
        return null;
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
