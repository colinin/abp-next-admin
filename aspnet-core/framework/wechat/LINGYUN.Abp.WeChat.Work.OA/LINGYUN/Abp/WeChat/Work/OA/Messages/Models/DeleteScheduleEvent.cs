using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.OA.Messages.Models;
/// <summary>
/// 删除日程事件
/// </summary>
/// <remarks>
/// 当日程管理员在API创建的日历上删除了日程后，触发该事件。
/// </remarks>
[EventName("delete_schedule")]
public class DeleteScheduleEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 日历ID
    /// </summary>
    [XmlElement("CalId")]
    public string? CalId { get; set; }
    /// <summary>
    /// 日程ID
    /// </summary>
    [XmlElement("ScheduleId")]
    public string ScheduleId { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<DeleteScheduleEvent>(this);
    }
}