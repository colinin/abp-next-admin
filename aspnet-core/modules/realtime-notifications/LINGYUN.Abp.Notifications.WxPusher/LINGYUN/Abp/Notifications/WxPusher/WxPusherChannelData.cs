using LINGYUN.Abp.Notifications;

namespace LINGYUN.Abp.Notifications.WxPusher;

/// <summary>
/// WxPusher 渠道数据(载荷)
/// </summary>
/// <remarks>
/// 存在性由 url 键推导(默认 Presence = DeclaredProperties), 无需实现判定方法。
/// </remarks>
[NotificationChannelData(WxPusherNotificationPublishProvider.ProviderName, Priority = 100)]
public class WxPusherChannelData : NotificationChannelData
{
    /// <summary>
    /// 消息点击后的跳转地址
    /// </summary>
    [NotificationChannelProperty(WxPusherNotificationDataKeys.Url)]
    public virtual string? Url { get; set; }
}
