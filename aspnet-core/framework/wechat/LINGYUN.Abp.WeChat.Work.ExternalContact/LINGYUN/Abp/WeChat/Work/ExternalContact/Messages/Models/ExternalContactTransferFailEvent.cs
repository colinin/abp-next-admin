using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 客户接替失败事件
/// </summary>
[EventName("external_contact_transfer_fail")]
public class ExternalContactTransferFailEvent : ExternalContactChangeEvent
{
    /// <summary>
    /// 接替失败的原因, customer_refused-客户拒绝， customer_limit_exceed-接替成员的客户数达到上限
    /// </summary>
    [XmlElement("FailReason")]
    public string FailReason { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ExternalContactTransferFailEvent>(this);
    }
}
