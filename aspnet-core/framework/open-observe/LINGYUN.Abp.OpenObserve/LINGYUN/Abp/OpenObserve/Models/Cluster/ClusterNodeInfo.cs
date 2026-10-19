using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Cluster;

/// <summary>
/// 集群节点信息
/// </summary>
public class ClusterNodeInfo
{
    /// <summary>
    /// 待处理的压缩任务数
    /// </summary>
    [JsonPropertyName("pending_jobs")]
    public int PendingJobs { get; set; }

    /// <summary>
    /// 未在模型中声明的字段
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraProperties { get; set; }
}
