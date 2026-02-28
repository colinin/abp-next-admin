using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook图文消息体
/// </summary>
public class WebhookNewsMessage
{
    /// <summary>
    /// 图文消息，一个图文消息支持1到8条图文
    /// </summary>
    [NotNull]
    [JsonProperty("articles")]
    [JsonPropertyName("articles")]
    public List<WebhookArticleMessage> Articles { get; set; }
    /// <summary>
    /// 创建一个Webhook图文消息体
    /// </summary>
    /// <param name="articles">图文消息列表</param>
    /// <exception cref="ArgumentException"></exception>
    public WebhookNewsMessage(List<WebhookArticleMessage> articles)
    {
        Articles = Check.NotNull(articles, nameof(articles));

        if (Articles.Count < 1 || Articles.Count > 8)
        {
            throw new ArgumentException("One image-text message supports 1 to 8 image-text messages!");
        }
    }
}

public class WebhookArticleMessage
{
    /// <summary>
    /// 标题，不超过128个字节，超过会自动截断
    /// </summary>
    [NotNull]
    [StringLength(128)]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 描述，不超过512个字节，超过会自动截断
    /// </summary>
    [CanBeNull]
    [StringLength(512)]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string Description { get; set; }
    /// <summary>
    /// 点击后跳转的链接。
    /// </summary>
    [NotNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图 1068*455，小图150*150。
    /// </summary>
    [CanBeNull]
    [JsonProperty("picurl")]
    [JsonPropertyName("picurl")]
    public string PictureUrl { get; set; }
    /// <summary>
    /// 创建一个图文消息
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="url">点击后跳转的链接</param>
    /// <param name="description">描述</param>
    /// <param name="pictureUrl">图文消息的图片链接</param>
    public WebhookArticleMessage(
        string title,
        string url,
        string description = "",
        string pictureUrl = "")
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title), 128);
        Url = Check.NotNullOrWhiteSpace(url, nameof(url));
        Description = Check.Length(description, nameof(description), 512);
        PictureUrl = pictureUrl;
    }
}
