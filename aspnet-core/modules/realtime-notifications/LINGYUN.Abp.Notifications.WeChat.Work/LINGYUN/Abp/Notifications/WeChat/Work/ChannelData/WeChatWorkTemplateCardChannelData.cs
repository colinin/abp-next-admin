using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 模板卡片消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkNotificationPublishProvider.ProviderName, Priority = 300)]
public class WeChatWorkTemplateCardChannelData : WeChatWorkChannelData
{
    public override WeChatWorkMessageKind Kind => WeChatWorkMessageKind.TemplateCard;

    [NotificationChannelProperty(WeChatWorkNotificationDataKeys.TemplateCardMessage)]
    public virtual TemplateCard? TemplateCard { get; set; }
}
