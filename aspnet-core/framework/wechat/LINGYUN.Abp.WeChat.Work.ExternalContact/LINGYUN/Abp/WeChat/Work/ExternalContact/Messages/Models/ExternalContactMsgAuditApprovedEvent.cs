using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 客户同意进行聊天内容存档事件推送
/// </summary>
[EventName("external_contact_msg_audit_approved")]
public class ExternalContactMsgAuditApprovedEvent : ExternalContactChangeEvent
{
    /// <summary>
    /// 欢迎语code，可用于发送欢迎语
    /// </summary>
    [XmlElement("WelcomeCode")]
    public string WelcomeCode { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ExternalContactMsgAuditApprovedEvent>(this);
    }
}
