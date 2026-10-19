using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Search;

/// <summary>
/// 取值项
/// </summary>
public class ValuesItem
{
    /// <summary>
    /// 取值
    /// </summary>
    [JsonPropertyName("zo_sql_key")]
    public string? Key { get; set; }

    /// <summary>
    /// 出现次数
    /// </summary>
    [JsonPropertyName("zo_sql_num")]
    public long Num { get; set; }
}
