using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 重复日程不包含的日期
/// </summary>
public class ScheduleReminderExcludeTime
{
    /// <summary>
    /// 不包含的日期时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("start_time")]
    [JsonPropertyName("start_time")]
    public long StartTime { get; set; }
}
