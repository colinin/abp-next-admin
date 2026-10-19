using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 删除任务执行明细
/// </summary>
public class StreamDeleteJobMetadata
{
    [JsonPropertyName("cluster")]
    public string? Cluster { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// 创建时间(微秒)
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    /// 结束时间(微秒), 0 表示仍在进行
    /// </summary>
    [JsonPropertyName("ended_at")]
    public long EndedAt { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
