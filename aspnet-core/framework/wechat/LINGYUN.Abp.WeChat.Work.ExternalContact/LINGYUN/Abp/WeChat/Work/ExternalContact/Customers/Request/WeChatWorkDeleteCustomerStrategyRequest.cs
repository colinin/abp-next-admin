using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
/// <summary>
/// 删除规则组请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94883#%E5%88%A0%E9%99%A4%E8%A7%84%E5%88%99%E7%BB%84" />
/// </remarks>
public class WeChatWorkDeleteCustomerStrategyRequest : WeChatWorkRequest
{
    /// <summary>
    /// 规则组id
    /// </summary>
    [NotNull]
    [JsonProperty("strategy_id")]
    [JsonPropertyName("strategy_id")]
    public int StrategyId { get; }
    public WeChatWorkDeleteCustomerStrategyRequest(int strategyId)
    {
        Check.NotDefaultOrNull<int>(strategyId, nameof(strategyId));

        StrategyId = strategyId;
    }
}
