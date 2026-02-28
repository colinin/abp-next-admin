using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 网页类型的成员属性
/// </summary>
public class MemberWebAttribute : MemberAttribute
{
    /// <summary>
    /// 网页
    /// </summary>
    [NotNull]
    [JsonProperty("web")]
    [JsonPropertyName("web")]
    public MemberWebModel Web { get; set; }
}

public class MemberWebModel
{
    /// <summary>
    /// 网页的url，必须包含http或者https头
    /// </summary>
    [NotNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 网页的展示标题，长度限制12个UTF8字符
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
}
