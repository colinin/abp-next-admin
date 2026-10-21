using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Function;

/// <summary>
/// 函数与数据流的关联
/// </summary>
public class FunctionStreamInfo
{
    /// <summary>
    /// 数据流名称
    /// </summary>
    [JsonPropertyName("stream")]
    public string? Stream { get; set; }

    /// <summary>
    /// 执行顺序, 越小越先执行
    /// </summary>
    [JsonPropertyName("order")]
    public int? Order { get; set; }

    /// <summary>
    /// 数据流类型
    /// </summary>
    [JsonPropertyName("streamType")]
    public string? StreamType { get; set; }

    /// <summary>
    /// 是否移除
    /// </summary>
    [JsonPropertyName("isRemoved")]
    public bool? IsRemoved { get; set; }

    /// <summary>
    /// 是否在扁平化之前执行
    /// </summary>
    [JsonPropertyName("applyBeforeFlattening")]
    public bool? ApplyBeforeFlattening { get; set; }
}
