using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Report;

/// <summary>
/// 调度频率
/// </summary>
public class ReportFrequency
{
    /// <summary>
    /// 频率类型
    /// </summary>
    /// <remarks>
    /// once / hours / days / weeks / months / cron
    /// </remarks>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// 间隔
    /// </summary>
    [JsonPropertyName("interval")]
    public int? Interval { get; set; }

    /// <summary>
    /// cron 表达式
    /// </summary>
    [JsonPropertyName("cron")]
    public string? Cron { get; set; }

    /// <summary>
    /// 是否对齐时间
    /// </summary>
    [JsonPropertyName("align_time")]
    public bool? AlignTime { get; set; }
}
