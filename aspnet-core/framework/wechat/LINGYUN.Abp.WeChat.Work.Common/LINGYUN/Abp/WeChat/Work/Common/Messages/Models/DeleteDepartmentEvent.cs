using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 删除部门事件
/// </summary>
[EventName("delete_party")]
public class DeleteDepartmentEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 部门Id
    /// </summary>
    [XmlElement("Id")]
    public string Id { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<DeleteDepartmentEvent>(this);
    }
}
