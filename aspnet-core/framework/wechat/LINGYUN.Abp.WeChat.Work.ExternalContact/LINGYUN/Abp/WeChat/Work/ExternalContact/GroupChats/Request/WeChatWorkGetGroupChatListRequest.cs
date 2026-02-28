using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Request;
/// <summary>
/// 获取客户群列表请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92120" />
/// </remarks>
public class WeChatWorkGetGroupChatListRequest : WeChatWorkRequest
{
    /// <summary>
    /// 客户群跟进状态过滤
    /// </summary>
    [CanBeNull]
    [JsonProperty("status_filter")]
    [JsonPropertyName("status_filter")]
    public StatusFilter? StatusFilter { get; set; }
    /// <summary>
    /// 客户群跟进状态过滤
    /// </summary>
    [CanBeNull]
    [JsonProperty("owner_filter")]
    [JsonPropertyName("owner_filter")]
    public OwnerFilter[]? OwnerFilter { get; set; }
    /// <summary>
    /// 用于分页查询的游标，字符串类型，由上一次调用返回，首次调用不填
    /// </summary>
    [CanBeNull]
    [JsonProperty("cursor")]
    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }
    /// <summary>
    /// 分页，预期请求的数据量，取值范围 1 ~ 1000
    /// </summary>
    [CanBeNull]
    [JsonProperty("limit")]
    [JsonPropertyName("limit")]
    public int Limit { get; }
    public WeChatWorkGetGroupChatListRequest(int limit = 1000)
    {
        Check.Range(limit, nameof(limit), 1, 1000);

        Limit = limit;
    }
}
