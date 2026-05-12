using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement.External;

public class ExternalLocalizationStoreCache : IExternalLocalizationStoreCache, ITransientDependency
{
    protected IDistributedCache<LocalizationResourceCacheItem> ResourceCache { get; }
    protected IDistributedCache<LocalizationResourcesCacheItem> ResourcesCache { get; }
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected IResourceRepository ResourceRepository { get; }
    protected IAbpDistributedLock DistributedLock { get; }

    public ExternalLocalizationStoreCache(
        IDistributedCache<LocalizationResourcesCacheItem> resourcesCache,
        IDistributedCache<LocalizationResourceCacheItem> resourceCache,
        IOptions<AbpLocalizationOptions> localizationOptions, 
        IResourceRepository resourceRepository,
        IAbpDistributedLock distributedLock)
    {
        ResourceCache = resourceCache;
        ResourcesCache = resourcesCache;
        ResourceRepository = resourceRepository;
        DistributedLock = distributedLock;
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
        var cacheItem = ResourceCache.Get(resourceName);

        return cacheItem?.IsEnabled == true ? cacheItem : null;
    }

    public async virtual Task<LocalizationResourceCacheItem> GetResourceOrNullAsync(string resourceName)
    {
        var cacheItem = await GetResourceCacheItemAsync(resourceName);

        return cacheItem?.IsEnabled == true ? cacheItem : null;
    }

    public async virtual Task<LocalizationResourceCacheItem[]> GetResourcesAsync()
    {
        var cacheItem = await GetResourcesCacheItemAsync();

        return cacheItem?.Resources
            .Where(x => x.IsEnabled)
            .Where(x => !LocalizationOptions.Resources.ContainsKey(x.Name))
            .ToArray();
    }

    internal async Task UpdateCache(string resourceName)
    {
        if (resourceName.IsNullOrWhiteSpace())
        {
            await CreateResourcesCacheItemAsync();
        }
        else
        {
            await CreateResourceCacheItemAsync(resourceName);
        }
    }

    protected async virtual Task<LocalizationResourceCacheItem> GetResourceCacheItemAsync(string resourceName)
    {
        var cacheItem = await ResourceCache.GetAsync(resourceName);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        return await CreateResourceCacheItemAsync(resourceName);
    }

    protected async virtual Task<LocalizationResourceCacheItem> CreateResourceCacheItemAsync(string resourceName)
    {
        await using var handle = await DistributedLock.TryAcquireAsync($"{nameof(ExternalLocalizationStoreCache)}_{nameof(CreateResourceCacheItemAsync)}");

        if (handle == null)
        {
            return null;
        }

        LocalizationResourceCacheItem cacheItem;
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

        return cacheItem;
    }

    protected async virtual Task<LocalizationResourcesCacheItem> GetResourcesCacheItemAsync()
    {
        var cacheItem = await ResourcesCache.GetAsync(LocalizationResourcesCacheItem.CacheKey);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        return await CreateResourcesCacheItemAsync();
    }

    protected async virtual Task<LocalizationResourcesCacheItem> CreateResourcesCacheItemAsync()
    {
        await using var handle = await DistributedLock.TryAcquireAsync($"{nameof(ExternalLocalizationStoreCache)}_{nameof(CreateResourcesCacheItemAsync)}");

        if (handle == null)
        {
            return null;
        }

        var resources = await ResourceRepository.GetListAsync();
        var resourceItems = resources
            .Where(x => !LocalizationOptions.Resources.ContainsKey(x.Name))
            .Select(x => new LocalizationResourceCacheItem(x.Name, x.DefaultCultureName)
            {
                IsEnabled = x.Enable,
            })
            .ToList();

        var cacheItem = new LocalizationResourcesCacheItem(resourceItems);

        await ResourcesCache.SetAsync(LocalizationResourcesCacheItem.CacheKey, cacheItem);

        return cacheItem;
    }
}
