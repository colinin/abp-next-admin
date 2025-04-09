using System;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.LocalizationManagement.External;

[Serializable]
[IgnoreMultiTenancy]
[CacheName("AbpExternalLocalizationTextsStamp")]
public class ExternalLocalizationTextStampCacheItem
{
    private const string CacheKeyFormat = "r:{0},c:{1}";
    public string Stamp { get; set; }

    public ExternalLocalizationTextStampCacheItem()
    {

    }

    public ExternalLocalizationTextStampCacheItem(string stamp)
    {
        Stamp = stamp;
    }

    public static string CalculateCacheKey(string resourceName, string cultureName)
    {
        return string.Format(CacheKeyFormat, resourceName, cultureName);
    }
}
