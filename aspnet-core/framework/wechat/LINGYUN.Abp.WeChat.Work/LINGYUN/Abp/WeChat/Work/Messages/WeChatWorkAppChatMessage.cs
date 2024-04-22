using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages;
/// <summary>
/// 企业微信群聊消息
/// </summary>
public abstract class WeChatWorkAppChatMessage : WeChatWorkRequest
{
    protected WeChatWorkAppChatMessage(
        string agentId,
        string chatId,
        string msgType)
    {
        AgentId = agentId;
        ChatId = chatId;
        MsgType = msgType;
    }

    /// <summary>
    /// 群聊id
    /// </summary>
    [NotNull]
    [JsonProperty("chatid")]
    [JsonPropertyName("chatid")]
    public virtual string ChatId { get; protected set; }
    /// <summary>
    /// 消息类型
    /// </summary>
    [NotNull]
    [JsonProperty("msgtype")]
    [JsonPropertyName("msgtype")]
    public virtual string MsgType { get; protected set; }
    /// <summary>
    /// 企业应用的id
    /// </summary>
    /// <remarks>
    /// 由于需要换取token, 因此需要传递应用标识
    /// </remarks>
    [NotNull]
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual string AgentId { get; protected set; }
}
