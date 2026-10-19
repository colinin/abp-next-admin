using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 更新数据流设置请求
/// </summary>
/// <remarks>
/// PUT /api/{org_id}/streams/{stream_name}/settings
/// 支持 add/set/remove 的字段使用包装语法
/// </remarks>
public class UpdateStreamSettingRequest
{
    [JsonPropertyName("partition_keys")]
    public UpdateStreamSettingValue<string>? PartitionKeys { get; set; }

    [JsonPropertyName("index_fields")]
    public UpdateStreamSettingValue<string>? IndexFields { get; set; }

    [JsonPropertyName("full_text_search_keys")]
    public UpdateStreamSettingValue<string>? FullTextSearchKeys { get; set; }

    [JsonPropertyName("bloom_filter_fields")]
    public UpdateStreamSettingValue<string>? BloomFilterFields { get; set; }

    [JsonPropertyName("defined_schema_fields")]
    public UpdateStreamSettingValue<string>? DefinedSchemaFields { get; set; }

    [JsonPropertyName("extended_retention_days")]
    public UpdateStreamSettingValue<object>? ExtendedRetentionDays { get; set; }

    [JsonPropertyName("data_retention")]
    public int? DataRetention { get; set; }

    [JsonPropertyName("flatten_level")]
    public int? FlattenLevel { get; set; }

    [JsonPropertyName("max_query_range")]
    public int? MaxQueryRange { get; set; }

    [JsonPropertyName("store_original_data")]
    public bool? StoreOriginalData { get; set; }

    [JsonPropertyName("approx_partition")]
    public bool? ApproxPartition { get; set; }

    [JsonPropertyName("index_original_data")]
    public bool? IndexOriginalData { get; set; }

    [JsonPropertyName("index_all_values")]
    public bool? IndexAllValues { get; set; }

    [JsonPropertyName("storage_type")]
    public string? StorageType { get; set; }
}
