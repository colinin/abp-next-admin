using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 点击菜单跳转链接时的事件推送
/// </summary>
[EventName("menu_click_jump_link")]
public class MenuClickJumpLinkPushEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<MenuClickJumpLinkPushEvent>(this);
    }
}
