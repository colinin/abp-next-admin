using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch;

[Serializable]
public class SerilogInfo
{
    [JsonPropertyName(ElasticsearchJsonFormatter.TimestampPropertyName)]
    public DateTime TimeStamp { get; set; }

    [JsonPropertyName(ElasticsearchJsonFormatter.LevelPropertyName)]
    public LogEventLevel Level { get; set; }

    [JsonPropertyName(ElasticsearchJsonFormatter.RenderedMessagePropertyName)]
    public string Message { get; set; }

    [JsonPropertyName("fields")]
    public SerilogField Fields { get; set; }

    [JsonPropertyName("exceptions")]
    public List<SerilogException> Exceptions { get; set; }
}
