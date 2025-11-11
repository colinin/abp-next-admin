using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 规则组详情
/// </summary>
public class CustomerStrategyInfo : CustomerStrategy
{
    /// <summary>
    /// 父规则组id， 如果当前规则组没父规则组，则为0
    /// </summary>
    [NotNull]
    [JsonProperty("parent_id")]
    [JsonPropertyName("parent_id")]
    public int ParentId { get; set; }
    /// <summary>
    /// 规则组名称
    /// </summary>
    [NotNull]
    [JsonProperty("strategy_name")]
    [JsonPropertyName("strategy_name")]
    public string StrategyName { get; set; }
    /// <summary>
    /// 规则组创建时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("create_time")]
    [JsonPropertyName("create_time")]
    public long CreateTime { get; set; }
    /// <summary>
    /// 规则组管理员userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("admin_list")]
    [JsonPropertyName("admin_list")]
    public string[] AdminList { get; set; }
    /// <summary>
    /// 规则组权限
    /// </summary>
    [NotNull]
    [JsonProperty("privilege")]
    [JsonPropertyName("privilege")]
    public CustomerStrategyPrivilege Privilege { get; set; }
}
