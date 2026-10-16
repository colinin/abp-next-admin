using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Notifications;

public static partial class NotificationDataWeChatWorkExtensions
{
    private const string TextCardMessage = Prefix + "text_card_message";

    /// <summary>
    /// 卡片消息通知
    /// </summary>
    /// <param name="notificationData">通知内容</param>
    /// <param name="textCardMessage">卡片消息</param>
    public static void WithTextCardMessage(
        this NotificationData notificationData,
        TextCardMessage textCardMessage)
    {
        notificationData.TrySetData(TextCardMessage, JsonConvert.SerializeObject(textCardMessage));
    }

    /// <summary>
    /// 尝试获取卡片消息
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="textCardMessage"></param>
    /// <returns></returns>
    public static bool TryGetTextCardMessage(
        this NotificationData notificationData,
        out TextCardMessage? textCardMessage)
    {
        return notificationData.TryGetMessagePayload(TextCardMessage, out textCardMessage);
    }
}
