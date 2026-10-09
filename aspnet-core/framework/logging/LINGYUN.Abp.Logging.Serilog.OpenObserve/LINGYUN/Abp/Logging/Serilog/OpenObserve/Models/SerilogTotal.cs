using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models;

public class SerilogTotal
{
    [JsonPropertyName("total")]
    public long Total { get; set; }
}
