using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
/// <summary>
/// 会议室预定情况
/// </summary>
public class MeetingRoomSchedule
{
    /// <summary>
    /// 开始时间的时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("start_time")]
    [JsonPropertyName("start_time")]
    public long StartTime { get; set; }
    /// <summary>
    /// 结束时间的时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("end_time")]
    [JsonPropertyName("end_time")]
    public long EndTime { get; set; }
    /// <summary>
    /// 预定人的userid
    /// </summary>
    [NotNull]
    [JsonProperty("booker")]
    [JsonPropertyName("booker")]
    public string Booker { get; set; }
    /// <summary>
    /// 会议室的预定状态
    /// </summary>
    [NotNull]
    [JsonProperty("status")]
    [JsonPropertyName("status")]
    public MeetingRoomScheduleStatus Status { get; set; }
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
    [CanBeNull]
    [JsonProperty("schedule_id")]
    [JsonPropertyName("schedule_id")]
    public string? ScheduleId { get; set; }
}
