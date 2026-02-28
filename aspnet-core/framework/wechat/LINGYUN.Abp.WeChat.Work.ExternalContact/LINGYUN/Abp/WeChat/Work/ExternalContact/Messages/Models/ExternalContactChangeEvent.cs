using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 企业客户变更事件推送
/// </summary>
public abstract class ExternalContactChangeEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 变更类型
    /// </summary>
    [XmlElement("ChangeType")]
    public string ChangeType { get; set; }
    /// <summary>
    /// 企业服务人员的UserID
    /// </summary>
    [XmlElement("UserID")]
    public string UserId { get; set; }
    /// <summary>
    /// 外部联系人的userid，注意不是企业成员的账号
    /// </summary>
    [XmlElement("ExternalUserID")]
    public string ExternalUserId { get; set; }
}
