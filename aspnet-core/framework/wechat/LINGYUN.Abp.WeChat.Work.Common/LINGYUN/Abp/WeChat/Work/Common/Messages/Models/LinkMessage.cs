using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 链接消息
/// </summary>
[EventName("link")]
public class LinkMessage : WeChatWorkGeneralMessage
{
    /// <summary>
    /// 消息标题
    /// </summary>
    [XmlElement("Title")]
    public string Title { get; set; }
    /// <summary>
    /// 消息描述
    /// </summary>
    [XmlElement("Description")]
    public string Description { get; set; }
    /// <summary>
    /// 消息链接
    /// </summary>
    [XmlElement("Url")]
    public string Url { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkGeneralMessageEto<LinkMessage>(this);
    }
}
