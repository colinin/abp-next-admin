using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Response;
/// <summary>
/// 预定会议室响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93620#%E9%A2%84%E5%AE%9A%E4%BC%9A%E8%AE%AE%E5%AE%A4"/>
/// </remarks>
public class WeChatWorkBookMeetingRoomResponse : WeChatWorkResponse
{
    /// <summary>
    /// 会议室的预定id
    /// </summary>
    [NotNull]
    [JsonProperty("booking_id")]
    [JsonPropertyName("booking_id")]
    public string BookingId { get; set; }
    /// <summary>
    /// 会议关联日程的id
    /// </summary>
    [NotNull]
    [JsonProperty("schedule_id")]
    [JsonPropertyName("schedule_id")]
    public string ScheduleId { get; set; }
}
