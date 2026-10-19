using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Dashboard;

/// <summary>
/// 仪表盘面板操作响应
/// </summary>
public class DashboardPanelResponse
{
    /// <summary>
    /// 面板定义(含服务端生成的 id 与 layout)
    /// </summary>
    [JsonPropertyName("panel")]
    public JsonNode? Panel { get; set; }

    /// <summary>
    /// 新的仪表盘哈希
    /// </summary>
    /// <remarks>
    /// 用于链式操作的下一次请求
    /// </remarks>
    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    /// <summary>
    /// 页签标识
    /// </summary>
    [JsonPropertyName("tabId")]
    public string? TabId { get; set; }
}
