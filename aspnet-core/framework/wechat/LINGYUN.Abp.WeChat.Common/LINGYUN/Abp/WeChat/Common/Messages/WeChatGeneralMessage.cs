using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Common.Messages;
/// <summary>
/// 微信普通消息
/// </summary>
public abstract class WeChatGeneralMessage : WeChatMessage
{
    /// <summary>
    /// 消息id，64位整型
    /// </summary>
    [XmlElement("MsgId")]
    public long MsgId { get; set; }
}
