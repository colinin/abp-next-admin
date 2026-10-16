using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Authorize.Models;

public class WeChatWorkCode2Session
{
    /// <summary>
    /// 用户所属企业的corpid
    /// </summary>
    [NotNull]
    [JsonProperty("corpid")]
    [JsonPropertyName("corpid")]
    public string CorpId { get; set; } = default!;
    /// <summary>
    /// 用户在企业内的UserID，对应管理端的账号，企业内唯一。注意：第三方小程序此处返回加密的userid
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; } = default!;
    /// <summary>
    /// 会话密钥
    /// </summary>
    [NotNull]
    [JsonProperty("session_key")]
    [JsonPropertyName("session_key")]
    public string SessionKey { get; set; } = default!;
    [CanBeNull]
    [JsonProperty("deviceid")]
    [JsonPropertyName("deviceid")]
    public string? DeviceId { get; set; } = default!;
}
