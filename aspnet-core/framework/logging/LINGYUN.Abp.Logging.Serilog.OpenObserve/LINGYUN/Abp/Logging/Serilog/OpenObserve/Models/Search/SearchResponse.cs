using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models.Search;

[Serializable]
public class SearchResponse<THit>
{
    [JsonPropertyName("hits")]
    public THit[] Hits { get; set; } = new THit[0];

    [JsonPropertyName("total")]
    public long Total { get; set; }

    [JsonPropertyName("from")]
    public int From { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("trace_id")]
    public string? TraceId { get; set; }

    [JsonPropertyName("order_by")]
    public string? OrderBy { get; set; }

    [JsonPropertyName("order_by_metadata")]
    public string[][]? OrderByMetadata { get; set; }
}
