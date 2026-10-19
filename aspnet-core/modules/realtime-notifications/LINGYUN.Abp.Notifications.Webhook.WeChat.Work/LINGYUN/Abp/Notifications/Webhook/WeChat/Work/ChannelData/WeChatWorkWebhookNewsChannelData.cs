using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 图文消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkWebhookNotificationProvider.ProviderName, Priority = 500)]
public class WeChatWorkWebhookNewsChannelData : WeChatWorkWebhookChannelData
{
    public override WeChatWorkWebhookMessageKind Kind => WeChatWorkWebhookMessageKind.News;

    /// <summary>
    /// 图文消息体
    /// </summary>
    [NotificationChannelProperty(WeChatWorkWebhookNotificationDataKeys.News)]
    public virtual WebhookNewsMessage? News { get; set; }
}
