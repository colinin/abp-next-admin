using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Request;
public class WeChatWorkCreateStrategyTagRequest : WeChatWorkRequest
{
    /// <summary>
    /// 规则组id
    /// </summary>
    [NotNull]
    [JsonProperty("strategy_id")]
    [JsonPropertyName("strategy_id")]
    public int StrategyId { get; }
    /// <summary>
    /// 标签组id
    /// </summary>
    [CanBeNull]
    [JsonProperty("group_id")]
    [JsonPropertyName("group_id")]
    public string? GroupId { get; set; }

    private string? _groupName;
    /// <summary>
    /// 标签组名称，最长为30个字符
    /// </summary>
    [CanBeNull]
    [JsonProperty("group_name")]
    [JsonPropertyName("group_name")]
    public string? GroupName {
        get => _groupName;
        set {

            Check.Length(value, nameof(GroupName), 30);
            _groupName = value;
        }
    }
    /// <summary>
    /// 标签组排序的次序值，order值大的排序靠前。有效的值范围是[0, 2^32)
    /// </summary>
    [CanBeNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int? Order { get; set; }
    /// <summary>
    /// 添加的标签组
    /// </summary>
    [CanBeNull]
    [JsonProperty("tag")]
    [JsonPropertyName("tag")]
    public List<NewStrategyTag> Tag { get; }

    public WeChatWorkCreateStrategyTagRequest(int strategyId)
    {
        Check.NotDefaultOrNull<int>(strategyId, nameof(strategyId));

        StrategyId = strategyId;

        Tag = new List<NewStrategyTag>();
    }
}

public class NewStrategyTag
{
    /// <summary>
    /// 添加的标签名称，最长为30个字符
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; }
    /// <summary>
    /// 标签次序值。order值大的排序靠前。有效的值范围是[0, 2^32)
    /// </summary>
    [CanBeNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int? Order { get; set; }
    public NewStrategyTag(string name, int? order = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name), 30);

        Name = name;
        Order = order;
    }
}
