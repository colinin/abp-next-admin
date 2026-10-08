using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models;

[Serializable]
public class OpenObserveErrorResponse
{
    /// <summary>
    /// Numeric error code (see below)
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }
    /// <summary>
    /// Human-readable problem description
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = default!;

    /// <summary>
    /// Raw technical detail (omitted when hint/suggestions are present)
    /// </summary>
    [JsonPropertyName("error_detail")]
    public string? ErrorDetail { get; set; }

    /// <summary>
    /// One-line guidance on how to fix the error
    /// </summary>
    [JsonPropertyName("hint")]
    public string? Hint { get; set; }

    /// <summary>
    /// Closest valid alternatives (up to 3), ranked by similarity
    /// </summary>
    [JsonPropertyName("suggestions")]
    public string? Suggestions { get; set; }

    [JsonPropertyName("trace_id")]
    public string? TraceId { get; set; }
}
