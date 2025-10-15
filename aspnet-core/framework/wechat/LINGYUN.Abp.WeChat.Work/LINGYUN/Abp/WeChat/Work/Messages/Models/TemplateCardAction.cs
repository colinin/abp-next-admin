using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 卡片的点击跳转事件
/// </summary>
public class TemplateCardAction
{
    /// <summary>
    /// 卡片跳转类型，1 代表跳转url，2 代表打开小程序。text_notice模版卡片中该字段取值范围为[1,2]
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public int Type { get; set; }
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
    private TemplateCardAction(
        int type,
        string url = null,
        string appId = null,
        string pagePath = null)
    {
        Type = type;
        Url = url;
        AppId = appId;
        PagePath = pagePath;
    }
    /// <summary>
    /// 创建一个跳转链接卡片事件
    /// </summary>
    /// <param name="url">跳转链接的url</param>
    /// <returns></returns>
    public static TemplateCardAction Link(string url)
    {
        Check.NotNullOrWhiteSpace(url, nameof(url));

        return new TemplateCardAction(1, url);
    }
    /// <summary>
    /// 创建一个跳转小程序卡片事件
    /// </summary>
    /// <param name="appId">小程序的appid</param>
    /// <param name="pagePath">小程序的pagePath</param>
    /// <returns></returns>
    public static TemplateCardAction MiniProgram(string appId, string pagePath)
    {
        Check.NotNullOrWhiteSpace(appId, nameof(appId));
        Check.NotNullOrWhiteSpace(pagePath, nameof(pagePath));

        return new TemplateCardAction(2, appId: appId, pagePath: pagePath);
    }
}
