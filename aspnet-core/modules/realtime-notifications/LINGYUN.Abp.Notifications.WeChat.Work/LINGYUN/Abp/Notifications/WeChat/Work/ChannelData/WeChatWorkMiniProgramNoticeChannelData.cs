using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 小程序通知消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkNotificationPublishProvider.ProviderName, Priority = 100)]
public class WeChatWorkMiniProgramNoticeChannelData : WeChatWorkChannelData
{
    public override WeChatWorkMessageKind Kind => WeChatWorkMessageKind.MiniProgramNotice;

    [NotificationChannelProperty(WeChatWorkNotificationDataKeys.MiniProgramMessage)]
    public virtual MiniProgramMessage? MiniProgram { get; set; }
}
