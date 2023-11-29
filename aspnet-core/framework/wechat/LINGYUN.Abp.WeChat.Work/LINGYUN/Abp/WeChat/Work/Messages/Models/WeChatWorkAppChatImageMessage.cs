using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信群聊图片消息
/// </summary>
public class WeChatWorkAppChatImageMessage : WeChatWorkAppChatMessage
{
    public WeChatWorkAppChatImageMessage(
        string agentId,
        string chatId,
        MediaMessage image) : base(agentId, chatId, "image")
    {
        Image = image;
    }
    /// <summary>
    /// 图片媒体文件
    /// </summary>
    [NotNull]
    [JsonProperty("image")]
    [JsonPropertyName("image")]
    public MediaMessage Image { get; set; }
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
