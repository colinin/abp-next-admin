using System;
using System.Collections.Generic;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.LocalizationManagement;

[Serializable]
[IgnoreMultiTenancy]
[CacheName("AbpLocalizationTexts")]
public class LocalizationTextCacheItem
{
    private const string CacheKeyFormat = "r:{0},c:{1}";
    public string ResourceName { get; set; }
    public string CultureName { get; set; }
    public Dictionary<string, string> Texts { get; set; }
    public LocalizationTextCacheItem()
    {
        Texts = new Dictionary<string, string>();
    }
    public LocalizationTextCacheItem(string resourceName, string cultureName, Dictionary<string, string> texts)
    {
        ResourceName = resourceName;
        CultureName = cultureName;
        Texts = texts;
    }

    public static string CalculateCacheKey(string resourceName, string cultureName)
    {
        return string.Format(CacheKeyFormat, resourceName, cultureName);
    }
}
