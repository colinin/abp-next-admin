using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
/// <summary>
/// 分配在职成员的客户请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92125" />
/// </remarks>
public class WeChatWorkTransferCustomerRequest : WeChatWorkRequest
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
    /// <summary>
    /// 转移成功后发给客户的消息，最多200个字符，不填则使用默认文案
    /// </summary>
    [CanBeNull]
    [JsonProperty("transfer_success_msg")]
    [JsonPropertyName("transfer_success_msg")]
    public string? TransferSuccessMsg { get; set; }
    public WeChatWorkTransferCustomerRequest(string handoverUserId, string takeoverUserId, string[] externalUserId, string? transferSuccessMsg = null)
    {
        Check.NotNullOrWhiteSpace(handoverUserId, nameof(handoverUserId));
        Check.NotNullOrWhiteSpace(takeoverUserId, nameof(takeoverUserId));
        Check.NotNullOrEmpty(externalUserId, nameof(externalUserId));
        Check.Length(transferSuccessMsg, nameof(transferSuccessMsg), 200);

        HandoverUserId = handoverUserId;
        TakeoverUserId = takeoverUserId;
        ExternalUserId = externalUserId;
        TransferSuccessMsg = transferSuccessMsg;
    }

    protected override void Validate()
    {
        Check.Length(TransferSuccessMsg, nameof(TransferSuccessMsg), 200);
    }
}
