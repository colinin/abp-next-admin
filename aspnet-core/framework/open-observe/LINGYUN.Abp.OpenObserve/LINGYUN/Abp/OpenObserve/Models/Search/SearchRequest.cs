using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Search;

/// <summary>
/// 搜索请求
/// </summary>
public class SearchRequest
{
    /// <summary>
    /// 查询条件
    /// </summary>
    [JsonPropertyName("query")]
    public SearchQuery Query { get; }

    /// <summary>
    /// 搜索来源
    /// </summary>
    /// <remarks>
    /// 支持 ui / dashboards / reports / alerts 等, 请求体中的值优先于查询字符串
    /// </remarks>
    [JsonPropertyName("search_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SearchTypeString { get; set; }

    /// <summary>
    /// 超时时间(秒)
    /// </summary>
    /// <remarks>
    /// 默认值由服务端 ZO_QUERY_TIMEOUT=600 决定
    /// </remarks>
    [JsonPropertyName("timeout")]
    public int? Timeout { get; }

    /// <summary>
    /// agent/MCP 客户端选项
    /// </summary>
    [JsonPropertyName("agent_options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SearchAgentOptions? AgentOptions { get; }

    /// <summary>
    /// SQL 编码方式
    /// </summary>
    /// <remarks>
    /// 为空或 base64, 为 base64 时 query.sql 需 URL Base64 编码, 服务端解码后置空
    /// </remarks>
    [JsonPropertyName("encoding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Encoding { get; set; }

    /// <summary>
    /// 查询的区域
    /// </summary>
    /// <remarks>
    /// 默认查询所有区域, local 表示仅本地区域
    /// </remarks>
    [JsonPropertyName("regions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Regions { get; set; }

    /// <summary>
    /// 查询的集群
    /// </summary>
    /// <remarks>
    /// local 表示仅本地集群
    /// </remarks>
    [JsonPropertyName("clusters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Clusters { get; set; }

    /// <summary>
    /// 是否使用缓存
    /// </summary>
    /// <remarks>
    /// 查询字符串中的 use_cache 会覆盖该值
    /// </remarks>
    [JsonPropertyName("use_cache")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseCache { get; set; }

    /// <summary>
    /// 是否清理缓存
    /// </summary>
    /// <remarks>
    /// 查询字符串中的 clear_cache 会覆盖该值
    /// </remarks>
    [JsonPropertyName("clear_cache")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ClearCache { get; set; }

    public SearchRequest(
        SearchQuery query,
        SearchType? searchType = null,
        int? timeout = OpenObserveConstants.ZO_QUERY_TIMEOUT,
        SearchAgentOptions? agentOptions = null)
    {
        Query = query;
        Timeout = timeout;
        AgentOptions = agentOptions;
        SearchTypeString = searchType?.AsString();
    }
}
