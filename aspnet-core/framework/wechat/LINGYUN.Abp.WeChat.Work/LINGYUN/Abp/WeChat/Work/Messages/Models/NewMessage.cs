using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 图文消息载体
/// </summary>
public class NewMessagePayload
{
    /// <summary>
    /// 图文消息列表
    /// </summary>
    [NotNull]
    [JsonProperty("articles")]
    [JsonPropertyName("articles")]
    public List<NewMessage> Articles { get; set; }
    public NewMessagePayload(List<NewMessage> articles)
    {
        Articles = articles;
    }
}
/// <summary>
/// 图文消息
/// </summary>
public class NewMessage
{
    public NewMessage(
        string title,
        string description = "",
        string url = "",
        string pictureUrl = "",
        string appId = "",
        string pagePath = "")
    {
        Title = title;
        Description = description;
        Url = url;
        PictureUrl = pictureUrl;
        AppId = appId;
        PagePath = pagePath;
    }

    /// <summary>
    /// 标题，不超过128个字节，超过会自动截断（支持id转译）
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 描述，不超过512个字节，超过会自动截断（支持id转译）
    /// </summary>
    [CanBeNull]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string Description { get; set; }
    /// <summary>
    /// 点击后跳转的链接。
    /// 最长2048字节，请确保包含了协议头(http/https)，小程序或者url必须填写一个
    /// </summary>
    [CanBeNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 图文消息的图片链接，最长2048字节，支持JPG、PNG格式，较好的效果为大图 1068*455，小图150*150。
    /// </summary>
    [CanBeNull]
    [JsonProperty("picurl")]
    [JsonPropertyName("picurl")]
    public string PictureUrl { get; set; }
    /// <summary>
    /// 小程序appid，必须是与当前应用关联的小程序，appid和pagepath必须同时填写，填写后会忽略url字段
    /// </summary>
    [CanBeNull]
    [JsonProperty("appid")]
    [JsonPropertyName("appid")]
    public string AppId { get; set; }
    /// <summary>
    /// 点击消息卡片后的小程序页面，最长128字节，仅限本小程序内的页面。appid和pagepath必须同时填写，填写后会忽略url字段
    /// </summary>
    [CanBeNull]
    [JsonProperty("pagepath")]
    [JsonPropertyName("pagepath")]
    public string PagePath { get; set; }
}
