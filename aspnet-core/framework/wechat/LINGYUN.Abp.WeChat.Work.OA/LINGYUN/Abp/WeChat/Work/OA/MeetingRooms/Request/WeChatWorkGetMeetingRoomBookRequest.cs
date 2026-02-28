using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
/// <summary>
/// 根据会议室预定ID查询预定详情请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93620#%E6%A0%B9%E6%8D%AE%E4%BC%9A%E8%AE%AE%E5%AE%A4%E9%A2%84%E5%AE%9Aid%E6%9F%A5%E8%AF%A2%E9%A2%84%E5%AE%9A%E8%AF%A6%E6%83%85"/>
/// </remarks>
public class WeChatWorkGetMeetingRoomBookRequest : WeChatWorkRequest
{
    /// <summary>
    /// 会议室id
    /// </summary>
    [NotNull]
    [JsonProperty("meetingroom_id")]
    [JsonPropertyName("meetingroom_id")]
    public int MeetingRoomId { get; }
    /// <summary>
    /// 会议室的预定id
    /// </summary>
    [NotNull]
    [JsonProperty("booking_id")]
    [JsonPropertyName("booking_id")]
    public string BookingId { get; }
    public WeChatWorkGetMeetingRoomBookRequest(int meetingRoomId, string bookingId)
    {
        MeetingRoomId = meetingRoomId;
        BookingId = Check.NotNullOrWhiteSpace(bookingId, nameof(bookingId));
    }
}
