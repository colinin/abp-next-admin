using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
/// <summary>
/// 获取规则组管理范围请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94883#%E8%8E%B7%E5%8F%96%E8%A7%84%E5%88%99%E7%BB%84%E7%AE%A1%E7%90%86%E8%8C%83%E5%9B%B4" />
/// </remarks>
public class WeChatWorkGetCustomerStrategyRangeRequest : WeChatWorkRequest
{
    /// <summary>
    /// 规则组id
    /// </summary>
    [NotNull]
    [JsonProperty("strategy_id")]
    [JsonPropertyName("strategy_id")]
    public int StrategyId { get; }
    /// <summary>
    /// 分页查询游标，首次调用可不填
    /// </summary>
    [CanBeNull]
    [JsonProperty("cursor")]
    [JsonPropertyName("cursor")]
    public string? Cursor { get; }
    /// <summary>
    /// 每个分页的成员/部门节点数，默认为1000，最大为1000
    /// </summary>
    [CanBeNull]
    [JsonProperty("limit")]
    [JsonPropertyName("limit")]
    public int? Limit { get; }
    public WeChatWorkGetCustomerStrategyRangeRequest(int strategyId, string? cursor = null, int? limit = 1000)
    {
        if (limit.HasValue)
        {
            Check.Range(limit.Value, nameof(limit), 1, 1000);
        }

        StrategyId = strategyId;
        Cursor = cursor;
        Limit = limit;
    }
}
