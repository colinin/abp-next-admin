using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Official.Messages;
public abstract class WeChatOfficialGeneralMessage : WeChatGeneralMessage
{
    /// <summary>
    /// 消息的数据ID（消息如果来自文章时才有）
    /// </summary>
    [XmlElement("MsgDataId")]
    public string MsgDataId { get; set; }
    /// <summary>
    /// 多图文时第几篇文章，从1开始（消息如果来自文章时才有）
    /// </summary>
    [XmlElement("Idx")]
    public string Idx { get; set; }
}
