using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Notifications;

public static partial class NotificationDataWeChatWorkExtensions
{
    private const string VideoMessage = Prefix + "video_message";

    /// <summary>
    /// 视频消息通知
    /// </summary>
    /// <param name="notificationData">通知内容</param>
    /// <param name="videoMessage">视频消息</param>
    public static void WithVideoMessage(
        this NotificationData notificationData,
        VideoMessage videoMessage)
    {
        notificationData.TrySetData(VideoMessage, JsonConvert.SerializeObject(videoMessage));
    }

    /// <summary>
    /// 尝试获取视频消息
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="videoMessage"></param>
    /// <returns></returns>
    public static bool TryGetVideoMessage(
        this NotificationData notificationData,
        out VideoMessage? videoMessage)
    {
        return notificationData.TryGetMessagePayload(VideoMessage, out videoMessage);
    }
}
