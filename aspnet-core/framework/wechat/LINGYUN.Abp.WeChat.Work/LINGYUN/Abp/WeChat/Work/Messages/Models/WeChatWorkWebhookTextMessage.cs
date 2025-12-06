using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信Webhook文本消息
/// </summary>
public class WeChatWorkWebhookTextMessage : WeChatWorkWebhookMessage
{
    /// <summary>
    /// 文本消息体
    /// </summary>
    [NotNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public WebhookTextMessage Text { get; set; }
    /// <summary>
    /// 创建一个企业微信Webhook文本消息
    /// </summary>
    /// <param name="text">文本消息体</param>
    public WeChatWorkWebhookTextMessage(WebhookTextMessage text) 
        : base("text")
    {
        Text = text;
    }
}
