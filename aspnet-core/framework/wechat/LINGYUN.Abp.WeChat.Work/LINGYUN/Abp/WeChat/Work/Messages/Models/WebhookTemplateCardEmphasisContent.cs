using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 关键数据样式
/// </summary>
public class WebhookTemplateCardEmphasisContent
{
    /// <summary>
    /// 关键数据样式的数据内容，建议不超过10个字
    /// </summary>
    [CanBeNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 关键数据样式的数据描述内容，建议不超过15个字
    /// </summary>
    [CanBeNull]
    [JsonProperty("desc")]
    [JsonPropertyName("desc")]
    public string Description { get; set; }
    /// <summary>
    /// 创建一个关键数据样式
    /// </summary>
    /// <param name="title">关键数据样式的数据内容</param>
    /// <param name="description">关键数据样式的数据描述内容</param>
    public WebhookTemplateCardEmphasisContent(string title, string description = null)
    {
        Title = title;
        Description = description;
    }
}
