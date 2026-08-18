using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Packages;

[EventName("platform.packages")]
public class PackageEto
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = default!;
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; } = default!;
    /// <summary>
    /// 强制更新
    /// </summary>
    public bool ForceUpdate { get; set; }
    public PackageLevel Level { get; set; }
}
