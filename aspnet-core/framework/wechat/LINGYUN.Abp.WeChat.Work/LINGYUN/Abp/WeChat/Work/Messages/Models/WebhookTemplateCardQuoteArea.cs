using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 引用文献
/// </summary>
public class WebhookTemplateCardQuoteArea
{
    /// <summary>
    /// 引用文献样式区域点击事件，0或不填代表没有点击事件，1 代表跳转url，2 代表跳转小程序
    /// </summary>
    [CanBeNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public int? Type { get; set; }
    /// <summary>
    /// 点击跳转的url，type是1时必填
    /// </summary>
    [CanBeNull]
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
    /// 引用文献样式的标题
    /// </summary>
    [CanBeNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 引用文献样式的引用文案
    /// </summary>
    [CanBeNull]
    [JsonProperty("quote_text")]
    [JsonPropertyName("quote_text")]
    public string QuoteText { get; set; }
    private WebhookTemplateCardQuoteArea(
        string title,
        int? type = null,
        string url = null,
        string appId = null,
        string pagePath = null,
        string quoteText = null)
    {
        Title = title;
        QuoteText = quoteText;
        Type = type;
        Url = url;
        AppId = appId;
        PagePath = pagePath;
    }
    /// <summary>
    /// 创建一个默认引用文献
    /// </summary>
    /// <param name="title">引用文献样式的标题</param>
    /// <param name="quoteText">引用文献样式的引用文案</param>
    /// <returns></returns>
    public static WebhookTemplateCardQuoteArea Default(string title, string quoteText = null)
    {
        Check.NotNullOrWhiteSpace(title, nameof(title));

        return new WebhookTemplateCardQuoteArea(title, quoteText: quoteText);
    }
    /// <summary>
    /// 创建一个跳转链接的引用文献
    /// </summary>
    /// <param name="title">引用文献样式的标题</param>
    /// <param name="url">点击跳转的url</param>
    /// <param name="quoteText">引用文献样式的引用文案</param>
    /// <returns></returns>
    public static WebhookTemplateCardQuoteArea Link(string title, string url, string quoteText = null)
    {
        Check.NotNullOrWhiteSpace(title, nameof(title));
        Check.NotNullOrWhiteSpace(url, nameof(url));

        return new WebhookTemplateCardQuoteArea(title, 1, url, quoteText: quoteText);
    }
    /// <summary>
    /// 创建一个跳转小程序的引用文献
    /// </summary>
    /// <param name="title">引用文献样式的标题</param>
    /// <param name="appId">跳转链接的小程序的appid</param>
    /// <param name="pagePath">跳转链接的小程序的pagepath</param>
    /// <param name="quoteText">引用文献样式的引用文案</param>
    /// <returns></returns>
    public static WebhookTemplateCardQuoteArea MiniProgram(string title, string appId, string pagePath, string quoteText = null)
    {
        Check.NotNullOrWhiteSpace(title, nameof(title));
        Check.NotNullOrWhiteSpace(appId, nameof(appId));
        Check.NotNullOrWhiteSpace(pagePath, nameof(pagePath));

        return new WebhookTemplateCardQuoteArea(title, 2, appId: appId, pagePath: pagePath, quoteText: quoteText);
    }
}
