using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Report;

/// <summary>
/// 批量删除报表请求
/// </summary>
public class BulkDeleteReportsRequest
{
    [JsonPropertyName("ids")]
    public string[] Ids { get; set; } = [];
}
