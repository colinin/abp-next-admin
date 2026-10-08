using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models.Search;

public class SearchAgentOptions
{
    /// <summary>
    /// query execution mode: default (result cache path) or partition (streaming partition loop, see below)
    /// </summary>
    [JsonPropertyName("mode")]
    public string? Mode { get; }
    /// <summary>
    /// response format for hits: json, csv, or md_table
    /// </summary>
    [JsonPropertyName("output_format")]
    public string? OutputFormat { get; }
    public SearchAgentOptions(
        SearchAgentMode mode = SearchAgentMode.Default,
        SearchAgentOutputFormat outputFormat = SearchAgentOutputFormat.Json)
    {
        Mode = mode switch
        {
            SearchAgentMode.Default => "default",
            SearchAgentMode.Partition => "partition",
            _ => "default",
        };
        OutputFormat = outputFormat switch
        {
            SearchAgentOutputFormat.Json => "json",
            SearchAgentOutputFormat.Csv => "csv",
            SearchAgentOutputFormat.MdTable => "md_table",
            _ => "json",
        };
    }
}
