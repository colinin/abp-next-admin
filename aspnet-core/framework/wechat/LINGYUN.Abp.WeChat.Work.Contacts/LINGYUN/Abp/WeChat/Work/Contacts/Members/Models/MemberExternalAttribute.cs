using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 成员扩展属性
/// </summary>
public class MemberExternalAttribute
{
    /// <summary>
    /// 扩展属性列表
    /// </summary>
    [NotNull]
    [JsonProperty("attrs")]
    [JsonPropertyName("attrs")]
    public MemberAttribute[] Attributes { get; set; }
}
