using System;
using System.Collections.Generic;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.LocalizationManagement;

[Serializable]
[IgnoreMultiTenancy]
[CacheName("DynamicLanguageInitializer")]
public class DynamicLanguageInitializerCacheItem
{
    public string CultureName { get; set; }
    public string UiCultureName { get; set; }
    public string DisplayName { get; set; }
    public string TwoLetterISOLanguageName { get; set; }
    public DynamicLanguageInitializerCacheItem()
    {
    }
    public DynamicLanguageInitializerCacheItem(
        string cultureName,
        string uiCultureName,
        string displayName,
        string twoLetterISOLanguageName)
    {
        CultureName = cultureName;
        UiCultureName = uiCultureName;
        DisplayName = displayName;
        TwoLetterISOLanguageName = twoLetterISOLanguageName;
    }
}

[Serializable]
[IgnoreMultiTenancy]
[CacheName("DynamicResourceInitializer")]
public class DynamicResourceInitializerCacheItem
{
    public string ResourceName { get; set; }
    public string DefaultCultureName { get; set; }
    public DynamicResourceInitializerCacheItem()
    {
    }
    public DynamicResourceInitializerCacheItem(
        string resourceName,
        string defaultCultureName)
    {
        ResourceName = resourceName;
        DefaultCultureName = defaultCultureName;
    }
}

[Serializable]
[IgnoreMultiTenancy]
[CacheName("DynamicTextInitializer")]
public class DynamicTextInitializerCacheItem
{
    public string ResourceName { get; set; }
    public string CultureName { get; set; }
    public Dictionary<string, string> Texts { get; set; }
    public DynamicTextInitializerCacheItem()
    {
        Texts = new Dictionary<string, string>();
    }
    public DynamicTextInitializerCacheItem(
        string resourceName,
        string cultureName,
        Dictionary<string, string> texts)
    {
        ResourceName = resourceName;
        CultureName = cultureName;
        Texts = texts;
    }
}
