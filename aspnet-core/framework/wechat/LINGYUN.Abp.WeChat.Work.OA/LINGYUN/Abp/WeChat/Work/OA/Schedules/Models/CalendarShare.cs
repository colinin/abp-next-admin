using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日历通知范围成员
/// </summary>
public class CalendarShare
{
    /// <summary>
    /// 日历通知范围成员的id
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 日历通知范围成员权限
    /// </summary>
    [NotNull]
    [JsonProperty("permission")]
    [JsonPropertyName("permission")]
    public CalendarSharePermission Permission { get; set; }
}
