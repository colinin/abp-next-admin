using System;
using System.Collections.Generic;
using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.LocalizationManagement;

[Serializable]
[IgnoreMultiTenancy]
[CacheName("AbpLocalizationLanguages")]
public class LocalizationLanguageCacheItem
{
    public const string CacheKey = "All";
    public List<LanguageInfo> Languages { get; set; }

    public LocalizationLanguageCacheItem()
    {

    }

    public LocalizationLanguageCacheItem(List<LanguageInfo> languages)
    {
        Languages = languages;
    }
}
