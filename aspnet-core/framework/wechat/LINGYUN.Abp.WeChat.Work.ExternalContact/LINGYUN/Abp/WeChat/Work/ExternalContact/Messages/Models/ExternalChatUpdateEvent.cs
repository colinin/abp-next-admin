using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 客户群变更事件推送
/// </summary>
public abstract class ExternalChatUpdateEvent : ExternalChatChangeEvent
{
    /// <summary>
    /// 变更详情。目前有以下几种：<br />
    /// add_member : 成员入群<br />
    /// del_member : 成员退群<br />
    /// change_owner : 群主变更<br />
    /// change_name : 群名变更<br />
    /// change_notice : 群公告变更
    /// </summary>
    [XmlElement("UpdateDetail")]
    public string UpdateDetail { get; set; }
}
