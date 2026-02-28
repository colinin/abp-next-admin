using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Response;
/// <summary>
/// 通过日程预定会议室响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93620#%E9%A2%84%E5%AE%9A%E4%BC%9A%E8%AE%AE%E5%AE%A4"/>
/// </remarks>
public class WeChatWorkBookMeetingRoomByScheduleResponse : WeChatWorkResponse
{
    /// <summary>
    /// 会议室的预定id
    /// </summary>
    [NotNull]
    [JsonProperty("booking_id")]
    [JsonPropertyName("booking_id")]
    public string BookingId { get; set; }
    /// <summary>
    /// 会议室冲突日期列表，为当天0点的时间戳；使用重复日程预定会议室，部分日期与会议室预定情况冲突时返回
    /// </summary>
    [NotNull]
    [JsonProperty("conflict_date")]
    [JsonPropertyName("conflict_date")]
    public long[] ConflictDate { get; set; }
}
