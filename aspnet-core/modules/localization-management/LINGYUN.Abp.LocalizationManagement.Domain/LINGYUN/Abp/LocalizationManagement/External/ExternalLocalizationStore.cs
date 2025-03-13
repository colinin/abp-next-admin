using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;

namespace LINGYUN.Abp.LocalizationManagement.External;

[Dependency(ReplaceServices = true)]
public class ExternalLocalizationStore : IExternalLocalizationStore, ITransientDependency
{
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected IExternalLocalizationStoreCache StoreCache { get; }

    public ExternalLocalizationStore(
        IOptions<AbpLocalizationOptions> localizationOptions, 
        IExternalLocalizationStoreCache storeCache)
    {
        LocalizationOptions = localizationOptions.Value;
        StoreCache = storeCache;
    }

    public virtual LocalizationResourceBase GetResourceOrNull(string resourceName)
    {
        var cacheItem = StoreCache.GetResourceOrNull(resourceName);

        if (cacheItem == null || LocalizationOptions.Resources.ContainsKey(cacheItem.Name))
        {
            return null;
        }

        return CreateNonTypedLocalizationResource(cacheItem);
    }

    public async virtual Task<LocalizationResourceBase> GetResourceOrNullAsync(string resourceName)
    {
        var cacheItem = await StoreCache.GetResourceOrNullAsync(resourceName);

        if (cacheItem == null || LocalizationOptions.Resources.ContainsKey(cacheItem.Name))
        {
            return null;
        }

        return CreateNonTypedLocalizationResource(cacheItem);
    }

    public async virtual Task<string[]> GetResourceNamesAsync()
    {
        var cacheNames = await StoreCache.GetResourceNamesAsync();

        return cacheNames
            .Where(name => !LocalizationOptions.Resources.ContainsKey(name))
            .ToArray();
    }

    public async virtual Task<LocalizationResourceBase[]> GetResourcesAsync()
    {
        var cacheItem = await StoreCache.GetResourcesAsync();


        if (cacheItem == null)
        {
            return Array.Empty<LocalizationResourceBase>();
        }

        return cacheItem
            .Where(x => x.IsEnabled)
            .Where(x => !LocalizationOptions.Resources.ContainsKey(x.Name))
            .Select(CreateNonTypedLocalizationResource)
            .ToArray();
    }

    protected virtual NonTypedLocalizationResource CreateNonTypedLocalizationResource(LocalizationResourceCacheItem cacheItem)
    {
        var localizationResource = new NonTypedLocalizationResource(
            cacheItem.Name,
            cacheItem.DefaultCulture);

        if (cacheItem.BaseResources.Length > 0)
        {
            localizationResource.AddBaseResources(cacheItem.BaseResources);
        }

        localizationResource.Contributors.Add(new ExternalLocalizationResourceContributor());

        return localizationResource;
    }
}