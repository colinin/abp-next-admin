using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;
/// <summary>
/// 引用文献样式
/// </summary>
public class TemplateCardQuoteArea
{
    public TemplateCardQuoteArea(string url, string title = "", string text = "")
    {
        Type = 1;
        Url = url;
        Title = title;
        Text = text;
    }
    public TemplateCardQuoteArea(string appid, string pagePath, string title = "", string text = "")
    {
        Type = 2;
        AppId = appid;
        PagePath = pagePath;
        Title = title;
        Text = text;
    }
    /// <summary>
    /// 引用文献样式区域点击事件，0或不填代表没有点击事件，1 代表跳转url，2 代表跳转小程序
    /// </summary>
    [CanBeNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public byte Type { get; }
    /// <summary>
    /// 点击跳转的url，quote_area.type是1时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 点击跳转的小程序的appid，必须是与当前应用关联的小程序，quote_area.type是2时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("appid")]
    [JsonPropertyName("appid")]
    public string AppId { get; set; }
    /// <summary>
    /// 点击跳转的小程序的pagepath，quote_area.type是2时选填
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
    public string Text { get; set; }
}
