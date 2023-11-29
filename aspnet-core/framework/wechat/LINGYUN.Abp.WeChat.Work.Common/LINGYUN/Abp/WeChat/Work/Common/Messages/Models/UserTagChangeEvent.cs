using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 标签成员变更事件
/// </summary>
[EventName("update_tag")]
public class UserTagChangeEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 标签Id
    /// </summary>
    [XmlElement("TagId")]
    public string TagId { get; set; }
    /// <summary>
    /// 标签中新增的成员userid列表，用逗号分隔
    /// </summary>
    [XmlElement("AddUserItems", IsNullable = true)]
    public string AddUserItems { get; set; }
    /// <summary>
    /// 标签中删除的成员userid列表，用逗号分隔
    /// </summary>
    [XmlElement("DelUserItems", IsNullable = true)]
    public string DelUserItems { get; set; }
    /// <summary>
    /// 标签中新增的部门id列表，用逗号分隔
    /// </summary>
    [XmlElement("AddPartyItems", IsNullable = true)]
    public string AddPartyItems { get; set; }
    /// <summary>
    /// 标签中删除的部门id列表，用逗号分隔
    /// </summary>
    [XmlElement("DelPartyItems", IsNullable = true)]
    public string DelPartyItems { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<UserTagChangeEvent>(this);
    }
}
