using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
/// <summary>
/// 获取规则组列表请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94883#%E8%8E%B7%E5%8F%96%E8%A7%84%E5%88%99%E7%BB%84%E5%88%97%E8%A1%A8" />
/// </remarks>
public class WeChatWorkGetCustomerStrategyListRequest : WeChatWorkRequest
{
    /// <summary>
    /// 分页查询游标，首次调用可不填
    /// </summary>
    [CanBeNull]
    [JsonProperty("cursor")]
    [JsonPropertyName("cursor")]
    public string? Cursor { get; }
    /// <summary>
    /// 分页大小,默认为1000，最大不超过1000
    /// </summary>
    [CanBeNull]
    [JsonProperty("limit")]
    [JsonPropertyName("limit")]
    public int? Limit { get; }
    public WeChatWorkGetCustomerStrategyListRequest(string? cursor = null, int? limit = 1000)
    {
        if (limit.HasValue)
        {
            Check.Range(limit.Value, nameof(limit), 1, 1000);
        }

        Cursor = cursor;
        Limit = limit;
    }
}
