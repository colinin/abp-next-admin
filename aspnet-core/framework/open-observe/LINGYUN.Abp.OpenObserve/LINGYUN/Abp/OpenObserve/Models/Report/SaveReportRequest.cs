using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Report;

/// <summary>
/// 创建/更新报表请求
/// </summary>
public class SaveReportRequest
{
    /// <summary>
    /// 报表名称(组织内唯一)
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 展示标题
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// 组织名称(必填)
    /// </summary>
    [JsonPropertyName("org_id")]
    public string Organization { get; set; } = default!;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// 报表邮件正文
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("frequency")]
    public ReportFrequency? Frequency { get; set; }

    /// <summary>
    /// 生成起始时间(微秒)
    /// </summary>
    [JsonPropertyName("start")]
    public long? Start { get; set; }

    /// <summary>
    /// 时区名称
    /// </summary>
    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    /// <summary>
    /// 固定时区偏移(分钟)
    /// </summary>
    [JsonPropertyName("timezoneOffset")]
    public int? TimezoneOffset { get; set; }

    /// <summary>
    /// 关联的仪表盘(必填)
    /// </summary>
    [JsonPropertyName("dashboards")]
    public ReportDashboardReference[] Dashboards { get; set; } = [];

    /// <summary>
    /// 投递目标(必填)
    /// </summary>
    [JsonPropertyName("destinations")]
    public ReportDestination[] Destinations { get; set; } = [];
}
