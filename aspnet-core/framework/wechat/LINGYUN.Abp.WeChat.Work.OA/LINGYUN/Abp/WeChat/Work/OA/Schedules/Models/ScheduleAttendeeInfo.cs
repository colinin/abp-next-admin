using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日程参与者详情
/// </summary>
public class ScheduleAttendeeInfo
{
    /// <summary>
    /// 日程参与者ID
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 日程参与者的接受状态
    /// </summary>
    [NotNull]
    [JsonProperty("response_status")]
    [JsonPropertyName("response_status")]
    public ScheduleAttendeeResponseStatus ResponseStatus { get; set; }
}
