using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Tags.Models;
/// <summary>
/// 标签
/// </summary>
public class TagInfo
{
    /// <summary>
    /// 标签id
    /// </summary>
    [NotNull]
    [JsonProperty("tagid")]
    [JsonPropertyName("tagid")]
    public int TagId { get; set; }
    /// <summary>
    /// 标签名
    /// </summary>
    [NotNull]
    [JsonProperty("tagname")]
    [JsonPropertyName("tagname")]
    public string TagName { get; set; }
}
