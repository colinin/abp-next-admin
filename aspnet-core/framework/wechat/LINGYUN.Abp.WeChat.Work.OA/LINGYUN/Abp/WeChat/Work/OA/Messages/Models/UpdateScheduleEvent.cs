using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.OA.Messages.Models;
/// <summary>
/// 修改日程事件
/// </summary>
/// <remarks>
/// 应用通过API创建日程后，当日程管理员修改了日程时，触发该事件。
/// </remarks>
[EventName("modify_schedule")]
public class UpdateScheduleEvent : WeChatWorkEventMessage
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
        return new WeChatWorkEventMessageEto<UpdateScheduleEvent>(this);
    }
}
