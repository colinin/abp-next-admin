using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Search;

/// <summary>
/// 搜索耗时明细
/// </summary>
public class SearchTookDetail
{
    /// <summary>
    /// 总耗时(毫秒)
    /// </summary>
    [JsonPropertyName("total")]
    public long Total { get; set; }

    /// <summary>
    /// 缓存耗时(毫秒)
    /// </summary>
    [JsonPropertyName("cache_took")]
    public long CacheTook { get; set; }

    /// <summary>
    /// 文件列表耗时(毫秒)
    /// </summary>
    [JsonPropertyName("file_list_took")]
    public long FileListTook { get; set; }

    /// <summary>
    /// 排队等待时间(毫秒)
    /// </summary>
    [JsonPropertyName("wait_in_queue")]
    public long WaitInQueue { get; set; }

    /// <summary>
    /// 索引耗时(毫秒)
    /// </summary>
    [JsonPropertyName("idx_took")]
    public long IdxTook { get; set; }

    /// <summary>
    /// 查询耗时(毫秒)
    /// </summary>
    [JsonPropertyName("search_took")]
    public long SearchTook { get; set; }
}
