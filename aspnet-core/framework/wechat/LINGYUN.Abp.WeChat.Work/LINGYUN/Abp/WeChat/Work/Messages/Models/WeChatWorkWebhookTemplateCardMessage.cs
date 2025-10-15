using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信Webhook模版卡片消息
/// </summary>
public class WeChatWorkWebhookTemplateCardMessage : WeChatWorkWebhookMessage
{
    /// <summary>
    /// 模版卡片消息体
    /// </summary>
    [NotNull]
    [JsonProperty("template_card")]
    [JsonPropertyName("template_card")]
    public WebhookTemplateCardMessage TemplateCard { get; set; }
    /// <summary>
    /// 创建一个企业微信Webhook模版卡片消息
    /// </summary>
    /// <param name="templateCard">模版卡片消息体</param>
    public WeChatWorkWebhookTemplateCardMessage(WebhookTemplateCardMessage templateCard)
        : base("template_card")
    {
        TemplateCard = templateCard;
    }
}
