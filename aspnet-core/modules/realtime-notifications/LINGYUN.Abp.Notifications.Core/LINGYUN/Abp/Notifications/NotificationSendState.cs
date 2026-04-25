namespace LINGYUN.Abp.Notifications;
/// <summary>
/// 发送状态
/// </summary>
public enum NotificationSendState
{
    /// <summary>
    /// 未发送
    /// </summary>
    None,
    /// <summary>
    /// 提供者禁用
    /// </summary>
    Disabled,
    /// <summary>
    /// 已发送
    /// </summary>
    Sent,
    /// <summary>
    /// 发送失败
    /// </summary>
    Failed
}
