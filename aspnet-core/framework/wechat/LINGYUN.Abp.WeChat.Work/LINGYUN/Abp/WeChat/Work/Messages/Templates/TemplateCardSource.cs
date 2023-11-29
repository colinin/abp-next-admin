using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;
/// <summary>
/// 来源文字颜色
/// </summary>
public enum DescriptionColor
{
    Gray = 0,
    Black = 1,
    Red = 2,
    Green = 3,
}
/// <summary>
/// 卡片来源样式信息
/// </summary>
public class TemplateCardSource
{
    public TemplateCardSource(
        string iconUrl = "", 
        string description = "",
        DescriptionColor descriptionColor = DescriptionColor.Gray)
    {
        IconUrl = iconUrl;
        Description = description;
        DescriptionColor = descriptionColor;
    }

    /// <summary>
    /// 来源图片的url，来源图片的尺寸建议为72*72
    /// </summary>
    [CanBeNull]
    [JsonProperty("icon_url")]
    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; }
    /// <summary>
    /// 来源图片的描述，建议不超过20个字，（支持id转译）
    /// </summary>
    [CanBeNull]
    [JsonProperty("desc")]
    [JsonPropertyName("desc")]
    public string Description { get; set; }
    /// <summary>
    /// 来源文字的颜色，目前支持：0(默认) 灰色，1 黑色，2 红色，3 绿色
    /// </summary>
    [CanBeNull]
    [JsonProperty("desc_color")]
    [JsonPropertyName("desc_color")]
    public DescriptionColor DescriptionColor { get; set; }
}
