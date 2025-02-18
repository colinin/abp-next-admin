using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Gdpr;
/// <summary>
/// AbpGdpr选项
/// See: https://abp.io/docs/latest/modules/gdpr#abpgdproptions
/// </summary>
public class AbpGdprOptions
{
    /// <summary>
    /// 用于指示允许的请求时间间隔.<br />
    /// 如果要增加或减少个人数据请求间隔,可以配置此属性.<br />
    /// 默认情况下, 用户可以每天请求一次其个人数据<br />
    /// 默认值: 1 天
    /// </summary>
    public TimeSpan RequestTimeInterval { get; set; }
    /// <summary>
    /// 由于 GDPR 模块旨在支持分布式方案, 因此收集和准备个人数据应该需要一段时间.
    /// 如果要根据应用程序的大小增加或减少数据准备时间, 可以配置此属性.<br />
    /// 默认值: 60 分钟
    /// </summary>
    public TimeSpan MinutesForDataPreparation { get; set; }
    /// <summary>
    /// 用户数据提供者列表
    /// </summary>
    public IList<IGdprUserDataProvider> GdprUserDataProviders { get; }
    /// <summary>
    /// 用户账户提供者列表
    /// </summary>
    public IList<IGdprUserAccountProvider> GdprUserAccountProviders { get; }
    public AbpGdprOptions()
    {
        RequestTimeInterval = TimeSpan.FromDays(1);
        MinutesForDataPreparation = TimeSpan.FromMinutes(60);

        GdprUserDataProviders = new List<IGdprUserDataProvider>();
        GdprUserAccountProviders = new List<IGdprUserAccountProvider>();
    }
}
