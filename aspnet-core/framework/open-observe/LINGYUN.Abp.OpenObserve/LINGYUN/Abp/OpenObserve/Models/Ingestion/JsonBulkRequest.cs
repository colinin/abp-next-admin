using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Ingestion;

/// <summary>
/// _bulk 动作行
/// </summary>
/// <remarks>
/// {"index":{"_index":"stream1"}}, 动作名(index/create/update)由外层字典的键决定
/// </remarks>
public class JsonBulkRequest
{
    /// <summary>
    /// 数据流名称
    /// </summary>
    [JsonPropertyName("_index")]
    public string Index { get; set; } = default!;
}
