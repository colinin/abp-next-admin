using LINGYUN.Abp.Notifications;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 文本消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkWebhookNotificationProvider.ProviderName, Priority = 100)]
public class WeChatWorkWebhookTextChannelData : WeChatWorkWebhookChannelData
{
    public override WeChatWorkWebhookMessageKind Kind => WeChatWorkWebhookMessageKind.Text;

    /// <summary>
    /// 文本内容
    /// </summary>
    [NotificationChannelProperty(WeChatWorkWebhookNotificationDataKeys.Text)]
    public virtual string? Content { get; set; }
}
