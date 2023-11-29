using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信群聊语言消息
/// </summary>
public class WeChatWorkAppChatVideoMessage : WeChatWorkAppChatMessage
{
    public WeChatWorkAppChatVideoMessage(
        string agentId,
        string chatId,
        VideoMessage video) : base(agentId, chatId, "video")
    {
        Video = video;
    }
    /// <summary>
    /// 视频媒体文件
    /// </summary>
    [NotNull]
    [JsonProperty("video")]
    [JsonPropertyName("video")]
    public VideoMessage Video { get; set; }
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
