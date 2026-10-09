using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 数据流字段定义
/// </summary>
public class StreamSchemaField
{
    /// <summary>
    /// 字段名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 字段类型
    /// </summary>
    /// <remarks>
    /// Utf8 / Int64 / Float64 / Timestamp / Boolean
    /// </remarks>
    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;
}
