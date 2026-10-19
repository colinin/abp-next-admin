using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.WeChat.Work.Messages.Models;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 语音消息载荷
/// </summary>
[NotificationChannelData(WeChatWorkNotificationPublishProvider.ProviderName, Priority = 600)]
public class WeChatWorkVoiceChannelData : WeChatWorkChannelData
{
    public override WeChatWorkMessageKind Kind => WeChatWorkMessageKind.Voice;

    [NotificationChannelProperty(WeChatWorkNotificationDataKeys.VoiceMessage)]
    public virtual MediaMessage? Voice { get; set; }
}
