using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Authorize.Models;
/// <summary>
/// 企业微信用户信息
/// </summary>
public class WeChatWorkUserInfo
{
    /// <summary>
    /// 成员UserID
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 成员票据，最大为512字节，有效期为1800s
    /// </summary>
    [NotNull]
    [JsonProperty("user_ticket")]
    [JsonPropertyName("user_ticket")]
    public string UserTicket { get; set; }
}
