using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 文件消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkNotificationPublishProvider.ProviderName, Priority = 700)]
public class WeChatWorkFileChannelData : WeChatWorkChannelData
{
    public override WeChatWorkMessageKind Kind => WeChatWorkMessageKind.File;

    [NotificationChannelProperty(WeChatWorkNotificationDataKeys.FileMessage)]
    public virtual MediaMessage? File { get; set; }
}
