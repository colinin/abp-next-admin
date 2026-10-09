using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 数据流列表响应
/// </summary>
public class StreamListResponse
{
    [JsonPropertyName("list")]
    public StreamInfo[] List { get; set; } = [];
}
