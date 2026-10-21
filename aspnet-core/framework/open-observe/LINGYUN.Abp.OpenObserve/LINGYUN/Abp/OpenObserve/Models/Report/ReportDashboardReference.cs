using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Report;

/// <summary>
/// 报表关联的仪表盘
/// </summary>
public class ReportDashboardReference
{
    /// <summary>
    /// 仪表盘标识
    /// </summary>
    [JsonPropertyName("dashboard")]
    public string Dashboard { get; set; } = default!;

    /// <summary>
    /// 仪表盘所在文件夹
    /// </summary>
    [JsonPropertyName("folder")]
    public string? Folder { get; set; }

    /// <summary>
    /// 页签
    /// </summary>
    [JsonPropertyName("tabs")]
    public string[]? Tabs { get; set; }

    /// <summary>
    /// 变量
    /// </summary>
    [JsonPropertyName("variables")]
    public JsonNode? Variables { get; set; }

    /// <summary>
    /// 时间范围
    /// </summary>
    [JsonPropertyName("timerange")]
    public ReportTimeRange? TimeRange { get; set; }
}
