using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 视频消息
/// </summary>
public class VideoMessage
{
    public VideoMessage(
        string mediaId,
        string title = "",
        string description = "")
    {
        Title = title;
        Description = description;
        MediaId = mediaId;
    }

    /// <summary>
    /// 视频消息的标题，不超过128个字节，超过会自动截断
    /// </summary>
    [CanBeNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 视频消息的描述，不超过512个字节，超过会自动截断
    /// </summary>
    [CanBeNull]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string Description { get; set; }
    /// <summary>
    /// 视频媒体文件id，可以调用上传临时素材接口获取
    /// </summary>
    [NotNull]
    [JsonProperty("media_id")]
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
}
