using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 文本消息
/// </summary>
public class TextMessage
{
    public TextMessage(string content)
    {
        Content = content;
    }

    /// <summary>
    /// 消息内容，最长不超过2048个字节，超过将截断（支持id转译）
    /// </summary>
    [NotNull]
    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }
}
