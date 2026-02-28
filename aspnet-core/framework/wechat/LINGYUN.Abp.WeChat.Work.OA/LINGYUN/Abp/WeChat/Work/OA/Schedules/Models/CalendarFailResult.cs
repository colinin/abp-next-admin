using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 无效的输入内容
/// </summary>
public class CalendarFailResult
{
    /// <summary>
    /// 无效的日历通知范围成员列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("shares")]
    [JsonPropertyName("shares")]
    public CalendarFailShare[] Shares { get; set; }
}
