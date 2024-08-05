namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 严重级别
/// </summary>
public enum NotificationSeverity : sbyte
{
    /// <summary>
    /// 成功
    /// </summary>
    Success = 0,
    /// <summary>
    /// 信息
    /// </summary>
    Info = 10,
    /// <summary>
    /// 警告
    /// </summary>
    Warn = 20,
    /// <summary>
    /// 错误
    /// </summary>
    Error = 30,
    /// <summary>
    /// 致命错误
    /// </summary>
    Fatal = 40
}
