using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.OA.Messages.Models;
/// <summary>
/// 会议室预定事件
/// </summary>
/// <remarks>
/// 当用户在企业微信预定会议室时，会触发该事件回调给会议室系统应用。
/// </remarks>
[EventName("book_meeting_room")]
public class BookMeetingRoomEvent : WeChatWorkEventMessage
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
        return new WeChatWorkEventMessageEto<BookMeetingRoomEvent>(this);
    }
}
