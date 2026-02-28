using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Models;
public abstract class TagGroup
{
    /// <summary>
    /// 标签组id
    /// </summary>
    [NotNull]
    [JsonProperty("group_id")]
    [JsonPropertyName("group_id")]
    public string GroupId { get; set; }
    /// <summary>
    /// 标签组名称
    /// </summary>
    [NotNull]
    [JsonProperty("group_name")]
    [JsonPropertyName("group_name")]
    public string GroupName { get; set; }
    /// <summary>
    /// 标签组创建时间
    /// </summary>
    [NotNull]
    [JsonProperty("create_time")]
    [JsonPropertyName("create_time")]
    public long CreateTime { get; set; }
    /// <summary>
    /// 标签组排序的次序值，order值大的排序靠前。有效的值范围是[0, 2^32)
    /// </summary>
    [NotNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int Order { get; set; }
}
