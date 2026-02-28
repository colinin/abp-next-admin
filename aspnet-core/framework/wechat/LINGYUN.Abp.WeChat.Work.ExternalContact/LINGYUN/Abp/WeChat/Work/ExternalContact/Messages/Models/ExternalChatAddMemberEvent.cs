using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 客户群成员入群事件推送
/// </summary>
[EventName("external_chat_add_member")]
public class ExternalChatAddMemberEvent : ExternalChatChangeMemberEvent
{
    /// <summary>
    /// 当是成员入群时有值。表示成员的入群方式<br />
    /// 0 - 由成员邀请入群（包括直接邀请入群和通过邀请链接入群）<br />
    /// 3 - 通过扫描群二维码入群<br />
    /// </summary>
    [XmlElement("JoinScene")]
    public ExternalChatMemberJoinScene JoinScene { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ExternalChatAddMemberEvent>(this);
    }
}
