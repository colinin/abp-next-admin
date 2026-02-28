using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 成员扩展属性
/// </summary>
public abstract class MemberAttribute
{
    /// <summary>
    /// 属性名称： 在新增或者更新操作时，需要先确保在管理端有创建该属性，否则会忽略
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    /// <summary>
    /// 属性类型
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public AttributeType Type { get; set; }
}
