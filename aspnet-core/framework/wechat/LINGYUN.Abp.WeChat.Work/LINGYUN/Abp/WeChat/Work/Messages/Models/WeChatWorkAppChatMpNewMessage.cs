using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信群聊文本图文消息
/// </summary>
public class WeChatWorkAppChatMpNewMessage : WeChatWorkAppChatMessage
{
    public WeChatWorkAppChatMpNewMessage(
        string agentId,
        string chatId,
        MpNewMessagePayload mpnews) : base(agentId, chatId, "mpnews")
    {
        News = mpnews;
    }
    /// <summary>
    /// 图文消息（mp）
    /// </summary>
    [NotNull]
    [JsonProperty("mpnews")]
    [JsonPropertyName("mpnews")]
    public MpNewMessagePayload News { get; set; }
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
