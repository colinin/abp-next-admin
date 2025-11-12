using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 卡片二级垂直内容
/// </summary>
public class WebhookTemplateCardVerticalContent
{
    /// <summary>
    /// 卡片二级标题，建议不超过26个字
    /// </summary>
    [CanBeNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 二级普通文本，建议不超过112个字
    /// </summary>
    [CanBeNull]
    [JsonProperty("desc")]
    [JsonPropertyName("desc")]
    public string Description { get; set; }
    /// <summary>
    /// 创建一个卡片二级垂直内容
    /// </summary>
    /// <param name="title">卡片二级标题</param>
    /// <param name="description">二级普通文本</param>
    public WebhookTemplateCardVerticalContent(string title, string description = null)
    {
        Title = title;
        Description = description;
    }
}
