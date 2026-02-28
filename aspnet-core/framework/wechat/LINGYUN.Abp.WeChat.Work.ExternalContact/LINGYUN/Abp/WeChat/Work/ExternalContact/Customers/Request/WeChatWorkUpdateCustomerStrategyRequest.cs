using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
/// <summary>
/// 编辑规则组及其管理范围请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94883#%E7%BC%96%E8%BE%91%E8%A7%84%E5%88%99%E7%BB%84%E5%8F%8A%E5%85%B6%E7%AE%A1%E7%90%86%E8%8C%83%E5%9B%B4" />
/// </remarks>
public class WeChatWorkUpdateCustomerStrategyRequest : WeChatWorkRequest
{
    /// <summary>
    /// 规则组id
    /// </summary>
    [NotNull]
    [JsonProperty("strategy_id")]
    [JsonPropertyName("strategy_id")]
    public int StrategyId { get; }
    /// <summary>
    /// 规则组名称
    /// </summary>
    [CanBeNull]
    [JsonProperty("strategy_name")]
    [JsonPropertyName("strategy_name")]
    public string? StrategyName { get; set; }
    /// <summary>
    /// 规则组管理员userid列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("admin_list")]
    [JsonPropertyName("admin_list")]
    public string[]? AdminList { get; set; }
    /// <summary>
    /// 规则组权限
    /// </summary>
    [CanBeNull]
    [JsonProperty("privilege")]
    [JsonPropertyName("privilege")]
    public CustomerStrategyPrivilege? Privilege { get; set; }
    /// <summary>
    /// 新增管理范围
    /// </summary>
    [NotNull]
    [JsonProperty("range_add")]
    [JsonPropertyName("range_add")]
    public List<CustomerStrategyRange> CreateRange { get; private set; }
    /// <summary>
    /// 删除管理范围
    /// </summary>
    [NotNull]
    [JsonProperty("range_del")]
    [JsonPropertyName("range_del")]
    public List<CustomerStrategyRange> DeleteRange { get; private set; }
    public WeChatWorkUpdateCustomerStrategyRequest(
        int strategyId, 
        string? strategyName = null, 
        string[]? adminList = null, 
        CustomerStrategyPrivilege? privilege = null)
    {
        Check.NotDefaultOrNull<int>(strategyId, nameof(strategyId));

        StrategyId = strategyId;
        StrategyName = strategyName;
        AdminList = adminList;
        Privilege = privilege;

        CreateRange = new List<CustomerStrategyRange>();
        DeleteRange = new List<CustomerStrategyRange>();
    }
}
