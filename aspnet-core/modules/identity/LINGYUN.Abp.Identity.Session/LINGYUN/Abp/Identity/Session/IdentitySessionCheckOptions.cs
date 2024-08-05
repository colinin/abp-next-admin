using System;

namespace LINGYUN.Abp.Identity.Session;
/// <summary>
/// 用于会话管理的配置
/// </summary>
public class IdentitySessionCheckOptions
{
    /// <summary>
    /// 保持访问时长
    /// 默认: 1分钟
    /// </summary>
    /// <remarks>
    /// 刷新缓存会话间隔时长
    /// </remarks>
    public TimeSpan KeepAccessTimeSpan { get; set; }
    /// <summary>
    /// 会话同步间隔（ms）
    /// 默认: 10分钟
    /// </summary>
    /// <remarks>
    /// 从缓存同步到持久化的间隔时长
    /// </remarks>
    public TimeSpan SessionSyncTimeSpan { get; set; }
    public IdentitySessionCheckOptions()
    {
        KeepAccessTimeSpan = TimeSpan.FromMinutes(1);
        SessionSyncTimeSpan = TimeSpan.FromMinutes(10);
    }
}
