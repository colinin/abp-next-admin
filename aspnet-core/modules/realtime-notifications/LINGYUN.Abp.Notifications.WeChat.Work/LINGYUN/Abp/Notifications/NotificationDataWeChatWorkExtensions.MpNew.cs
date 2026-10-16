using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Notifications;
public static partial class NotificationDataWeChatWorkExtensions
{
    private const string MpNewMessage = Prefix + "mp_new_message";

    /// <summary>
    /// 图文消息
    /// </summary>
    /// <param name="notificationData">通知内容</param>
    /// <param name="mpNewMessagePayload">图文消息（mp）载体</param>
    public static void WithMpNewMessage(
        this NotificationData notificationData,
        MpNewMessagePayload mpNewMessagePayload)
    {
        notificationData.TrySetData(MpNewMessage, JsonConvert.SerializeObject(mpNewMessagePayload));
    }

    /// <summary>
    /// 尝试图文消息
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="workMiniProgramMessage"></param>
    /// <returns></returns>
    public static bool TryGetMpNewMessage(
        this NotificationData notificationData,
        out MpNewMessagePayload? mpNewMessagePayload)
    {
        return notificationData.TryGetMessagePayload(MpNewMessage, out mpNewMessagePayload);
    }
}
