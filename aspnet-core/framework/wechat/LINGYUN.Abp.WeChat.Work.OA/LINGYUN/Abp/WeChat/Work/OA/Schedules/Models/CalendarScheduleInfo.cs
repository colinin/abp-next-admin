using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日历日程详情
/// </summary>
public class CalendarScheduleInfo : ScheduleInfo
{
    /// <summary>
    /// 日程编号，是一个自增数字
    /// </summary>
    [NotNull]
    [JsonProperty("sequence")]
    [JsonPropertyName("sequence")]
    public ulong Sequence { get; set; }
}
