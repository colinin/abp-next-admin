using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Models;
public class TransferCustomerResult
{
    /// <summary>
    /// 客户的外部联系人userid
    /// </summary>
    [NotNull]
    [JsonProperty("external_userid")]
    [JsonPropertyName("external_userid")]
    public string ExternalUserid { get; set; }
    /// <summary>
    /// 接替状态
    /// </summary>
    [NotNull]
    [JsonProperty("status")]
    [JsonPropertyName("status")]
    public TransferStatus Status { get; set; }
    /// <summary>
    /// 接替客户的时间，如果是等待接替状态，则为未来的自动接替时间
    /// </summary>
    [NotNull]
    [JsonProperty("takeover_time")]
    [JsonPropertyName("takeover_time")]
    public long TakeoverTime { get; set; }
}
