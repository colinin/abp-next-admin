using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages;
/// <summary>
/// 企业微信Webhook消息结构体
/// </summary>
public abstract class WeChatWorkWebhookMessage : WeChatWorkRequest
{
    /// <summary>
    /// 消息类型
    /// </summary>
    [NotNull]
    [JsonProperty("msgtype")]
    [JsonPropertyName("msgtype")]
    public string MsgType { get; set; }
    protected WeChatWorkWebhookMessage(string msgType)
    {
        MsgType = msgType;
    }
}
