using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信Webhook Markdown消息
/// </summary>
public class WeChatWorkWebhookMarkdownMessage : WeChatWorkWebhookMessage
{
    /// <summary>
    /// Markdown消息体
    /// </summary>
    [NotNull]
    [JsonProperty("markdown")]
    [JsonPropertyName("markdown")]
    public WebhookMarkdownMessage Markdown { get; set; }
    /// <summary>
    /// 创建一个企业微信Webhook Markdown消息
    /// </summary>
    /// <param name="markdown">Markdown消息体</param>
    public WeChatWorkWebhookMarkdownMessage(WebhookMarkdownMessage markdown)
    : base("markdown")
    {
        Markdown = markdown;
    }
}
