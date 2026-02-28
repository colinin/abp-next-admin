using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
/// <summary>
/// 分配离职成员的客户请求参数
/// </summary>
public class WeChatWorkResignedTransferCustomerRequest : WeChatWorkRequest
{
    /// <summary>
    /// 原跟进成员的userid
    /// </summary>
    [NotNull]
    [JsonProperty("handover_userid")]
    [JsonPropertyName("handover_userid")]
    public string HandoverUserId { get; }
    /// <summary>
    /// 接替成员的userid
    /// </summary>
    [NotNull]
    [JsonProperty("takeover_userid")]
    [JsonPropertyName("takeover_userid")]
    public string TakeoverUserId { get; }
    /// <summary>
    /// 客户的external_userid列表，每次最多分配100个客户
    /// </summary>
    [NotNull]
    [JsonProperty("external_userid")]
    [JsonPropertyName("external_userid")]
    public string[] ExternalUserId { get; }
    public WeChatWorkResignedTransferCustomerRequest(string handoverUserId, string takeoverUserId, string[] externalUserId)
    {
        Check.NotNullOrWhiteSpace(handoverUserId, nameof(handoverUserId));
        Check.NotNullOrWhiteSpace(takeoverUserId, nameof(takeoverUserId));
        Check.NotNullOrEmpty(externalUserId, nameof(externalUserId));

        HandoverUserId = handoverUserId;
        TakeoverUserId = takeoverUserId;
        ExternalUserId = externalUserId;
    }

    protected override void Validate()
    {
        if (ExternalUserId.Length > 100)
        {
            throw new ArgumentException("Transfer a maximum of 100 customers at a time!");
        }
    }
}
