using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 无效的日历通知范围成员
/// </summary>
public class CalendarFailShare
{
    /// <summary>
    /// 错误码
    /// </summary>
    [NotNull]
    [JsonProperty("errcode")]
    [JsonPropertyName("errcode")]
    public int ErrorCode { get; set; }
    /// <summary>
    /// 错误消息
    /// </summary>
    [NotNull]
    [JsonProperty("errmsg")]
    [JsonPropertyName("errmsg")]
    public string ErrorMessage { get; set; }
    /// <summary>
    /// 日历通知范围成员的id
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
}
