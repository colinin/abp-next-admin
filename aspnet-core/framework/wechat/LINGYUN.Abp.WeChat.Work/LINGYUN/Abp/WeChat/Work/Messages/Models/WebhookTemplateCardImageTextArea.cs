using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 左图右文样式
/// </summary>
public class WebhookTemplateCardImageTextArea
{
    /// <summary>
    /// 左图右文样式区域点击事件，0或不填代表没有点击事件，1 代表跳转url，2 代表跳转小程序
    /// </summary>
    [CanBeNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public int? Type { get; set; }
    /// <summary>
    /// 左图右文样式的图片url
    /// </summary>
    [NotNull]
    [JsonProperty("image_url")]
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }
    /// <summary>
    /// 点击跳转的url，type是1时必填
    /// </summary>
    [NotNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 点击跳转的小程序的appid，type是2时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("appid")]
    [JsonPropertyName("appid")]
    public string AppId { get; set; }
    /// <summary>
    /// 点击跳转的小程序的pagepath，type是2时选填
    /// </summary>
    [CanBeNull]
    [JsonProperty("pagepath")]
    [JsonPropertyName("pagepath")]
    public string PagePath { get; set; }
    /// <summary>
    /// 左图右文样式的标题
    /// </summary>
    [CanBeNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 左图右文样式的描述
    /// </summary>
    [CanBeNull]
    [JsonProperty("desc")]
    [JsonPropertyName("desc")]
    public string Description { get; set; }
    private WebhookTemplateCardImageTextArea(
        string imageUrl,
        int? type = null, 
        string url = null,
        string appId = null,
        string pagePath = null,
        string title = null,
        string description = null)
    {
        Type = type;
        ImageUrl = imageUrl;
        Url = url;
        AppId = appId;
        PagePath = pagePath;
        Title = title;
        Description = description;
    }
    /// <summary>
    /// 创建一个跳转链接左图右文样式
    /// </summary>
    /// <param name="imageUrl">左图右文样式的图片url</param>
    /// <param name="url">点击跳转的url</param>
    /// <param name="title">左图右文样式的标题</param>
    /// <param name="description">左图右文样式的描述</param>
    /// <returns></returns>
    public static WebhookTemplateCardImageTextArea Link(
        string imageUrl,
        string url,
        string title = null,
        string description = null)
    {
        Check.NotNullOrWhiteSpace(imageUrl, nameof(imageUrl));
        Check.NotNullOrWhiteSpace(url, nameof(url));

        return new WebhookTemplateCardImageTextArea(imageUrl, 1, url: url, title: title, description: description);
    }
    /// <summary>
    /// 创建一个跳转小程序左图右文样式
    /// </summary>
    /// <param name="imageUrl">左图右文样式的图片url</param>
    /// <param name="appId">小程序AppId</param>
    /// <param name="pagePath">小程序PagePath</param>
    /// <param name="title">左图右文样式的标题</param>
    /// <param name="description">左图右文样式的描述</param>
    /// <returns></returns>
    public static WebhookTemplateCardImageTextArea MiniProgram(
        string imageUrl, 
        string appId, 
        string pagePath,
        string title = null,
        string description = null)
    {
        Check.NotNullOrWhiteSpace(imageUrl, nameof(imageUrl));
        Check.NotNullOrWhiteSpace(appId, nameof(appId));
        Check.NotNullOrWhiteSpace(pagePath, nameof(pagePath));

        return new WebhookTemplateCardImageTextArea(imageUrl, 2, appId: appId, pagePath: pagePath, title: title, description: description);
    }
}
