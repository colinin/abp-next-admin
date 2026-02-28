using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日程参与者
/// </summary>
public class ScheduleAttendee
{
    /// <summary>
    /// 日程参与者ID
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; }
    public ScheduleAttendee(string userId)
    {
        UserId = Check.NotNullOrWhiteSpace(userId, nameof(userId), 64); 
    }
}
