using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.OA.Messages.Models;
/// <summary>
/// 日程回执事件
/// </summary>
/// <remarks>
/// 当应用创建的日程，参与人进行回执操作（接受、待定、拒绝）时，触发该事件。
/// </remarks>
[EventName("respond_schedule")]
public class RespondScheduleEvent : WeChatWorkEventMessage
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
        return new WeChatWorkEventMessageEto<RespondScheduleEvent>(this);
    }
}