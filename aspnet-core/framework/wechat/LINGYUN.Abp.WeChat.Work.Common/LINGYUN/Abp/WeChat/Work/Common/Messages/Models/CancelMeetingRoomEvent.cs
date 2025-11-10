using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 会议室取消事件
/// </summary>
/// <remarks>
/// 当用户在企业微信取消预定会议室时，会触发该事件回调给会议室系统应用；如果该会议室由自建应用预定，除了会议室系统应用外，也会回调给对应的自建应用。
/// </remarks>
[EventName("cancel_meeting_room")]
public class CancelMeetingRoomEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 会议室id
    /// </summary>
    [XmlElement("MeetingRoomId")]
    public int MeetingRoomId { get; set; }
    /// <summary>
    /// 预定id，可根据该ID查询具体的会议预定情况
    /// </summary>
    [XmlElement("BookingId")]
    public string BookingId { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<CancelMeetingRoomEvent>(this);
    }
}
