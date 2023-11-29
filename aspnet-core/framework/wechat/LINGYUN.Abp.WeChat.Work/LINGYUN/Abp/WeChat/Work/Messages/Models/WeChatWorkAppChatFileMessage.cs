using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信群聊文件消息
/// </summary>
public class WeChatWorkAppChatFileMessage : WeChatWorkAppChatMessage
{
    public WeChatWorkAppChatFileMessage(
        string agentId,
        string chatId,
        MediaMessage file) : base(agentId, chatId, "file")
    {
        File = file;
    }
    /// <summary>
    /// 媒体文件
    /// </summary>
    [NotNull]
    [JsonProperty("file")]
    [JsonPropertyName("file")]
    public MediaMessage File { get; set; }
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
