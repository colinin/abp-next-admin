using System;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.LocalizationManagement.External;

[IgnoreMultiTenancy]
[CacheName("AbpExternalLocalizationResource")]
public class LocalizationResourceCacheItem
{
    public virtual string Name { get; set; }

    public virtual string DefaultCulture { get; set; }

    public virtual string[] BaseResources { get; set; }

    public virtual string[] SupportedCultures { get; set; }

    public bool IsEnabled { get; set; }

    public LocalizationResourceCacheItem(
        string name, 
        string defaultCulture = null,
        string[] baseResources = null, 
        string[] supportedCultures = null)
    {
        Name = name;
        DefaultCulture = defaultCulture;
        BaseResources = baseResources ?? Array.Empty<string>();
        SupportedCultures = supportedCultures ?? Array.Empty<string>();
    }
}
