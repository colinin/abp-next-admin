using JetBrains.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook MarkdownV2消息体
/// </summary>
public class WebhookMarkdownV2Message
{
    /// <summary>
    /// markdown_v2内容，最长不超过4096个字节，必须是utf8编码。
    /// </summary>
    /// <remarks>
    /// 1. markdown_v2不支持字体颜色、@群成员的语法， 具体支持的语法可参考下面说明<br />
    /// 2. 消息内容在客户端 4.1.36 版本以下(安卓端为4.1.38以下) 消息表现为纯文本，建议使用最新客户端版本体验
    /// </remarks>
    [NotNull]
    [StringLength(4096)]
    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }
    /// <summary>
    /// 创建一个Webhook MarkdownV2消息体
    /// </summary>
    /// <param name="content">markdown_v2内容，最长不超过4096个字节，必须是utf8编码</param>
    public WebhookMarkdownV2Message(string content)
    {
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), 4096);
    }
}
