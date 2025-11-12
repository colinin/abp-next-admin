using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信Webhook语音消息
/// </summary>
public class WeChatWorkWebhookVoiceMessage : WeChatWorkWebhookMessage
{
    /// <summary>
    /// 语音消息体
    /// </summary>
    [NotNull]
    [JsonProperty("voice")]
    [JsonPropertyName("voice")]
    public WebhookVoiceMessage Voice { get; set; }
    /// <summary>
    /// 创建一个企业微信Webhook语音消息
    /// </summary>
    /// <param name="voice">语音消息体</param>
    public WeChatWorkWebhookVoiceMessage(WebhookVoiceMessage voice)
        : base("voice")
    {
        Voice = voice;
    }
}
