using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.LocalizationManagement.External;

[IgnoreMultiTenancy]
[CacheName("AbpExternalLocalizationResources")]
public class LocalizationResourcesCacheItem
{
    public const string CacheKey = "All";

    public List<LocalizationResourceCacheItem> Resources { get; set; }

    public LocalizationResourcesCacheItem()
    {
        Resources = new List<LocalizationResourceCacheItem>();
    }

    public LocalizationResourcesCacheItem(List<LocalizationResourceCacheItem> resources)
    {
        Resources = resources;
    }

    public LocalizationResourceCacheItem GetResourceOrNull(string name)
    {
        return Resources?.FirstOrDefault(x => x.Name == name);
    }
}
