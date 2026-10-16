using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Notifications;

public static partial class NotificationDataWeChatWorkExtensions
{
    private const string TemplateCardMessage = Prefix + "template_card_message";

    /// <summary>
    /// 模板卡片消息通知
    /// </summary>
    /// <param name="notificationData">通知内容</param>
    /// <param name="templateCard">模板卡片消息</param>
    public static void WithTemplateCardMessage(
        this NotificationData notificationData,
        TemplateCard templateCard)
    {
        notificationData.TrySetData(TemplateCardMessage, JsonConvert.SerializeObject(templateCard));
    }

    /// <summary>
    /// 尝试获取模板卡片消息
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="textTemplateCard"></param>
    /// <returns></returns>
    public static bool TryGetTextTemplateCardMessage(
        this NotificationData notificationData,
        out TextTemplateCard? textTemplateCard)
    {
        return notificationData.TryGetMessagePayload(TemplateCardMessage, out textTemplateCard);
    }
}
