using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 添加企业客户事件推送
/// </summary>
[EventName("external_contact_create")]
public class ExternalContactCreateEvent : ExternalContactChangeEvent
{
    /// <summary>
    /// 添加此用户的「联系我」方式配置的state参数，或在获客链接中指定的customer_channel参数，可用于识别添加此用户的渠道
    /// </summary>
    [XmlElement("State")]
    public string State { get; set; }
    /// <summary>
    /// 欢迎语code，可用于发送欢迎语
    /// </summary>
    [XmlElement("WelcomeCode")]
    public string WelcomeCode { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ExternalContactCreateEvent>(this);
    }
}
