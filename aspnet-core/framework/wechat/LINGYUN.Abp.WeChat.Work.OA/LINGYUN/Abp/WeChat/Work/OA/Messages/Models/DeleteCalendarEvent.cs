using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.OA.Messages.Models;
/// <summary>
/// 删除日历事件
/// </summary>
/// <remarks>
/// 当日历管理员删除了API创建的日历时，触发该事件。
/// </remarks>
[EventName("delete_calendar")]
public class DeleteCalendarEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 日历ID
    /// </summary>
    [XmlElement("CalId")]
    public string CalId { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<DeleteCalendarEvent>(this);
    }
}
