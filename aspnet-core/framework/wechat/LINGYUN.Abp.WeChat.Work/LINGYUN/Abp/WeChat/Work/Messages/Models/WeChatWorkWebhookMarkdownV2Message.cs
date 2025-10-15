using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信Webhook MarkdownV2消息
/// </summary>
public class WeChatWorkWebhookMarkdownV2Message : WeChatWorkWebhookMessage
{
    /// <summary>
    /// markdown_v2消息体
    /// </summary>
    [NotNull]
    [JsonProperty("markdown_v2")]
    [JsonPropertyName("markdown_v2")]
    public WebhookMarkdownV2Message Markdown { get; set; }
    /// <summary>
    /// 创建一个企业微信Webhook MarkdownV2消息
    /// </summary>
    /// <param name="markdown">markdown_v2消息体</param>
    public WeChatWorkWebhookMarkdownV2Message(WebhookMarkdownV2Message markdown)
    : base("markdown_v2")
    {
        Markdown = markdown;
    }
}
