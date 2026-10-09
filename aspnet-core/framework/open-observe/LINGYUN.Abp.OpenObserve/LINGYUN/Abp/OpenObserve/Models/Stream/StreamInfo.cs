using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 数据流信息
/// </summary>
public class StreamInfo
{
    /// <summary>
    /// 数据流名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 存储类型
    /// </summary>
    /// <remarks>
    /// s3 / disk
    /// </remarks>
    [JsonPropertyName("storage_type")]
    public string? StorageType { get; set; }

    /// <summary>
    /// 数据流类型
    /// </summary>
    /// <remarks>
    /// logs / metrics / traces
    /// </remarks>
    [JsonPropertyName("stream_type")]
    public string? StreamType { get; set; }

    /// <summary>
    /// 统计信息
    /// </summary>
    [JsonPropertyName("stats")]
    public StreamStats? Stats { get; set; }

    /// <summary>
    /// 字段定义
    /// </summary>
    /// <remarks>
    /// 查询参数 fetchSchema 为 false 时不返回
    /// </remarks>
    [JsonPropertyName("schema")]
    public StreamSchemaField[]? Schema { get; set; }

    /// <summary>
    /// 数据流设置
    /// </summary>
    [JsonPropertyName("settings")]
    public StreamSettingsInfo? Settings { get; set; }
}
