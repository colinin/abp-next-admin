using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Search;

/// <summary>
/// 搜索响应
/// </summary>
/// <remarks>
/// 参考: https://openobserve.ai/docs/reference/api/search/
/// </remarks>
public class SearchResponse<THit>
{
    /// <summary>
    /// 命中数据
    /// </summary>
    /// <remarks>
    /// 每条记录为摄取时的原始行, 并附加 _timestamp(微秒) 与 _p
    /// </remarks>
    [JsonPropertyName("hits")]
    public THit[] Hits { get; set; } = [];

    /// <summary>
    /// 命中总数
    /// </summary>
    [JsonPropertyName("total")]
    public long Total { get; set; }

    /// <summary>
    /// 偏移量
    /// </summary>
    /// <remarks>
    /// 取自 query.from
    /// </remarks>
    [JsonPropertyName("from")]
    public long From { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    /// <remarks>
    /// 取自 query.size
    /// </remarks>
    [JsonPropertyName("size")]
    public long Size { get; set; }

    /// <summary>
    /// 查询耗时
    /// </summary>
    /// <remarks>
    /// 单位: 毫秒
    /// </remarks>
    [JsonPropertyName("took")]
    public long Took { get; set; }

    /// <summary>
    /// 耗时明细
    /// </summary>
    [JsonPropertyName("took_detail")]
    public SearchTookDetail? TookDetail { get; set; }

    /// <summary>
    /// 扫描数据量
    /// </summary>
    /// <remarks>
    /// 单位: MB
    /// </remarks>
    [JsonPropertyName("scan_size")]
    public long ScanSize { get; set; }

    /// <summary>
    /// 扫描记录数
    /// </summary>
    [JsonPropertyName("scan_records")]
    public long ScanRecords { get; set; }

    /// <summary>
    /// 缓存命中比例
    /// </summary>
    [JsonPropertyName("result_cache_ratio")]
    public int? ResultCacheRatio { get; set; }

    /// <summary>
    /// 返回的列
    /// </summary>
    [JsonPropertyName("columns")]
    public string[]? Columns { get; set; }

    /// <summary>
    /// 链路标识
    /// </summary>
    [JsonPropertyName("trace_id")]
    public string? TraceId { get; set; }

    /// <summary>
    /// 函数执行错误
    /// </summary>
    [JsonPropertyName("function_error")]
    public string[]? FunctionError { get; set; }

    /// <summary>
    /// 是否为部分数据
    /// </summary>
    /// <remarks>
    /// 为 true 时表示响应基于部分数据
    /// </remarks>
    [JsonPropertyName("is_partial")]
    public bool IsPartial { get; set; }

    /// <summary>
    /// 直方图间隔(秒)
    /// </summary>
    [JsonPropertyName("histogram_interval")]
    public long? HistogramInterval { get; set; }

    /// <summary>
    /// 服务端调整后的开始时间(微秒)
    /// </summary>
    /// <remarks>
    /// 受数据流的 max_query_range 限制时会调整时间范围
    /// </remarks>
    [JsonPropertyName("new_start_time")]
    public long? NewStartTime { get; set; }

    /// <summary>
    /// 服务端调整后的结束时间(微秒)
    /// </summary>
    [JsonPropertyName("new_end_time")]
    public long? NewEndTime { get; set; }

    /// <summary>
    /// 排序方向
    /// </summary>
    /// <remarks>
    /// asc / desc
    /// </remarks>
    [JsonPropertyName("order_by")]
    public string? OrderBy { get; set; }

    /// <summary>
    /// 排序元数据
    /// </summary>
    [JsonPropertyName("order_by_metadata")]
    public string[][]? OrderByMetadata { get; set; }

    /// <summary>
    /// 响应类型
    /// </summary>
    [JsonPropertyName("response_type")]
    public string? ResponseType { get; set; }

    /// <summary>
    /// 结果格式
    /// </summary>
    /// <remarks>
    /// agent_options.output_format 为 csv/md_table 时返回, 如 csv / md_table / ndjson
    /// </remarks>
    [JsonPropertyName("format")]
    public string? Format { get; set; }

    /// <summary>
    /// 格式化后的结果
    /// </summary>
    /// <remarks>
    /// agent_options.output_format 为 csv/md_table 时, hits 为空, 数据在此字段
    /// </remarks>
    [JsonPropertyName("data")]
    public string? Data { get; set; }

    /// <summary>
    /// 提示信息
    /// </summary>
    [JsonPropertyName("advisory")]
    public string? Advisory { get; set; }

    /// <summary>
    /// 未在模型中声明的字段
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraProperties { get; set; }
}
