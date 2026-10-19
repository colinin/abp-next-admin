using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 视频消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkNotificationPublishProvider.ProviderName, Priority = 500)]
public class WeChatWorkVideoChannelData : WeChatWorkChannelData
{
    public override WeChatWorkMessageKind Kind => WeChatWorkMessageKind.Video;

    [NotificationChannelProperty(WeChatWorkNotificationDataKeys.VideoMessage)]
    public virtual VideoMessage? Video { get; set; }
}
