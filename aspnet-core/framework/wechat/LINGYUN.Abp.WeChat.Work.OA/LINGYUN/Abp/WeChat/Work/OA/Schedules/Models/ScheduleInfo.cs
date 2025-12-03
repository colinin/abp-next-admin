using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日程详情
/// </summary>
public class ScheduleInfo
{
    /// <summary>
    /// 日程ID
    /// </summary>
    [NotNull]
    [JsonProperty("schedule_id")]
    [JsonPropertyName("schedule_id")]
    public string ScheduleId { get; set; }
    /// <summary>
    /// 管理员userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("admins")]
    [JsonPropertyName("admins")]
    public string[] Admins { get; set; }
    /// <summary>
    /// 日程参与者列表
    /// </summary>
    [NotNull]
    [JsonProperty("attendees")]
    [JsonPropertyName("attendees")]
    public ScheduleAttendeeInfo[] Attendees { get; set; }
    /// <summary>
    /// 日程标题
    /// </summary>
    [NotNull]
    [JsonProperty("summary")]
    [JsonPropertyName("summary")]
    public string Summary { get; set; }
    /// <summary>
    /// 日程描述
    /// </summary>
    [CanBeNull]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    /// <summary>
    /// 提醒相关信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("reminders")]
    [JsonPropertyName("reminders")]
    public ScheduleReminder? Reminders { get; set; }
    /// <summary>
    /// 日程地址
    /// </summary>
    /// <remarks>
    /// 不多于128个字符
    /// </remarks>
    [CanBeNull]
    [JsonProperty("location")]
    [JsonPropertyName("location")]
    public string? Location { get; set; }
    /// <summary>
    /// 日程状态
    /// </summary>
    [NotNull]
    [JsonProperty("status")]
    [JsonPropertyName("status")]
    public ScheduleStatus Status { get; set; }
    /// <summary>
    /// 日程开始时间，Unix时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("start_time")]
    [JsonPropertyName("start_time")]
    public long StartTime { get; set; }
    /// <summary>
    /// 日程结束时间，Unix时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("end_time")]
    [JsonPropertyName("end_time")]
    public long EndTime { get; set; }
    /// <summary>
    /// 是否全天日程
    /// </summary>
    [NotNull]
    [JsonProperty("is_whole_day")]
    [JsonPropertyName("is_whole_day")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool IsWholeDay { get; set; }
    /// <summary>
    /// 日程所属日历ID
    /// </summary>
    /// <remarks>
    /// 不多于64字节
    /// </remarks>
    [CanBeNull]
    [JsonProperty("cal_id")]
    [JsonPropertyName("cal_id")]
    public string? CalId { get; set; }
}
