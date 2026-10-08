using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models.Search;

public class SearchTotal
{
    [JsonPropertyName("total")]
    public long Total { get; set; }
}
