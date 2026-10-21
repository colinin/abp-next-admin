using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 模板卡片消息载荷
/// </summary>
/// <remarks>
/// 抽象类型 <see cref="WebhookTemplateCardMessage"/> 的两个具体实现
/// (<see cref="WebhookTextNoticeCardMessage"/> / <see cref="WebhookNewsNoticeCardMessage"/>)
/// 都需要 JSON 多态转换器才能在通知数据中往返, 由渠道模块注册。
/// </remarks>
[NotificationChannelData(WeChatWorkWebhookNotificationProvider.ProviderName, Priority = 800)]
public class WeChatWorkWebhookTemplateCardChannelData : WeChatWorkWebhookChannelData
{
    public override WeChatWorkWebhookMessageKind Kind => WeChatWorkWebhookMessageKind.TemplateCard;

    /// <summary>
    /// 模板卡片消息体
    /// </summary>
    [NotificationChannelProperty(WeChatWorkWebhookNotificationDataKeys.TemplateCard)]
    public virtual WebhookTemplateCardMessage? TemplateCard { get; set; }
}
