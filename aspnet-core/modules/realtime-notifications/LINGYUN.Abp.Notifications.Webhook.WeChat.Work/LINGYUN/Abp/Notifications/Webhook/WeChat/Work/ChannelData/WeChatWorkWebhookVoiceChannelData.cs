using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 语音消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkWebhookNotificationProvider.ProviderName, Priority = 700)]
public class WeChatWorkWebhookVoiceChannelData : WeChatWorkWebhookChannelData
{
    public override WeChatWorkWebhookMessageKind Kind => WeChatWorkWebhookMessageKind.Voice;

    /// <summary>
    /// 语音消息体(素材 ID)
    /// </summary>
    [NotificationChannelProperty(WeChatWorkWebhookNotificationDataKeys.Voice)]
    public virtual WebhookVoiceMessage? Voice { get; set; }
}
