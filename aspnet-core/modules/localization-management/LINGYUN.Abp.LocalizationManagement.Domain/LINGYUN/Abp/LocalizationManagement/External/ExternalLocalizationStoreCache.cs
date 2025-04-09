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
    protected IResourceRepository ResourceRepository { get; }

    public ExternalLocalizationStoreCache(
        IDistributedCache<LocalizationResourcesCacheItem> resourcesCache,
        IDistributedCache<LocalizationResourceCacheItem> resourceCache,
        IOptions<AbpLocalizationOptions> localizationOptions, 
        IResourceRepository resourceRepository)
    {
        ResourceCache = resourceCache;
        ResourcesCache = resourcesCache;
        ResourceRepository = resourceRepository;
        LocalizationOptions = localizationOptions.Value;
    }

    public async virtual Task RemoveAsync(string resourceName)
    {
        await ResourceCache.RemoveAsync(resourceName);
        await ResourcesCache.RemoveAsync(LocalizationResourcesCacheItem.CacheKey);
    }

    public async virtual Task<string[]> GetResourceNamesAsync()
    {
        var cacheItem = await GetResourcesCacheItemAsync();

        return cacheItem.Resources
            .Where(x => x.IsEnabled)
            .Where(x => !LocalizationOptions.Resources.ContainsKey(x.Name))
            .Select(x => x.Name)
            .ToArray();
    }

    public virtual LocalizationResourceCacheItem GetResourceOrNull(string resourceName)
    {
        var cacheItem = GetResourceCacheItem(resourceName);
        
        return cacheItem.IsEnabled ? cacheItem : null;
    }

    public async virtual Task<LocalizationResourceCacheItem> GetResourceOrNullAsync(string resourceName)
    {
        var cacheItem = await GetResourceCacheItemAsync(resourceName);

        return cacheItem.IsEnabled ? cacheItem : null;
    }

    public async virtual Task<LocalizationResourceCacheItem[]> GetResourcesAsync()
    {
        var cacheItem = await GetResourcesCacheItemAsync();

        return cacheItem.Resources
            .Where(x => x.IsEnabled)
            .Where(x => !LocalizationOptions.Resources.ContainsKey(x.Name))
            .ToArray();
    }

    protected virtual LocalizationResourceCacheItem GetResourceCacheItem(string resourceName)
    {
        var cacheItem = ResourceCache.Get(resourceName);
        if (cacheItem == null)
        {
            var resource = ResourceRepository.FindByName(resourceName);

            if (resource == null)
            {
                cacheItem = new LocalizationResourceCacheItem(resourceName)
                {
                    IsEnabled = false
                };
            }
            else
            {
                cacheItem = new LocalizationResourceCacheItem(resource.Name, resource.DefaultCultureName)
                {
                    IsEnabled = resource.Enable
                };
            }

            ResourceCache.Set(resourceName, cacheItem);
        }

        return cacheItem;
    }

    protected async virtual Task<LocalizationResourceCacheItem> GetResourceCacheItemAsync(string resourceName)
    {
        var cacheItem = await ResourceCache.GetAsync(resourceName);
        if (cacheItem == null)
        {
            var resource = await ResourceRepository.FindByNameAsync(resourceName);
            if (resource == null)
            {
                cacheItem = new LocalizationResourceCacheItem(resourceName)
                {
                    IsEnabled = false
                };
            }
            else
            {
                cacheItem = new LocalizationResourceCacheItem(resource.Name, resource.DefaultCultureName)
                {
                    IsEnabled = resource.Enable
                };
            }

            await ResourceCache.SetAsync(resourceName, cacheItem);
        }

        return cacheItem;
    }

    protected async virtual Task<LocalizationResourcesCacheItem> GetResourcesCacheItemAsync()
    {
        var cacheItem = await ResourcesCache.GetAsync(LocalizationResourcesCacheItem.CacheKey);
        if (cacheItem == null)
        {
            var resources = await ResourceRepository.GetListAsync();
            var resourceItems = resources
                .Where(x => !LocalizationOptions.Resources.ContainsKey(x.Name))
                .Select(x => new LocalizationResourceCacheItem(x.Name, x.DefaultCultureName)
                {
                    IsEnabled = x.Enable,
                })
                .ToList();

            cacheItem = new LocalizationResourcesCacheItem(resourceItems);

            await ResourcesCache.SetAsync(LocalizationResourcesCacheItem.CacheKey, cacheItem);
        }

        return cacheItem;
    }
}
