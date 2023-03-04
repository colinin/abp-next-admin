using System;

namespace LINGYUN.Abp.TextTemplating;

public class AbpTextTemplatingCachingOptions
{
    /// <summary>
    /// 文本模板缓存最小过期时间
    /// </summary>
    public TimeSpan? MinimumCacheDuration { get; set; }
    /// <summary>
    /// 文本模板缓存绝对过期时间
    /// </summary>
    public TimeSpan? MaximumCacheDuration { get; set; }
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticTemplateDefinitionToDatabase { get; set; }
    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicTemplateDefinitionStoreEnabled { get; set; }
    /// <summary>
    /// 缓存刷新时间
    /// default: 2 hours
    /// </summary>
    public TimeSpan TemplateDefinitionsCacheRefreshInterval { get; set; }
    /// <summary>
    /// 申请时间戳超时时间
    /// default: 1 minutes
    /// </summary>
    public TimeSpan TemplateDefinitionsCacheStampTimeOut { get; set; }
    /// <summary>
    /// 时间戳过期时间
    /// default: 30 days
    /// </summary>
    public TimeSpan TemplateDefinitionsCacheStampExpiration { get; set; }

    public AbpTextTemplatingCachingOptions()
    {
        MinimumCacheDuration = TimeSpan.FromHours(1);
        MaximumCacheDuration = TimeSpan.FromDays(30);

        SaveStaticTemplateDefinitionToDatabase = true;
        IsDynamicTemplateDefinitionStoreEnabled = false;
        TemplateDefinitionsCacheRefreshInterval = TimeSpan.FromHours(2);
        TemplateDefinitionsCacheStampTimeOut = TimeSpan.FromMinutes(1);
        TemplateDefinitionsCacheStampExpiration = TimeSpan.FromDays(30);
    }
}
