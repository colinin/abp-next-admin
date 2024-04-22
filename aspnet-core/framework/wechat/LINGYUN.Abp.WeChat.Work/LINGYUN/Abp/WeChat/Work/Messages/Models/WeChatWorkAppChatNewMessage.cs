using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信群聊文本图文消息
/// </summary>
public class WeChatWorkAppChatNewMessage : WeChatWorkAppChatMessage
{
    public WeChatWorkAppChatNewMessage(
        string agentId,
        string chatId,
        NewMessagePayload news) : base(agentId, chatId, "news")
    {
        News = news;
    }
    /// <summary>
    /// 图文消息
    /// </summary>
    [NotNull]
    [JsonProperty("news")]
    [JsonPropertyName("news")]
    public NewMessagePayload News { get; set; }
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
