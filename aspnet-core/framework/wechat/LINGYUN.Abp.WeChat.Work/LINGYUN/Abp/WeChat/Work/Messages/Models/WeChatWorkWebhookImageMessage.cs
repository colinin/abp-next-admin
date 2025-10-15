using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信Webhook图片消息
/// </summary>
public class WeChatWorkWebhookImageMessage : WeChatWorkWebhookMessage
{
    /// <summary>
    /// 图片消息体
    /// </summary>
    [NotNull]
    [JsonProperty("image")]
    [JsonPropertyName("image")]
    public WebhookImageMessage Image { get; set; }
    /// <summary>
    /// 创建一个企业微信Webhook图片消息
    /// </summary>
    /// <param name="image">图片消息体</param>
    public WeChatWorkWebhookImageMessage(WebhookImageMessage image)
        : base("image")
    {
        Image = image;
    }
}
