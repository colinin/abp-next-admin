using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
public class WeChatWorkGetUnassignedListRequest : WeChatWorkRequest
{
    /// <summary>
    /// 分页查询游标，字符串类型，适用于数据量较大的情况，如果使用该参数则无需填写page_id，该参数由上一次调用返回
    /// </summary>
    [CanBeNull]
    [JsonProperty("cursor")]
    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }
    /// <summary>
    /// 每次返回的最大记录数，默认为1000，最大值为1000
    /// </summary>
    [CanBeNull]
    [JsonProperty("page_size")]
    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }
    public WeChatWorkGetUnassignedListRequest(string? cursor = null, int? pageSize = 1000)
    {
        Cursor = cursor;
        PageSize = pageSize;
    }
}
