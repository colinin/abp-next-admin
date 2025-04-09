using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.LocalizationManagement.External;

public class LocalizationTextMemoryCacheItem
{
    private const string CacheKeyFormat = "r:{0},c:{1}";

    public string CacheStamp { get; }

    public DateTime LastCheckTime { get; set; }

    public Dictionary<string, string> Texts { get; set; }

    public LocalizationTextMemoryCacheItem()
    {
        Texts = new Dictionary<string, string>();
    }
    public LocalizationTextMemoryCacheItem(Dictionary<string, string> texts, string cacheStamp)
    {
        Texts = texts;
        CacheStamp = cacheStamp;
        LastCheckTime = DateTime.Now;
    }

    public static string CalculateCacheKey(string resourceName, string cultureName)
    {
        return string.Format(CacheKeyFormat, resourceName, cultureName);
    }
}
