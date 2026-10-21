using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Dashboard;

/// <summary>
/// 新增/更新仪表盘面板请求
/// </summary>
/// <remarks>
/// panel 为 v8 面板对象, 文档未给出完整 schema, 因此使用自由 JSON
/// 新增时 layout 可省略(自动计算), 更新时省略 layout 表示保留原布局
/// </remarks>
public class SaveDashboardPanelRequest
{
    /// <summary>
    /// 面板定义
    /// </summary>
    [JsonPropertyName("panel")]
    public JsonNode? Panel { get; set; }

    /// <summary>
    /// 页签标识
    /// </summary>
    /// <remarks>
    /// 默认第一个页签
    /// </remarks>
    [JsonPropertyName("tabId")]
    public string? TabId { get; set; }
}
