using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WxPusher;

[Serializable]
public class WxPusherPagedResult<T>
{
    /// <summary>
    /// 总数
    /// </summary>
    [JsonProperty("total")]
    public int Total { get; set; }
    /// <summary>
    /// 当前页码
    /// </summary>
    [JsonProperty("page")]
    public int Page { get; set; }
    /// <summary>
    /// 页码大小
    /// </summary>
    [JsonProperty("pageSize")]
    public int PageSize { get; set; }
    /// <summary>
    /// 记录列表
    /// </summary>
    [JsonProperty("records")]
    public List<T> Records { get; set; } = new List<T>();
}
