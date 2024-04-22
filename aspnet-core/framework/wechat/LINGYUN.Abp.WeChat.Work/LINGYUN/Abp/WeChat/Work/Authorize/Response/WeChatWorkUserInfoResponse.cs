using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Authorize.Response;
/// <summary>
/// 企业微信用户信息响应
/// </summary>
public class WeChatWorkUserInfoResponse : WeChatWorkResponse
{
    /// <summary>
    /// 成员UserID
    /// </summary>
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 成员票据，最大为512字节，有效期为1800s
    /// </summary>
    [JsonProperty("user_ticket")]
    [JsonPropertyName("user_ticket")]
    public string UserTicket { get; set; }
}
