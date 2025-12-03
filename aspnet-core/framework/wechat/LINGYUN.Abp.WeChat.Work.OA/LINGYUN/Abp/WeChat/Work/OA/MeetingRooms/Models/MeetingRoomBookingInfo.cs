using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
/// <summary>
/// 会议室预订信息
/// </summary>
public class MeetingRoomBookingInfo
{
    /// <summary>
    /// 会议室id
    /// </summary>
    [NotNull]
    [JsonProperty("meetingroom_id")]
    [JsonPropertyName("meetingroom_id")]
    public int MeetingRoomId { get; set; }
    /// <summary>
    /// 该会议室查询时间段内的预定情况
    /// </summary>
    [NotNull]
    [JsonProperty("schedule")]
    [JsonPropertyName("schedule")]
    public MeetingRoomSchedule[] Schedule { get; set; }
}
