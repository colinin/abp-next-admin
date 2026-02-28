using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信Webhook图文消息
/// </summary>
internal class WeChatWorkWebhookNewsMessage : WeChatWorkWebhookMessage
{
    /// <summary>
    /// 图文消息体
    /// </summary>
    [NotNull]
    [JsonProperty("news")]
    [JsonPropertyName("news")]
    public WebhookNewsMessage News { get; set; }
    /// <summary>
    /// 创建一个企业微信Webhook图文消息
    /// </summary>
    /// <param name="news">图文消息体</param>
    public WeChatWorkWebhookNewsMessage(WebhookNewsMessage news)
        : base("news")
    {
        News = news;
    }
}
