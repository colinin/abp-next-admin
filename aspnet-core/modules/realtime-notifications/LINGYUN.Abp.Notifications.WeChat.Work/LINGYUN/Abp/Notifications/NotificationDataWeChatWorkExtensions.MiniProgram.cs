using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Notifications;

public static partial class NotificationDataWeChatWorkExtensions
{
    private const string MiniProgramMessage = Prefix + "mini_program_message";

    /// <summary>
    /// 小程序通知
    /// </summary>
    /// <param name="notificationData">通知内容</param>
    /// <param name="miniProgramMessage">小程序通知消息</param>
    public static void WithMiniProgramMessage(
        this NotificationData notificationData,
        MiniProgramMessage miniProgramMessage)
    {
        notificationData.TrySetData(MiniProgramMessage, JsonConvert.SerializeObject(miniProgramMessage));
    }

    /// <summary>
    /// 尝试获取小程序消息
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="workMiniProgramMessage"></param>
    /// <returns></returns>
    public static bool TryGetMiniProgramMessage(
        this NotificationData notificationData,
        out MiniProgramMessage? miniProgramMessage)
    {
        return notificationData.TryGetMessagePayload(MiniProgramMessage, out miniProgramMessage);
    }
}
