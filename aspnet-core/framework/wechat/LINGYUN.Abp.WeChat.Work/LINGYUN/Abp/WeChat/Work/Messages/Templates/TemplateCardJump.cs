using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;
/// <summary>
/// 跳转指引样式
/// </summary>
public class TemplateCardJump
{
    public TemplateCardJump(string title, string url)
    {
        Type = 1;
        Url = url;
        Title = title;
    }
    public TemplateCardJump(string title, string appid, string pagePath)
    {
        Type = 2;
        AppId = appid;
        PagePath = pagePath;
        Title = title;
    }
    /// <summary>
    /// 跳转链接类型，0或不填代表不是链接，1 代表跳转url，2 代表跳转小程序
    /// </summary>
    [CanBeNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public byte Type { get; }
    /// <summary>
    /// 跳转链接样式的文案内容，建议不超过18个字
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 跳转链接的url，jump_list.type是1时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 跳转链接的小程序的appid，必须是与当前应用关联的小程序，jump_list.type是2时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("appid")]
    [JsonPropertyName("appid")]
    public string AppId { get; set; }
    /// <summary>
    /// 跳转链接的小程序的pagepath，jump_list.type是2时选填
    /// </summary>
    [CanBeNull]
    [JsonProperty("pagepath")]
    [JsonPropertyName("pagepath")]
    public string PagePath { get; set; }
}
