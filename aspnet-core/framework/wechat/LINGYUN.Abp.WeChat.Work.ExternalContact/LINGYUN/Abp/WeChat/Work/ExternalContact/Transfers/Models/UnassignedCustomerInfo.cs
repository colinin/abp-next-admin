using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Models;
public class UnassignedCustomerInfo
{
    /// <summary>
    /// 离职成员的userid
    /// </summary>
    [NotNull]
    [JsonProperty("handover_userid")]
    [JsonPropertyName("handover_userid")]
    public string HandoverUserid { get; set; }
    /// <summary>
    /// 外部联系人userid
    /// </summary>
    [NotNull]
    [JsonProperty("external_userid")]
    [JsonPropertyName("external_userid")]
    public string ExternalUserid { get; set; }
    /// <summary>
    /// 成员离职时间
    /// </summary>
    [NotNull]
    [JsonProperty("dimission_time")]
    [JsonPropertyName("dimission_time")]
    public long DimissionTime { get; set; }
}
