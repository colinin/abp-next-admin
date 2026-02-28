using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch;

public class SerilogException
{
    [JsonPropertyName("SourceContext")]
    public int Depth { get; set; }

    [JsonPropertyName("ClassName")]
    public string Class { get; set; }

    [JsonPropertyName("Message")]
    public string Message { get; set; }

    [JsonPropertyName("Source")]
    public string Source { get; set; }

    [JsonPropertyName("StackTraceString")]
    public string StackTrace { get; set; }

    [JsonPropertyName("HResult")]
    public int HResult { get; set; }

    [JsonPropertyName("HelpURL")]
    public string HelpURL { get; set; }
}
