using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 创建数据流设置请求
/// </summary>
/// <remarks>
/// POST /api/{org_id}/streams/{stream_name}/settings, 所有字段可选
/// </remarks>
public class StreamSettingRequest
{
    /// <summary>
    /// 分区字段
    /// </summary>
    [JsonPropertyName("partition_keys")]
    public string[]? PartitionKeys { get; set; }

    /// <summary>
    /// 二级索引字段
    /// </summary>
    [JsonPropertyName("index_fields")]
    public string[]? IndexFields { get; set; }

    /// <summary>
    /// 全文检索字段
    /// </summary>
    [JsonPropertyName("full_text_search_keys")]
    public string[]? FullTextSearchKeys { get; set; }

    /// <summary>
    /// 布隆过滤器字段
    /// </summary>
    [JsonPropertyName("bloom_filter_fields")]
    public string[]? BloomFilterFields { get; set; }

    /// <summary>
    /// 数据保留天数
    /// </summary>
    /// <remarks>
    /// 最小 3 天, compliance 流最小 30 天
    /// </remarks>
    [JsonPropertyName("data_retention")]
    public int? DataRetention { get; set; }

    /// <summary>
    /// 扁平化层级
    /// </summary>
    [JsonPropertyName("flatten_level")]
    public int? FlattenLevel { get; set; }

    /// <summary>
    /// 保留的自定义 schema 字段
    /// </summary>
    [JsonPropertyName("defined_schema_fields")]
    public string[]? DefinedSchemaFields { get; set; }

    /// <summary>
    /// 单次查询最大时间范围(小时)
    /// </summary>
    [JsonPropertyName("max_query_range")]
    public int? MaxQueryRange { get; set; }

    /// <summary>
    /// 是否存储原始数据
    /// </summary>
    [JsonPropertyName("store_original_data")]
    public bool? StoreOriginalData { get; set; }

    /// <summary>
    /// 是否使用均分时间范围分区
    /// </summary>
    [JsonPropertyName("approx_partition")]
    public bool? ApproxPartition { get; set; }

    /// <summary>
    /// 延长保留期的时间范围
    /// </summary>
    /// <remarks>
    /// 文档未说明元素的具体结构
    /// </remarks>
    [JsonPropertyName("extended_retention_days")]
    public object[]? ExtendedRetentionDays { get; set; }

    /// <summary>
    /// 是否对原始日志体建立全文索引
    /// </summary>
    [JsonPropertyName("index_original_data")]
    public bool? IndexOriginalData { get; set; }

    /// <summary>
    /// 是否为所有字段建立精确匹配索引
    /// </summary>
    [JsonPropertyName("index_all_values")]
    public bool? IndexAllValues { get; set; }

    /// <summary>
    /// 存储类型
    /// </summary>
    /// <remarks>
    /// normal(默认) / compliance
    /// </remarks>
    [JsonPropertyName("storage_type")]
    public string? StorageType { get; set; }
}
