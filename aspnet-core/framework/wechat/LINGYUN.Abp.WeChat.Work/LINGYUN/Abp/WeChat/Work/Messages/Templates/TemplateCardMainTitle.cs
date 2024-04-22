using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;
/// <summary>
/// 卡票标题
/// </summary>
public class TemplateCardMainTitle
{
    public TemplateCardMainTitle(
        string title = "", 
        string description = "")
    {
        Title = title;
        Description = description;
    }

    /// <summary>
    /// 一级标题，建议不超过36个字，文本通知型卡片本字段非必填，但不可本字段和sub_title_text都不填，（支持id转译）
    /// </summary>
    [CanBeNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 标题辅助信息，建议不超过44个字，（支持id转译）
    /// </summary>
    [CanBeNull]
    [JsonProperty("desc")]
    [JsonPropertyName("desc")]
    public string Description { get; set; }
}
