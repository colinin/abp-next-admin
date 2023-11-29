using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;
/// <summary>
/// 整体卡片的点击跳转事件
/// </summary>
public class TemplateCardCardAction
{
    public TemplateCardCardAction(string url)
    {
        Type = 1;
        Url = url;
    }
    public TemplateCardCardAction(string appid, string pagePath)
    {
        Type = 2;
        AppId = appid;
        PagePath = pagePath;
    }
    /// <summary>
    /// 跳转链接类型，0或不填代表不是链接，1 代表跳转url，2 代表跳转小程序
    /// </summary>
    [CanBeNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public byte Type { get; set; }
    /// <summary>
    /// 跳转链接的url，card_action.type是1时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 跳转链接的小程序的appid，必须是与当前应用关联的小程序，card_action.type是2时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("appid")]
    [JsonPropertyName("appid")]
    public string AppId { get; set; }
    /// <summary>
    /// 跳转链接的小程序的pagepath，card_action.type是2时选填
    /// </summary>
    [CanBeNull]
    [JsonProperty("pagepath")]
    [JsonPropertyName("pagepath")]
    public string PagePath { get; set; }
}
