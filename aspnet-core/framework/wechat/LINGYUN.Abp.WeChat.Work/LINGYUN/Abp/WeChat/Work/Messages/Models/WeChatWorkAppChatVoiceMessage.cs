using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信群聊语言消息
/// </summary>
public class WeChatWorkAppChatVoiceMessage : WeChatWorkAppChatMessage
{
    public WeChatWorkAppChatVoiceMessage(
        string agentId,
        string chatId,
        MediaMessage voice) : base(agentId, chatId, "voice")
    {
        Voice = voice;
    }
    /// <summary>
    /// 语音媒体文件
    /// </summary>
    [NotNull]
    [JsonProperty("voice")]
    [JsonPropertyName("voice")]
    public MediaMessage Voice { get; set; }
}
