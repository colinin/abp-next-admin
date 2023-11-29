using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// markdown消息
/// </summary>
public class MarkdownMessage
{
    /// <summary>
    /// markdown内容，最长不超过2048个字节，必须是utf8编码
    /// </summary>
    [NotNull]
    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }
    public MarkdownMessage(string content)
    {
        Content = content;
    }
}
