using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Cluster;

/// <summary>
/// 集群信息响应
/// </summary>
/// <remarks>
/// 响应结构: {"regions":{"openobserve":{"zo1":{"pending_jobs":0}}}}
/// </remarks>
public class ClusterInfoResponse
{
    /// <summary>
    /// 按区域与节点组织的集群信息
    /// </summary>
    [JsonPropertyName("regions")]
    public Dictionary<string, Dictionary<string, ClusterNodeInfo>> Regions { get; set; } = [];

    /// <summary>
    /// 未在模型中声明的字段
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraProperties { get; set; }
}
