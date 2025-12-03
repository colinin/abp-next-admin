using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 文本类型的成员属性
/// </summary>
public class MemberTextAttribute : MemberAttribute
{
    /// <summary>
    /// 文本
    /// </summary>
    [NotNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public MemberTextModel Text { get; set; }
}

public class MemberTextModel
{
    /// <summary>
    /// 文本
    /// </summary>
    [NotNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public string Value { get; set; }
}
