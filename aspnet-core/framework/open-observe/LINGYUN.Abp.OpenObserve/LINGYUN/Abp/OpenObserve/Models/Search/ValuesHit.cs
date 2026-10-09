using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Search;

/// <summary>
/// 单个字段的取值
/// </summary>
public class ValuesHit
{
    /// <summary>
    /// 字段名称
    /// </summary>
    [JsonPropertyName("field")]
    public string? Field { get; set; }

    /// <summary>
    /// 取值列表
    /// </summary>
    [JsonPropertyName("values")]
    public ValuesItem[] Values { get; set; } = [];
}
