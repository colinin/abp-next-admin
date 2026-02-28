using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 规则组管理范围
/// </summary>
public class CustomerStrategyRange
{
    /// <summary>
    /// 节点类型
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public CustomerStrategyRangeType Type { get; set; }
    /// <summary>
    /// 管理范围内配置的成员userid，仅 Type为<see cref="CustomerStrategyRangeType.Member"/> 时返回
    /// </summary>
    [CanBeNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string? UserId { get; set; }
    /// <summary>
    /// 管理范围内配置的部门partyid，仅 Type为<see cref="CustomerStrategyRangeType.Part"/> 时返回
    /// </summary>
    [CanBeNull]
    [JsonProperty("partyid")]
    [JsonPropertyName("partyid")]
    public int? PartyId { get; set; }

    public CustomerStrategyRange()
    {

    }

    private CustomerStrategyRange(
        CustomerStrategyRangeType type, 
        string? userId = null, 
        int? partyId = null)
    {
        Type = type;
        UserId = userId;
        PartyId = partyId;
    }

    public static CustomerStrategyRange Member(string userId)
    {
        Check.NotNullOrWhiteSpace(userId, nameof(userId));

        return new CustomerStrategyRange(CustomerStrategyRangeType.Member, userId);
    }
    public static CustomerStrategyRange Party(int partyId)
    {
        Check.NotDefaultOrNull<int>(partyId, nameof(partyId));

        return new CustomerStrategyRange(CustomerStrategyRangeType.Part, partyId: partyId);
    }
}
