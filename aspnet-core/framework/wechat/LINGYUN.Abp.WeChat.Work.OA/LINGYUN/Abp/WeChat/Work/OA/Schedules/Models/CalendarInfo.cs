using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日历详情
/// </summary>
public class CalendarInfo
{
    /// <summary>
    /// 日历ID
    /// </summary>
    [NotNull]
    [JsonProperty("cal_id")]
    [JsonPropertyName("cal_id")]
    public string CalId { get; set; }
    /// <summary>
    /// 日历的管理员userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("admins")]
    [JsonPropertyName("admins")]
    public string[] Admins { get; set; }
    /// <summary>
    /// 日历标题。1 ~ 128 字符
    /// </summary>
    [NotNull]
    [JsonProperty("summary")]
    [JsonPropertyName("summary")]
    public string Summary { get; set; }
    /// <summary>
    /// 日历颜色，RGB颜色编码16进制表示，例如："#0000FF" 表示纯蓝色
    /// </summary>
    [NotNull]
    [JsonProperty("color")]
    [JsonPropertyName("color")]
    public string Color { get; set; }
    /// <summary>
    /// 日历描述。0 ~ 512 字符
    /// </summary>
    [CanBeNull]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    /// <summary>
    /// 是否公共日历。
    /// </summary>
    [NotNull]
    [JsonProperty("is_public")]
    [JsonPropertyName("is_public")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool IsPublic { get; set; }
    /// <summary>
    /// 公开范围。仅当是公共日历时有效
    /// </summary>
    [CanBeNull]
    [JsonProperty("public_range")]
    [JsonPropertyName("public_range")]
    public CalendarPublicRange? PublicRange { get; set; }
    /// <summary>
    /// 是否全员日历。
    /// </summary>
    [NotNull]
    [JsonProperty("is_corp_calendar")]
    [JsonPropertyName("is_corp_calendar")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool IsCorpCalendar { get; set; }
    /// <summary>
    /// 日历通知范围成员列表。最多2000人
    /// </summary>
    [NotNull]
    [JsonProperty("shares")]
    [JsonPropertyName("shares")]
    public CalendarShare[] Shares { get; set; }
}
