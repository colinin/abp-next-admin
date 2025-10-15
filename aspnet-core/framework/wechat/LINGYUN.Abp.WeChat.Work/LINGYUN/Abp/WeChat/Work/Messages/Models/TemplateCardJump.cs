using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 跳转指引样式
/// </summary>
public class TemplateCardJump
{
    /// <summary>
    /// 跳转链接类型，0或不填代表不是链接，1 代表跳转url，2 代表跳转小程序
    /// </summary>
    [CanBeNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public int? Type { get; set; }
    /// <summary>
    /// 跳转链接样式的文案内容，建议不超过13个字
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 跳转链接的url，type是1时必填
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
    private TemplateCardJump(
        string title,
        int? type = null,
        string url = null,
        string appId = null,
        string pagePath = null)
    {
        Type = type;
        Title = title;
        Url = url;
        AppId = appId;
        PagePath = pagePath;
    }
    /// <summary>
    /// 创建一个默认指引样式
    /// </summary>
    /// <param name="title">跳转链接样式的文案内容</param>
    /// <returns></returns>
    public static TemplateCardJump Default(string title)
    {
        Check.NotNullOrWhiteSpace(title, nameof(title));

        return new TemplateCardJump(title);
    }
    /// <summary>
    /// 创建一个跳转链接的指引样式
    /// </summary>
    /// <param name="title">跳转链接样式的文案内容</param>
    /// <param name="url">跳转链接的url</param>
    /// <returns></returns>
    public static TemplateCardJump Link(string title, string url)
    {
        Check.NotNullOrWhiteSpace(title, nameof(title));
        Check.NotNullOrWhiteSpace(url, nameof(url));

        return new TemplateCardJump(title, 1, url);
    }
    /// <summary>
    /// 创建一个跳转小程序的指引样式
    /// </summary>
    /// <param name="title">跳转链接样式的文案内容</param>
    /// <param name="appId">跳转链接的小程序的appid</param>
    /// <param name="pagePath">跳转链接的小程序的pagepath</param>
    /// <returns></returns>
    public static TemplateCardJump MiniProgram(string title, string appId, string pagePath)
    {
        Check.NotNullOrWhiteSpace(title, nameof(title));
        Check.NotNullOrWhiteSpace(appId, nameof(appId));
        Check.NotNullOrWhiteSpace(pagePath, nameof(pagePath));

        return new TemplateCardJump(title, 2, appId: appId, pagePath: pagePath);
    }
}
