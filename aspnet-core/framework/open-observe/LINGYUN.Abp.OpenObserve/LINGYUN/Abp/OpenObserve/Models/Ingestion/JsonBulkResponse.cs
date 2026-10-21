using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Ingestion;

/// <summary>
/// _bulk 响应
/// </summary>
/// <remarks>
/// Elasticsearch _bulk 兼容结构
/// {"took":0,"errors":false,"items":[{"index":{"_index":"stream1","_id":"..","status":200,"result":"created"}}]}
/// </remarks>
public class JsonBulkResponse
{
    /// <summary>
    /// 耗时
    /// </summary>
    /// <remarks>
    /// OpenObserve 固定返回 0, 不填充实际耗时
    /// </remarks>
    [JsonPropertyName("took")]
    public long Took { get; set; }

    /// <summary>
    /// 是否存在失败项
    /// </summary>
    [JsonPropertyName("errors")]
    public bool Errors { get; set; }

    /// <summary>
    /// 每条记录的处理结果
    /// </summary>
    /// <remarks>
    /// 每个元素的键为提交时使用的动作(index/create/update)
    /// </remarks>
    [JsonPropertyName("items")]
    public Dictionary<string, JsonBulkItem>[] Items { get; set; } = [];
}
