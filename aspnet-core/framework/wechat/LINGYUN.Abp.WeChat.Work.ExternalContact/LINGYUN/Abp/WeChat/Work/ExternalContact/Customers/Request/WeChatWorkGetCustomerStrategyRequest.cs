using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
/// <summary>
/// 获取规则组请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94883#%E8%8E%B7%E5%8F%96%E8%A7%84%E5%88%99%E7%BB%84%E8%AF%A6%E6%83%85" />
/// </remarks>
public class WeChatWorkGetCustomerStrategyRequest : WeChatWorkRequest
{
    /// <summary>
    /// 规则组id
    /// </summary>
    [NotNull]
    [JsonProperty("strategy_id")]
    [JsonPropertyName("strategy_id")]
    public int StrategyId { get; }
    public WeChatWorkGetCustomerStrategyRequest(int strategyId)
    {
        StrategyId = strategyId;
    }
}
