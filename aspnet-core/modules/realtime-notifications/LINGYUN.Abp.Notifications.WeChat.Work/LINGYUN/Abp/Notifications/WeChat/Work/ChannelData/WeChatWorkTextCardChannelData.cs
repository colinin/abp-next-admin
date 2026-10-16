using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 文本卡片消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkNotificationPublishProvider.ProviderName, Priority = 400)]
public class WeChatWorkTextCardChannelData : WeChatWorkChannelData
{
    public override WeChatWorkMessageKind Kind => WeChatWorkMessageKind.TextCard;

    [NotificationChannelProperty(WeChatWorkNotificationDataKeys.TextCardMessage)]
    public virtual TextCardMessage? TextCard { get; set; }
}
