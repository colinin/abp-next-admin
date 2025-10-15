using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook语音消息体
/// </summary>
public class WebhookVoiceMessage
{
    /// <summary>
    /// 语音文件id，通过下文的文件上传接口获取
    /// </summary>
    [NotNull]
    [JsonProperty("media_id")]
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
    /// <summary>
    /// 创建一个Webhook语音消息体
    /// </summary>
    /// <param name="mediaId">语音文件id</param>
    public WebhookVoiceMessage(string mediaId)
    {
        MediaId = mediaId;
    }
}
