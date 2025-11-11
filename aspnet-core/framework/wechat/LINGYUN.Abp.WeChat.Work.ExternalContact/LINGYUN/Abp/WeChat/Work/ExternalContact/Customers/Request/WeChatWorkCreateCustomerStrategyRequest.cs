using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
/// <summary>
/// 批量获取客户详情请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92994" />
/// </remarks>
public class WeChatWorkCreateCustomerStrategyRequest : WeChatWorkRequest
{
    /// <summary>
    /// 父规则组id
    /// </summary>
    [CanBeNull]
    [JsonProperty("parent_id")]
    [JsonPropertyName("parent_id")]
    public int? ParentId { get; set; }
    /// <summary>
    /// 规则组名称
    /// </summary>
    [NotNull]
    [JsonProperty("strategy_name")]
    [JsonPropertyName("strategy_name")]
    public string StrategyName { get; }
    /// <summary>
    /// 规则组管理员userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("admin_list")]
    [JsonPropertyName("admin_list")]
    public string[] AdminList { get; }
    /// <summary>
    /// 规则组权限
    /// </summary>
    [NotNull]
    [JsonProperty("privilege")]
    [JsonPropertyName("privilege")]
    public CustomerStrategyPrivilege Privilege { get; }
    /// <summary>
    /// 规则组管理范围
    /// </summary>
    [NotNull]
    [JsonProperty("range")]
    [JsonPropertyName("range")]
    public CustomerStrategyRange[] Range { get; }
    public WeChatWorkCreateCustomerStrategyRequest(
        string strategyName,
        string[] adminList,
        CustomerStrategyPrivilege? privilege = null,
        CustomerStrategyRange[]? range = null)
    {
        Check.NotNullOrWhiteSpace(strategyName, nameof(strategyName));
        Check.NotNullOrEmpty(adminList, nameof(adminList));

        if (adminList.Length > 20)
        {
            throw new ArgumentException("Up to 20 admin list can be configured at a time!");
        }
        if (range != null && range.Length > 100)
        {
            throw new ArgumentException("Up to 100 management range can be configured at a time!");
        }

        StrategyName = strategyName;
        AdminList = adminList;
        Privilege = privilege ?? CustomerStrategyPrivilege.Default();
    }
}
