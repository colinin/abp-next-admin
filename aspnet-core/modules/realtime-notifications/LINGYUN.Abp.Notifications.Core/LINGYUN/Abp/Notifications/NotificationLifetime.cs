namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知存活时间
/// 发送之后取消用户订阅,类似于微信小程序
/// </summary>
public enum NotificationLifetime
{
    /// <summary>
    /// 持久化
    /// </summary>
    Persistent = 0,
    /// <summary>
    /// 一次性
    /// </summary>
    OnlyOne = 1
}
