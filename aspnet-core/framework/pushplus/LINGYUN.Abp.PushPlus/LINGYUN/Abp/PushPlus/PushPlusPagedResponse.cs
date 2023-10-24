using Newtonsoft.Json;
using System.Collections.Generic;

namespace LINGYUN.Abp.PushPlus;

public class PushPlusPagedResponse<T>
{
    /// <summary>
    /// 当前页码
    /// </summary>
    [JsonProperty("pageNum")]
    public int PageNum { get; set; }
    /// <summary>
    /// 分页大小
    /// </summary>
    [JsonProperty("pageSize")]
    public int PageSize { get; set; }
    /// <summary>
    /// 总行数
    /// </summary>
    [JsonProperty("total")]
    public int Total { get; set; }
    /// <summary>
    /// 总页数
    /// </summary>
    [JsonProperty("pages")]
    public int Pages { get; set; }
    /// <summary>
    /// 消息列表
    /// </summary>
    [JsonProperty("list")]
    public List<T> Items { get; set; }

    public PushPlusPagedResponse()
    {
        Items = new List<T>();
    }
}
