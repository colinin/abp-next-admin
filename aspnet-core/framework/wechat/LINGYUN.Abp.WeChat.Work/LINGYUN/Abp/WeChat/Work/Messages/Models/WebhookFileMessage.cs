using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook文件消息体
/// </summary>
public class WebhookFileMessage
{
    /// <summary>
    /// 文件id，通过下文的文件上传接口获取
    /// </summary>
    [NotNull]
    [JsonProperty("media_id")]
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
    /// <summary>
    /// 创建一个Webhook文件消息体
    /// </summary>
    /// <param name="mediaId">文件id，通过下文的文件上传接口获取</param>
    public WebhookFileMessage(string mediaId)
    {
        MediaId = mediaId;
    }
}
