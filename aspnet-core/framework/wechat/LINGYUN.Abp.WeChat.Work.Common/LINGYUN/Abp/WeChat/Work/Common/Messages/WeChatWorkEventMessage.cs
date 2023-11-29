using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;
/// <summary>
/// 企业微信事件消息
/// </summary>
public abstract class WeChatWorkEventMessage : WeChatEventMessage
{
    /// <summary>
    /// 企业应用的id，整型。可在应用的设置页面查看
    /// </summary>
    [XmlElement("AgentID")]
    public int AgentId { get; set; }
}
