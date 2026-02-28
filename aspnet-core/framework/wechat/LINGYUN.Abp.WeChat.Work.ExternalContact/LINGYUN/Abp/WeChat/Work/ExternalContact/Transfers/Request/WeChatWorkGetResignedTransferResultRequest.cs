using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
/// <summary>
/// 查询离职成员的客户接替状态请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94082" />
/// </remarks>
public class WeChatWorkGetResignedTransferResultRequest : WeChatWorkRequest
{
    /// <summary>
    /// 原添加成员的userid
    /// </summary>
    [NotNull]
    [JsonProperty("handover_userid")]
    [JsonPropertyName("handover_userid")]
    public string HandOverUserId { get; }
    /// <summary>
    /// 接替成员的userid
    /// </summary>
    [NotNull]
    [JsonProperty("takeover_userid")]
    [JsonPropertyName("takeover_userid")]
    public string TakeOverUserId { get; }
    /// <summary>
    /// 分页查询的cursor，每个分页返回的数据不会超过1000条；不填或为空表示获取第一个分页
    /// </summary>
    [CanBeNull]
    [JsonProperty("cursor")]
    [JsonPropertyName("cursor")]
    public string? Cursor { get; }
    public WeChatWorkGetResignedTransferResultRequest(
        string handOverUserId,
        string takeOverUserId,
        string? cursor = null)
    {
        Check.NotNullOrWhiteSpace(handOverUserId, nameof(handOverUserId));
        Check.NotNullOrWhiteSpace(takeOverUserId, nameof(takeOverUserId));

        HandOverUserId = handOverUserId;
        TakeOverUserId = takeOverUserId;
        Cursor = cursor;
    }
}
