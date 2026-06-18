using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement.External;

public class ExternalLocalizationStoreCache : IExternalLocalizationStoreCache, ITransientDependency
{
    protected IDistributedCache<LocalizationResourceCacheItem> ResourceCache { get; }
    protected IDistributedCache<LocalizationResourcesCacheItem> ResourcesCache { get; }
    protected AbpLocalizationOptions LocalizationOptions { get; }

    public ExternalLocalizationStoreCache(
        IDistributedCache<LocalizationResourcesCacheItem> resourcesCache,
        IDistributedCache<LocalizationResourceCacheItem> resourceCache,
        IOptions<AbpLocalizationOptions> localizationOptions)
    {
        ResourceCache = resourceCache;
        ResourcesCache = resourcesCache;
        LocalizationOptions = localizationOptions.Value;
    }

    public async virtual Task<string[]> GetResourceNamesAsync()
    {
        var cacheItem = await ResourcesCache.GetAsync(LocalizationResourcesCacheItem.CacheKey);

        return cacheItem.Resources
            .Where(x => x.IsEnabled)
            .Where(x => !LocalizationOptions.Resources.ContainsKey(x.Name))
            .Select(x => x.Name)
            .ToArray();
    }

    public virtual LocalizationResourceCacheItem GetResourceOrNull(string resourceName)
    {
        var cacheItem = ResourceCache.Get(resourceName);

        return cacheItem?.IsEnabled == true ? cacheItem : null;
    }

    public async virtual Task<LocalizationResourceCacheItem> GetResourceOrNullAsync(string resourceName)
    {
        var cacheItem = await ResourceCache.GetAsync(resourceName);

        return cacheItem?.IsEnabled == true ? cacheItem : null;
    }

    public async virtual Task<LocalizationResourceCacheItem[]> GetResourcesAsync()
    {
        var cacheItem = await ResourcesCache.GetAsync(LocalizationResourcesCacheItem.CacheKey);

        return cacheItem?.Resources
            .Where(x => x.IsEnabled)
            .Where(x => !LocalizationOptions.Resources.ContainsKey(x.Name))
            .ToArray();
    }
}
