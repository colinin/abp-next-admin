using LINGYUN.Abp.WeChat.Common.Messages;
using System.Collections.Generic;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 模板卡片事件推送
/// </summary>
[EventName("template_card_event")]
public class TemplateCardPushEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 与发送模板卡片消息时指定的task_id相同
    /// </summary>
    [XmlElement("TaskId")]
    public string TaskId { get; set; }
    /// <summary>
    /// 通用模板卡片的类型，类型有
    /// "text_notice", 
    /// "news_notice",
    /// "button_interaction",
    /// "vote_interaction", 
    /// "multiple_interaction"
    /// 五种
    /// </summary>
    [XmlElement("CardType")]
    public string CardType { get; set; }
    /// <summary>
    /// 用于调用更新卡片接口的ResponseCode，72小时内有效，且只能使用一次
    /// </summary>
    [XmlElement("ResponseCode")]
    public string ResponseCode { get; set; }
    /// <summary>
    /// 与发送模板卡片消息时指定的按钮btn:key值相同
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("SelectedItems")]
    public TemplateCardQuestionInfo QuestionInfo { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<TemplateCardPushEvent>(this);
    }
}

public class TemplateCardQuestionInfo
{
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("SelectedItem")]
    public List<TemplateCardQuestion> Items { get; set; }
}

public class TemplateCardQuestion
{
    /// <summary>
    /// 问题的key值
    /// </summary>
    [XmlElement("QuestionKey")]
    public string QuestionKey { get; set; }
    /// <summary>
    /// 问题的key值
    /// </summary>
    [XmlArray("OptionIds")]
    public List<TemplateCardQuestionOption> OptionIds { get; set; }
}

public class TemplateCardQuestionOption
{
    /// <summary>
    /// 问题的key值
    /// </summary>
    [XmlElement("OpitonId")]
    public List<string> Items { get; set; }
}
