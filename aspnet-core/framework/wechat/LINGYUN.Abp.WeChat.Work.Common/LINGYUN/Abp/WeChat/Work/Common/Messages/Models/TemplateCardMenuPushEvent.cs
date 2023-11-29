using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 通用模板卡片右上角菜单事件推送
/// </summary>
[EventName("template_card_menu_event")]
public class TemplateCardMenuPushEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 与发送模板卡片消息时指定的task_id相同
    /// </summary>
    [XmlElement("TaskId")]
    public string TaskId { get; set; }
    /// <summary>
    /// 通用模板卡片的类型，
    /// 类型有
    /// "text_notice", 
    /// "news_notice", 
    /// "button_interaction"
    /// 三种
    /// </summary>
    [XmlElement("CardType")]
    public string CardType { get; set; }
    /// <summary>
    /// 用于调用更新卡片接口的ResponseCode
    /// </summary>
    [XmlElement("ResponseCode")]
    public string ResponseCode { get; set; }
    /// <summary>
    /// 与发送模板卡片右上角菜单的按钮key值相同
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<TemplateCardMenuPushEvent>(this);
    }
}
