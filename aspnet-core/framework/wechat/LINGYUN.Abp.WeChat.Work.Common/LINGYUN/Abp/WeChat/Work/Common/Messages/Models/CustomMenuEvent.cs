using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 自定义菜单事件
/// </summary>
[EventName("custom_menu")]
public class CustomMenuEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<CustomMenuEvent>(this);
    }
}
