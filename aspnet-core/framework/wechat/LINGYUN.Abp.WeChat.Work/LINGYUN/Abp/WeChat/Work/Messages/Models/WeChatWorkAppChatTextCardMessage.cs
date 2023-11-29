using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信群聊文本卡片消息
/// </summary>
public class WeChatWorkAppChatTextCardMessage : WeChatWorkAppChatMessage
{
    public WeChatWorkAppChatTextCardMessage(
        string agentId,
        string chatId,
        TextCardMessage textcard) : base(agentId, chatId, "textcard")
    {
        TextCard = textcard;
    }
    /// <summary>
    /// 卡片消息
    /// </summary>
    [NotNull]
    [JsonProperty("textcard")]
    [JsonPropertyName("textcard")]
    public TextCardMessage TextCard { get; set; }
    /// <summary>
    /// 表示是否是保密消息，
    /// 0表示可对外分享，
    /// 1表示不能分享且内容显示水印，
    /// 默认为0
    /// </summary>
    [JsonProperty("safe")]
    [JsonPropertyName("safe")]
    public byte Safe { get; set; }
}
