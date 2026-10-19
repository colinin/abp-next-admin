using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Report;

/// <summary>
/// 启用/禁用报表响应
/// </summary>
public class EnableReportResponse
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}
