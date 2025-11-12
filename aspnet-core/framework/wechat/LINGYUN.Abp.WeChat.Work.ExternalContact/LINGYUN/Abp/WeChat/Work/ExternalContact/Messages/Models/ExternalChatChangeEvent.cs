using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 客户群变更事件推送
/// </summary>
public abstract class ExternalChatChangeEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 变更类型
    /// </summary>
    [XmlElement("ChangeType")]
    public string ChangeType { get; set; }
    /// <summary>
    /// 群ID
    /// </summary>
    [XmlElement("ChatId")]
    public string ChatId { get; set; }
}

