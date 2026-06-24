using LINGYUN.Abp.LocalizationManagement.External;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.LocalizationManagement;

public class DynamicLocalizationChangedEventHandler :
    IDistributedEventHandler<DynamicLocalizationChangedEto>,
    ITransientDependency
{
    protected IDistributedCache<LocalizationResourcesCacheItem> ResourcesCache { get; }
    protected IDistributedCache<LocalizationLanguageCacheItem> LanguageCache { get; }

    protected IExternalLocalizationTextStoreCache ExternalLocalizationTextStoreCache { get; }
    protected ILocalizationTextStoreCache LocalizationTextStoreCache { get; }

    public DynamicLocalizationChangedEventHandler(
        IDistributedCache<LocalizationResourcesCacheItem> resourcesCache, 
        IDistributedCache<LocalizationLanguageCacheItem> languageCache, 
        IExternalLocalizationTextStoreCache externalLocalizationTextStoreCache,
        ILocalizationTextStoreCache localizationTextStoreCache)
    {
        ResourcesCache = resourcesCache;
        LanguageCache = languageCache;
        ExternalLocalizationTextStoreCache = externalLocalizationTextStoreCache;
        LocalizationTextStoreCache = localizationTextStoreCache;
    }

    public async virtual Task HandleEventAsync(DynamicLocalizationChangedEto eventData)
    {
        var resourcesCacheItem = await ResourcesCache.GetAsync(LocalizationResourcesCacheItem.CacheKey);
        var languagesCacheItem = await LanguageCache.GetAsync(LocalizationLanguageCacheItem.CacheKey);
        if (languagesCacheItem == null || resourcesCacheItem == null)
        {
            return;
        }
        foreach (var language in languagesCacheItem.Languages)
        {
            foreach (var resource in resourcesCacheItem.Resources)
            {
                if (ExternalLocalizationTextStoreCache is ExternalLocalizationTextStoreCache storeCache)
                {
                    await storeCache.UpdateCache(resource.Name, language.CultureName);
                }
                if (LocalizationTextStoreCache is LocalizationTextStoreCache textStoreCache)
                {
                    await textStoreCache.UpdateCache(resource.Name, language.CultureName);
                }
            }
        }
    }
}
