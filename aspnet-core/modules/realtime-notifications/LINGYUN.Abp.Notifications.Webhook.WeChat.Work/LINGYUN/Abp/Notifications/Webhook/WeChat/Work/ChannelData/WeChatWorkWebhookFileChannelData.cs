using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 文件消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkWebhookNotificationProvider.ProviderName, Priority = 600)]
public class WeChatWorkWebhookFileChannelData : WeChatWorkWebhookChannelData
{
    public override WeChatWorkWebhookMessageKind Kind => WeChatWorkWebhookMessageKind.File;

    /// <summary>
    /// 文件消息体(素材 ID)
    /// </summary>
    [NotificationChannelProperty(WeChatWorkWebhookNotificationDataKeys.File)]
    public virtual WebhookFileMessage? File { get; set; }
}
