using System;
using System.Collections.Generic;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.LocalizationManagement.External;

[Serializable]
[IgnoreMultiTenancy]
[CacheName("AbpExternalLocalizationTexts")]
public class ExternalLocalizationTextCacheItem
{
    private const string CacheKeyFormat = "r:{0},c:{1}";

    public Dictionary<string, string> Texts { get; set; }
    public ExternalLocalizationTextCacheItem()
    {
        Texts = new Dictionary<string, string>();
    }
    public ExternalLocalizationTextCacheItem(Dictionary<string, string> texts)
    {
        Texts = texts;
    }

    public static string CalculateCacheKey(string resourceName, string cultureName)
    {
        return string.Format(CacheKeyFormat, resourceName, cultureName);
    }
}
