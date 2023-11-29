using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信群聊markdown消息
/// </summary>
public class WeChatWorkAppChatMarkdownMessage : WeChatWorkAppChatMessage
{
    public WeChatWorkAppChatMarkdownMessage(
        string agentId,
        string chatId,
        MarkdownMessage markdown) : base(agentId, chatId, "markdown")
    {
        Markdown = markdown;
    }
    /// <summary>
    /// markdown消息
    /// </summary>
    [NotNull]
    [JsonProperty("markdown")]
    [JsonPropertyName("markdown")]
    public MarkdownMessage Markdown { get; set; }
    /// <summary>
    /// 表示是否是保密消息，
    /// 0不保密
    /// 1保密
    /// 默认为0
    /// </summary>
    [JsonProperty("safe")]
    [JsonPropertyName("safe")]
    public byte Safe { get; set; }
}
