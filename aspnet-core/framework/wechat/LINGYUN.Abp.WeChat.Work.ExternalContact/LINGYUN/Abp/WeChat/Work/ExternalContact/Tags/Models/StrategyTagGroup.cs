using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Models;
public class StrategyTagGroup : TagGroup
{
    /// <summary>
    /// 标签组所属的规则组id
    /// </summary>
    [NotNull]
    [JsonProperty("strategy_id")]
    [JsonPropertyName("strategy_id")]
    public int StrategyId { get; set; }
    /// <summary>
    /// 标签列表
    /// </summary>
    [NotNull]
    [JsonProperty("tag")]
    [JsonPropertyName("tag")]
    public StrategyTag[] Tag { get; set; }
}
