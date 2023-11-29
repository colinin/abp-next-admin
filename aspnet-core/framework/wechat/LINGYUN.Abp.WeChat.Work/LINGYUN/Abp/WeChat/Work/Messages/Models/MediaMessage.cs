using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 媒体文件消息
/// </summary>
public class MediaMessage
{
    /// <summary>
    ///媒体文件id，可以调用上传临时素材接口获取
    /// </summary>
    [NotNull]
    [JsonProperty("media_id")]
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
    public MediaMessage(string mediaId)
    {
        MediaId = mediaId;
    }
}
