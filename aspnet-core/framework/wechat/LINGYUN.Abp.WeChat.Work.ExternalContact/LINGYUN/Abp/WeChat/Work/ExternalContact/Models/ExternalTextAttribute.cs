using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Models;
/// <summary>
/// 文本类型属性
/// </summary>
public class ExternalTextAttribute : ExternalAttribute
{
    /// <summary>
    /// 文本
    /// </summary>
    [NotNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public ExternalTextModel Text { get; set; }
}

public class ExternalTextModel
{
    /// <summary>
    /// 文本
    /// </summary>
    [NotNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public string Value { get; set; }
}
