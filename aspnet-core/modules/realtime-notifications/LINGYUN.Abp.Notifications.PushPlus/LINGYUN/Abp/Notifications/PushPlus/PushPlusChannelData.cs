using LINGYUN.Abp.Notifications;

namespace LINGYUN.Abp.Notifications.PushPlus;

/// <summary>
/// PushPlus 渠道数据(载荷)
/// </summary>
/// <remarks>
/// webhook 与 callback 是"可选参数"而非互斥的"载荷标识":
/// 写入其中任意一个即表示生产者要使用 PushPlus 渠道,
/// 因此存在性判定模式声明为 <see cref="NotificationChannelDataPresenceMode.AnyDeclaredProperty"/>。
/// </remarks>
[NotificationChannelData(
    PushPlusNotificationPublishProvider.ProviderName,
    Priority = 100,
    Presence = NotificationChannelDataPresenceMode.AnyDeclaredProperty)]
public class PushPlusChannelData : NotificationChannelData
{
    /// <summary>
    /// webhook 地址
    /// </summary>
    [NotificationChannelProperty(PushPlusNotificationDataKeys.Webhook)]
    public virtual string? Webhook { get; set; }

    /// <summary>
    /// 回调地址
    /// </summary>
    [NotificationChannelProperty(PushPlusNotificationDataKeys.CallbackUrl)]
    public virtual string? CallbackUrl { get; set; }
}
