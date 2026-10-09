using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Ingestion;

/// <summary>
/// _bulk 单条记录处理结果
/// </summary>
public class JsonBulkItem
{
    [JsonPropertyName("_index")]
    public string? Index { get; set; }

    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    /// <remarks>
    /// 兼容 Elasticsearch 的占位值, 不代表真实版本
    /// </remarks>
    [JsonPropertyName("_version")]
    public int Version { get; set; }

    /// <summary>
    /// 处理结果
    /// </summary>
    /// <remarks>
    /// 如 created
    /// </remarks>
    [JsonPropertyName("result")]
    public string? Result { get; set; }

    /// <summary>
    /// 状态码
    /// </summary>
    /// <remarks>
    /// 200 表示成功, >=400 表示失败
    /// </remarks>
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    [JsonPropertyName("error")]
    public JsonBulkItemError? Error { get; set; }

    /// <summary>
    /// 原始记录
    /// </summary>
    /// <remarks>
    /// 仅在失败时返回
    /// </remarks>
    [JsonPropertyName("originalRecord")]
    public JsonElement? OriginalRecord { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => Status is >= 200 and < 300;
}
