using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Request;
/// <summary>
/// 撤回消息请求载体
/// </summary>
public class WeChatWorkMessageReCallRequest : WeChatWorkRequest
{
    /// <summary>
    /// 调用接口凭证
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string AccessToken { get; set; }
    /// <summary>
    /// 消息ID
    /// </summary>
    [JsonProperty("msgid")]
    [JsonPropertyName("msgid")]
    public string MessageId { get; set; }
    public WeChatWorkMessageReCallRequest(string accessToken, string messageId)
    {
        AccessToken = accessToken;
        MessageId = messageId;
    }
}
