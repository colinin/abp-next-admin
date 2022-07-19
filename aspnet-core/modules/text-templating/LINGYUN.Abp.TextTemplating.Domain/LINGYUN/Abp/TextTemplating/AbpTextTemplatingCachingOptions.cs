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

    public AbpTextTemplatingCachingOptions()
    {
        MinimumCacheDuration = TimeSpan.FromHours(1);
        MaximumCacheDuration = TimeSpan.FromDays(30);
    }
}
