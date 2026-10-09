using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 按时间范围删除数据响应
/// </summary>
public class StreamDeleteJobResponse
{
    /// <summary>
    /// 删除任务标识
    /// </summary>
    /// <remarks>
    /// 用于查询删除进度
    /// </remarks>
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;
}
