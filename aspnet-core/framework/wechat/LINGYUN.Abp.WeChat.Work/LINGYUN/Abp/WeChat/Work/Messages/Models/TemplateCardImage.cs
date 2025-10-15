using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 图片样式
/// </summary>
public class TemplateCardImage
{
    /// <summary>
    /// 图片的url
    /// </summary>
    [NotNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 图片的宽高比，宽高比要小于2.25，大于1.3，不填该参数默认1.3
    /// </summary>
    [CanBeNull]
    [JsonProperty("aspect_ratio")]
    [JsonPropertyName("aspect_ratio")]
    public float? AspectRatio { get; set; }
    /// <summary>
    /// 创建一个图片样式
    /// </summary>
    /// <param name="url">图片的url</param>
    /// <param name="aspectRatio">图片的宽高比,不填该参数默认1.3</param>
    public TemplateCardImage(string url, float? aspectRatio = 1.3f)
    {
        Url = url;
        AspectRatio = aspectRatio;
    }
}
