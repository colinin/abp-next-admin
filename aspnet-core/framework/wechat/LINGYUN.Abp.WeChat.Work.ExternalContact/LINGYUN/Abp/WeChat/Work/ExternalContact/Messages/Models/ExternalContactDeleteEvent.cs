using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 删除企业客户事件
/// </summary>
[EventName("external_contact_delete")]
public class ExternalContactDeleteEvent : ExternalContactChangeEvent
{
    /// <summary>
    /// 删除客户的操作来源，DELETE_BY_TRANSFER表示此客户是因在职继承自动被转接成员删除
    /// </summary>
    [XmlElement("Source")]
    public string Source { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ExternalContactDeleteEvent>(this);
    }
}
