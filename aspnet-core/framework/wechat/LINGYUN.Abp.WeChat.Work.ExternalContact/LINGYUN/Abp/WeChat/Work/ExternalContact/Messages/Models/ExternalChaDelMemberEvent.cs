using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 客户群成员退群事件推送
/// </summary>
[EventName("external_chat_del_member")]
public class ExternalChaDelMemberEvent : ExternalChatChangeMemberEvent
{
    /// <summary>
    /// 当是成员退群时有值。表示成员的退群方式<br />
    /// 0 - 自己退群<br />
    /// 1 - 群主/群管理员移出<br />
    /// </summary>
    [XmlElement("QuitScene")]
    public ExternalChatMemberQuitScene QuitScene { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ExternalChaDelMemberEvent>(this);
    }
}
