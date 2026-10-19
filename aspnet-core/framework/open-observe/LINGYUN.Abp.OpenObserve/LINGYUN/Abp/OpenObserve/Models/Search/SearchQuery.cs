using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Search;

/// <summary>
/// 搜索查询条件
/// </summary>
public class SearchQuery
{
    /// <summary>
    /// SQL 查询语句
    /// </summary>
    /// <remarks>
    /// 使用 SQL 过滤数据, 默认按 _timestamp 排序, 可用 order by 覆盖,
    /// 使用 from/size 做分页
    /// </remarks>
    [JsonPropertyName("sql")]
    public string Sql { get; set; }

    /// <summary>
    /// 开始时间(微秒)
    /// </summary>
    /// <remarks>
    /// 必须提供, 否则会全量扫描
    /// </remarks>
    [JsonPropertyName("start_time")]
    public long StartTime { get; set; }

    /// <summary>
    /// 结束时间(微秒)
    /// </summary>
    [JsonPropertyName("end_time")]
    public long EndTime { get; set; }

    /// <summary>
    /// 偏移量
    /// </summary>
    [JsonPropertyName("from")]
    public long From { get; set; }

    /// <summary>
    /// 返回条数
    /// </summary>
    [JsonPropertyName("size")]
    public long Size { get; set; }

    /// <summary>
    /// 快速模式
    /// </summary>
    /// <remarks>
    /// 仅扫描 Interesting Fields, 需服务端开启 ZO_QUICK_MODE_ENABLED
    /// </remarks>
    [JsonPropertyName("quick_mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool QuickMode { get; set; }

    /// <summary>
    /// 查询类型标签
    /// </summary>
    [JsonPropertyName("query_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? QueryType { get; set; }

    /// <summary>
    /// 是否统计命中总数
    /// </summary>
    [JsonPropertyName("track_total_hits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool TrackTotalHits { get; set; }

    /// <summary>
    /// 是否使用函数
    /// </summary>
    [JsonPropertyName("uses_zo_fn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool UsesZoFn { get; set; }

    /// <summary>
    /// 应用于结果的函数名称(VRL/JS)
    /// </summary>
    [JsonPropertyName("query_fn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? QueryFn { get; set; }

    /// <summary>
    /// 是否跳过 WAL
    /// </summary>
    [JsonPropertyName("skip_wal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool SkipWal { get; set; }

    /// <summary>
    /// 是否流式输出
    /// </summary>
    [JsonPropertyName("streaming_output")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool StreamingOutput { get; set; }

    /// <summary>
    /// 流式标识
    /// </summary>
    [JsonPropertyName("streaming_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StreamingId { get; set; }

    /// <summary>
    /// 直方图间隔(秒)
    /// </summary>
    [JsonPropertyName("histogram_interval")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long HistogramInterval { get; set; }

    /// <summary>
    /// 时区
    /// </summary>
    /// <remarks>
    /// 固定偏移, 如 +08:00, 应用于未指定第三个参数的 histogram() 分桶
    /// </remarks>
    [JsonPropertyName("timezone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Timezone { get; set; }

    public SearchQuery(
        string sql,
        DateTime startTime,
        DateTime endTime,
        long from = 0,
        long size = 10)
    {
        Sql = sql;
        StartTime = startTime.ToMicroseconds();
        EndTime = endTime.ToMicroseconds();
        From = from;
        Size = size;
    }

    /// <summary>
    /// 设置时间范围
    /// </summary>
    public virtual SearchQuery WithTimeRange(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime.ToMicroseconds();
        EndTime = endTime.ToMicroseconds();

        return this;
    }

    /// <summary>
    /// 设置分页
    /// </summary>
    public virtual SearchQuery WithPaging(long from, long size)
    {
        From = from;
        Size = size;

        return this;
    }
}
