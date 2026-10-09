using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 按时间范围删除数据任务状态
/// </summary>
public class StreamDeleteJobStatusResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    /// <summary>
    /// 任务状态
    /// </summary>
    /// <remarks>
    /// Completed / Pending
    /// </remarks>
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;

    /// <summary>
    /// 各集群/区域的执行明细
    /// </summary>
    [JsonPropertyName("metadata")]
    public StreamDeleteJobMetadata[]? Metadata { get; set; }

    /// <summary>
    /// 执行错误
    /// </summary>
    [JsonPropertyName("errors")]
    public StreamDeleteJobError[]? Errors { get; set; }
}
