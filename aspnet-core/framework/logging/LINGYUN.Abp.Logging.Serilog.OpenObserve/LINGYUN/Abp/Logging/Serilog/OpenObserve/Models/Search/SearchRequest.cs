using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models.Search;

public class SearchRequest
{
    /// <summary>
    /// query params
    /// </summary>
    [JsonPropertyName("query")]
    public SearchQuery Query { get; }
    /// <summary>
    /// default is empty, support: ui, dashboards, reports, alerts
    /// </summary>
    [JsonPropertyName("search_type")]
    public string? SearchType { get; }
    /// <summary>
    /// default value based on ZO_QUERY_TIMEOUT=600
    /// </summary>
    [JsonPropertyName("timeout")]
    public int? Timeout { get; }
    /// <summary>
    /// options for agent/MCP clients
    /// </summary>
    [JsonPropertyName("agent_options")]
    public SearchAgentOptions? AgentOptions { get; }
    public SearchRequest(
        SearchQuery query, 
        SearchType? searchType = null, 
        int? timeout = OpenObserveConstants.ZO_QUERY_TIMEOUT, 
        SearchAgentOptions? agentOptions = null)
    {
        Query = query;
        Timeout = timeout;
        AgentOptions = agentOptions;
        if (searchType != null)
        {
            SearchType = searchType switch
            {
                Search.SearchType.UI => "ui",
                Search.SearchType.Dashboards => "dashboards",
                Search.SearchType.Reports => "reports",
                Search.SearchType.Alerts => "alerts",
                _ => null,
            };
        }
    }
}
