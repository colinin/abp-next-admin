using System;
using System.Collections.Generic;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.LocalizationManagement;

[Serializable]
[IgnoreMultiTenancy]
public class LocalizationLanguageCacheItem
{
    internal const string CacheKey = "Abp.Localization.Languages";
    public List<LanguageInfo> Languages { get; set; }

    public LocalizationLanguageCacheItem()
    {

    }

    public LocalizationLanguageCacheItem(List<LanguageInfo> languages)
    {
        Languages = languages;
    }
}
