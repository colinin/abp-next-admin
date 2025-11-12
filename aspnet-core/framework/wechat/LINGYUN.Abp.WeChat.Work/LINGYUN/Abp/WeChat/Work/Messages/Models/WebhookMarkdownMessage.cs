using JetBrains.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook Markdown消息体
/// </summary>
public class WebhookMarkdownMessage
{
    /// <summary>
    /// markdown内容，最长不超过4096个字节，必须是utf8编码
    /// </summary>
    [NotNull]
    [StringLength(4096)]
    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }
    /// <summary>
    /// 创建一个Webhook Markdown消息体
    /// </summary>
    /// <param name="content">markdown内容，最长不超过4096个字节，必须是utf8编码</param>
    public WebhookMarkdownMessage(string content)
    {
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), 4096);
    }
}
