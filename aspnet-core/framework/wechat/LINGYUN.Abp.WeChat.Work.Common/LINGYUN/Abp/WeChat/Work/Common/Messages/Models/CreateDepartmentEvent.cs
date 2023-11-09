using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 新增部门事件
/// </summary>
[EventName("create_party")]
public class CreateDepartmentEvent : DepartmentUpdateEvent
{
    /// <summary>
    /// 部门排序
    /// </summary>
    [XmlElement("Order", IsNullable = true)]
    public int? Order { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<CreateDepartmentEvent>(this);
    }
}
