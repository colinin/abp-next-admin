using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Dashboard;

/// <summary>
/// 删除仪表盘面板响应
/// </summary>
public class DeleteDashboardPanelResponse
{
    /// <summary>
    /// 新的仪表盘哈希
    /// </summary>
    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    /// <summary>
    /// 已删除的面板标识
    /// </summary>
    [JsonPropertyName("panelId")]
    public string? PanelId { get; set; }
}
