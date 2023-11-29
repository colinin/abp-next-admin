using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信群聊文本消息
/// </summary>
public class WeChatWorkAppChatTextMessage : WeChatWorkAppChatMessage
{
    public WeChatWorkAppChatTextMessage(
        string agentId,
        string chatId,
        TextMessage text) : base(agentId, chatId, "text")
    {
        Text = text;
    }

    /// <summary>
    /// 消息内容，最长不超过2048个字节，超过将截断（支持id转译）
    /// </summary>
    [NotNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public TextMessage Text { get; set; }
    /// <summary>
    /// 表示是否是保密消息，
    /// 0表示可对外分享，
    /// 1表示不能分享且内容显示水印，
    /// 默认为0
    /// </summary>
    [JsonProperty("safe")]
    [JsonPropertyName("safe")]
    public int Safe { get; set; }
}
