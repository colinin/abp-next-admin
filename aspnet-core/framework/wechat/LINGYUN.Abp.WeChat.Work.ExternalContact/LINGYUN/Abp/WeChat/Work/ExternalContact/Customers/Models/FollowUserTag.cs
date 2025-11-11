using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 外部联系人所打标签
/// </summary>
public class FollowUserTag
{
    /// <summary>
    /// 该成员添加此外部联系人所打标签的分组名称
    /// </summary>
    [CanBeNull]
    [JsonProperty("group_name")]
    [JsonPropertyName("group_name")]
    public string? GroupName { get; set; }
    /// <summary>
    /// 该成员添加此外部联系人所打标签名称
    /// </summary>
    [NotNull]
    [JsonProperty("tag_name")]
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }
    /// <summary>
    /// 该成员添加此外部联系人所打标签类型
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public FollowUserTagType Type { get; set; }
    /// <summary>
    /// 该成员添加此外部联系人所打企业标签的id，用户自定义类型标签（type=2）不返回
    /// </summary>
    [CanBeNull]
    [JsonProperty("tag_id")]
    [JsonPropertyName("tag_id")]
    public string? TagId { get; set; }
}
