using System;

namespace LINGYUN.Abp.Identity.Session;
public class IdentitySessionCleanupOptions
{
    /// <summary>
    /// 是否启用会话清理
    /// 默认: false
    /// </summary>
    public bool IsCleanupEnabled { get; set; }
    /// <summary>
    /// 会话清理间隔（ms）
    /// 默认: 1小时
    /// </summary>
    public int CleanupPeriod { get; set; }
    /// <summary>
    /// 不活跃会话保持时长
    /// 默认: 30天
    /// </summary>
    public TimeSpan InactiveTimeSpan { get; set; }
    public IdentitySessionCleanupOptions()
    {
        CleanupPeriod = 3_600_000;
        InactiveTimeSpan = TimeSpan.FromDays(30);
    }
}
