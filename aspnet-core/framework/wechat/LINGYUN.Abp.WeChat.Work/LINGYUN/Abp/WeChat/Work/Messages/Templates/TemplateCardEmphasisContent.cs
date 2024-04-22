using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;
/// <summary>
/// 关键数据样式
/// </summary>
public class TemplateCardEmphasisContent
{
    public TemplateCardEmphasisContent(
        string title = "", 
        string description = "")
    {
        Title = title;
        Description = description;
    }

    /// <summary>
    /// 关键数据样式的数据内容，建议不超过14个字
    /// </summary>
    [CanBeNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 关键数据样式的数据描述内容，建议不超过22个字
    /// </summary>
    [CanBeNull]
    [JsonProperty("desc")]
    [JsonPropertyName("desc")]
    public string Description { get; set; }
}
