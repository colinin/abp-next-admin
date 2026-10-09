using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Ingestion;

/// <summary>
/// _bulk 失败项错误信息
/// </summary>
public class JsonBulkItemError
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("index_uuid")]
    public string? IndexUuid { get; set; }

    [JsonPropertyName("shard")]
    public string? Shard { get; set; }

    [JsonPropertyName("index")]
    public string? Index { get; set; }
}
