using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Notifications;

public static partial class NotificationDataWeChatWorkExtensions
{
    private const string VoiceMessage = Prefix + "voice_message";

    /// <summary>
    /// 语言消息通知
    /// </summary>
    /// <param name="notificationData">通知内容</param>
    /// <param name="voiceMessage">语言消息</param>
    public static void WithVoiceMessage(
        this NotificationData notificationData,
        MediaMessage voiceMessage)
    {
        notificationData.TrySetData(VoiceMessage, JsonConvert.SerializeObject(voiceMessage));
    }

    /// <summary>
    /// 尝试获取语言消息
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="voiceMessage"></param>
    /// <returns></returns>
    public static bool TryGetVoiceMessage(
        this NotificationData notificationData,
        out MediaMessage? voiceMessage)
    {
        return notificationData.TryGetMessagePayload(VoiceMessage, out voiceMessage);
    }
}
