using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Report;

/// <summary>
/// 移动报表请求
/// </summary>
public class MoveReportsRequest
{
    /// <summary>
    /// 报表标识集合
    /// </summary>
    [JsonPropertyName("report_ids")]
    public string[] ReportIds { get; set; } = [];

    /// <summary>
    /// 目标文件夹
    /// </summary>
    [JsonPropertyName("dst_folder_id")]
    public string DestinationFolderId { get; set; } = default!;
}
