using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging;

public class LogInfo
{
    [JsonPropertyName("@timestamp")]
    public DateTime TimeStamp { get; set; }

    [JsonPropertyName("level")]
    public LogLevel Level { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("fields")]
    public LogField Fields { get; set; } = default!;

    [JsonPropertyName("exceptions")]
    public List<LogException>? Exceptions { get; set; }
}
