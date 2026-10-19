using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Report;

/// <summary>
/// 报表时间范围
/// </summary>
public class ReportTimeRange
{
    /// <summary>
    /// 类型
    /// </summary>
    /// <remarks>
    /// 如 relative
    /// </remarks>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// 周期
    /// </summary>
    /// <remarks>
    /// 如 1w
    /// </remarks>
    [JsonPropertyName("period")]
    public string? Period { get; set; }

    [JsonPropertyName("from")]
    public long From { get; set; }

    [JsonPropertyName("to")]
    public long To { get; set; }
}
