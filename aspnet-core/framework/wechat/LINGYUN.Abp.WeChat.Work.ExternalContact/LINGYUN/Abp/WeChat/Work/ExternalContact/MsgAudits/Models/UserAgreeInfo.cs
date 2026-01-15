using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Models;
/// <summary>
/// 单聊同意情况
/// </summary>
public class UserAgreeInfo
{
    /// <summary>
    /// 内部成员的userid
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 外部成员的exteranalopenid
    /// </summary>
    [NotNull]
    [JsonProperty("exteranalopenid")]
    [JsonPropertyName("exteranalopenid")]
    public string ExteranalOpenId { get; set; }
    /// <summary>
    /// 同意:"Agree"，不同意:"Disagree"
    /// </summary>
    [NotNull]
    [JsonProperty("agree_status")]
    [JsonPropertyName("agree_status")]
    public string AgreeStatus { get; set; }
    /// <summary>
    /// 同意状态改变的具体时间，utc时间
    /// </summary>
    [CanBeNull]
    [JsonProperty("status_change_time")]
    [JsonPropertyName("status_change_time")]
    public long? StatusChangeTime { get; set; }
}
