using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 卡片来源样式信息
/// </summary>
public class TemplateCardSource
{
    /// <summary>
    /// 来源图片的url
    /// </summary>
    [CanBeNull]
    [JsonProperty("icon_url")]
    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; }
    /// <summary>
    /// 来源图片的描述，建议不超过13个字
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
    public int? DescriptionColor { get; set; }
    private TemplateCardSource(
        string iconUrl = null,
        string description = null,
        int? descriptionColor = 0)
    {
        IconUrl = iconUrl;
        Description = description;
        DescriptionColor = descriptionColor;
    }
    /// <summary>
    /// 创建一个灰色卡片来源样式
    /// </summary>
    /// <param name="iconUrl">来源图片的url</param>
    /// <param name="description">来源图片的描述</param>
    /// <returns></returns>
    public static TemplateCardSource Grey(string iconUrl, string description = null)
    {
        return new TemplateCardSource(iconUrl, description, 0);
    }
    /// <summary>
    /// 创建一个黑色卡片来源样式
    /// </summary>
    /// <param name="iconUrl">来源图片的url</param>
    /// <param name="description">来源图片的描述</param>
    /// <returns></returns>
    public static TemplateCardSource Black(string iconUrl, string description = null)
    {
        return new TemplateCardSource(iconUrl, description, 1);
    }
    /// <summary>
    /// 创建一个红色卡片来源样式
    /// </summary>
    /// <param name="iconUrl">来源图片的url</param>
    /// <param name="description">来源图片的描述</param>
    /// <returns></returns>
    public static TemplateCardSource Red(string iconUrl, string description = null)
    {
        return new TemplateCardSource(iconUrl, description, 2);
    }
    /// <summary>
    /// 创建一个绿色卡片来源样式
    /// </summary>
    /// <param name="iconUrl">来源图片的url</param>
    /// <param name="description">来源图片的描述</param>
    /// <returns></returns>
    public static TemplateCardSource Green(string iconUrl, string description = null)
    {
        return new TemplateCardSource(iconUrl, description, 3);
    }
}
