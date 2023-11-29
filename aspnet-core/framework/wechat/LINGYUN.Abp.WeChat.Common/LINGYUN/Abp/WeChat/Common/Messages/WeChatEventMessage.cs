using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Common.Messages;
/// <summary>
/// 微信事件消息
/// </summary>
public abstract class WeChatEventMessage : WeChatMessage
{
    /// <summary>
    /// 事件类型
    /// </summary>
    [XmlElement("Event")]
    public string Event { get; set; }
}
