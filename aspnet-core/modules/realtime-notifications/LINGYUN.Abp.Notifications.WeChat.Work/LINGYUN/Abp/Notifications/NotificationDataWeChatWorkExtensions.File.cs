using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Notifications;

public static partial class NotificationDataWeChatWorkExtensions
{
    private const string FileMessage = Prefix + "file_message";

    /// <summary>
    /// 文件消息通知
    /// </summary>
    /// <param name="notificationData">通知内容</param>
    /// <param name="fileMessage">文件消息</param>
    public static void WithFileMessage(
        this NotificationData notificationData,
        MediaMessage fileMessage)
    {
        notificationData.TrySetData(FileMessage, JsonConvert.SerializeObject(fileMessage));
    }

    /// <summary>
    /// 尝试获取文件消息
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="fileMessage"></param>
    /// <returns></returns>
    public static bool TryGetFileMessage(
        this NotificationData notificationData,
        out MediaMessage? fileMessage)
    {
        return notificationData.TryGetMessagePayload(FileMessage, out fileMessage);
    }
}
