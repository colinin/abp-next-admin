using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 图文消息（mp）载体
/// </summary>
public class MpNewMessagePayload
{
    /// <summary>
    /// 图文消息（mp）列表
    /// </summary>
    [NotNull]
    [JsonProperty("articles")]
    [JsonPropertyName("articles")]
    public List<MpNewMessage> Articles { get; set; }
    public MpNewMessagePayload(List<MpNewMessage> articles)
    {
        Articles = articles;
    }
}
/// <summary>
/// 图文消息（mp）
/// </summary>
public class MpNewMessage
{
    public MpNewMessage(
        string title,
        string thumbMediaId,
        string content,
        string author = "",
        string contentSourceUrl = "",
        string description = "")
    {
        Title = title;
        ThumbMediaId = thumbMediaId;
        Author = author;
        ContentSourceUrl = contentSourceUrl;
        Content = content;
        Description = description;
    }

    /// <summary>
    /// 标题，不超过128个字节，超过会自动截断（支持id转译）
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 图文消息缩略图的media_id, 可以通过素材管理接口获得。此处thumb_media_id即上传接口返回的media_id
    /// </summary>
    [NotNull]
    [JsonProperty("thumb_media_id")]
    [JsonPropertyName("thumb_media_id")]
    public string ThumbMediaId { get; set; }
    /// <summary>
    /// 图文消息的作者，不超过64个字节
    /// </summary>
    [CanBeNull]
    [JsonProperty("author")]
    [JsonPropertyName("author")]
    public string Author { get; set; }
    /// <summary>
    /// 图文消息点击“阅读原文”之后的页面链接
    /// </summary>
    [CanBeNull]
    [JsonProperty("content_source_url")]
    [JsonPropertyName("content_source_url")]
    public string ContentSourceUrl { get; set; }
    /// <summary>
    /// 图文消息的内容，支持html标签，不超过666 K个字节（支持id转译）
    /// </summary>
    [NotNull]
    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }
    /// <summary>
    /// 图文消息的描述，不超过512个字节，超过会自动截断（支持id转译）
    /// </summary>
    [CanBeNull]
    [JsonProperty("digest")]
    [JsonPropertyName("digest")]
    public string Description { get; set; }
}
