using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信Webhook文件消息
/// </summary>
public class WeChatWorkWebhookFileMessage : WeChatWorkWebhookMessage
{
    /// <summary>
    /// 文件消息体
    /// </summary>
    [NotNull]
    [JsonProperty("file")]
    [JsonPropertyName("file")]
    public WebhookFileMessage File { get; set; }
    /// <summary>
    /// 创建一个企业微信Webhook文件消息
    /// </summary>
    /// <param name="file">文件消息体</param>
    public WeChatWorkWebhookFileMessage(WebhookFileMessage file)
        : base("file")
    {
        File = file;
    }
}
