using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 模版卡片的主要内容
/// </summary>
public class TemplateCardMainTitle
{
    /// <summary>
    /// 一级标题，建议不超过26个字。模版卡片主要内容的一级标题main_title.title和二级普通文本sub_title_text必须有一项填写
    /// </summary>
    [CanBeNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 标题辅助信息，建议不超过30个字
    /// </summary>
    [CanBeNull]
    [JsonProperty("desc")]
    [JsonPropertyName("desc")]
    public string Description { get; set; }
    /// <summary>
    /// 创建一个模版卡片的主要内容
    /// </summary>
    /// <param name="title">一级标题</param>
    /// <param name="description">标题辅助信息</param>
    public TemplateCardMainTitle(string title, string description = null)
    {
        Title = title;
        Description = description;
    }
}
