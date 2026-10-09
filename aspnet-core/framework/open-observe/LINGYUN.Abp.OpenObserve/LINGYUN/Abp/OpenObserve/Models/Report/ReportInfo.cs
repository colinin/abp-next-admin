using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Report;

/// <summary>
/// 报表信息
/// </summary>
public class ReportInfo
{
    /// <summary>
    /// 报表标识(KSUID)
    /// </summary>
    [JsonPropertyName("report_id")]
    public string? ReportId { get; set; }

    /// <summary>
    /// 报表名称
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 展示标题
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// 所属文件夹
    /// </summary>
    [JsonPropertyName("folder_id")]
    public string? FolderId { get; set; }

    /// <summary>
    /// 关联的仪表盘
    /// </summary>
    /// <remarks>
    /// 列表接口返回字符串数组, 创建/更新接口为对象数组, 因此使用自由 JSON
    /// </remarks>
    [JsonPropertyName("dashboards")]
    public JsonNode? Dashboards { get; set; }

    /// <summary>
    /// 调度频率
    /// </summary>
    [JsonPropertyName("frequency")]
    public ReportFrequency? Frequency { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// 所有者
    /// </summary>
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    /// <summary>
    /// 上次触发时间(微秒)
    /// </summary>
    [JsonPropertyName("last_triggered_at")]
    public long? LastTriggeredAt { get; set; }

    /// <summary>
    /// 未在模型中声明的字段
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraProperties { get; set; }
}
