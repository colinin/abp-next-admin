namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知类型
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// 应用（仅对当前租户）
    /// </summary>
    Application = 0,
    /// <summary>
    /// 系统通知（全局发布）
    /// </summary>
    System = 10,
    /// <summary>
    /// 用户（对应用户,受租户控制）
    /// </summary>
    User = 20,
    /// <summary>
    /// 服务端回调,用户不应进行处理
    /// </summary>
    ServiceCallback = 30,
}
