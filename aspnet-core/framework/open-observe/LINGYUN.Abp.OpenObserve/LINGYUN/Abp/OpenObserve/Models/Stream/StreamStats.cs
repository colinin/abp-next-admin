using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 数据流统计信息
/// </summary>
public class StreamStats
{
    /// <summary>
    /// 数据流创建时间(微秒)
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    /// 记录的最小时间戳(微秒)
    /// </summary>
    [JsonPropertyName("doc_time_min")]
    public long DocTimeMin { get; set; }

    /// <summary>
    /// 记录的最大时间戳(微秒)
    /// </summary>
    [JsonPropertyName("doc_time_max")]
    public long DocTimeMax { get; set; }

    /// <summary>
    /// 记录数
    /// </summary>
    [JsonPropertyName("doc_num")]
    public long DocNum { get; set; }

    /// <summary>
    /// 存储文件数
    /// </summary>
    [JsonPropertyName("file_num")]
    public long FileNum { get; set; }

    /// <summary>
    /// 原始数据大小
    /// </summary>
    /// <remarks>
    /// 文档字段表标注为 int64, 但响应示例为小数(3323.5), 因此使用 double
    /// </remarks>
    [JsonPropertyName("storage_size")]
    public double StorageSize { get; set; }

    /// <summary>
    /// 压缩后大小
    /// </summary>
    [JsonPropertyName("compressed_size")]
    public double CompressedSize { get; set; }

    /// <summary>
    /// 索引大小
    /// </summary>
    [JsonPropertyName("index_size")]
    public double IndexSize { get; set; }
}
