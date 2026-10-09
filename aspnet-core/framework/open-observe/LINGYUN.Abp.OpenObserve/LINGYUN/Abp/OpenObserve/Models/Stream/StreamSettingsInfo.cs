using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 数据流设置(响应)
/// </summary>
public class StreamSettingsInfo
{
    /// <summary>
    /// 分区字段
    /// </summary>
    [JsonPropertyName("partition_keys")]
    public Dictionary<string, object?>? PartitionKeys { get; set; }

    /// <summary>
    /// 全文检索字段
    /// </summary>
    [JsonPropertyName("full_text_search_keys")]
    public string[]? FullTextSearchKeys { get; set; }
}
