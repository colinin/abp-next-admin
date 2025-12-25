using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Request;
/// <summary>
/// 获取单聊会话同意情况请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91782" />
/// </remarks>
public class WeChatWorkCheckSingleAgreeRequest : WeChatWorkRequest
{
    /// <summary>
    /// 内部成员的userid
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; }
    /// <summary>
    /// 外部成员的exteranalopenid
    /// </summary>
    [NotNull]
    [JsonProperty("exteranalopenid")]
    [JsonPropertyName("exteranalopenid")]
    public string ExteranalOpenId { get; }
    public WeChatWorkCheckSingleAgreeRequest(string userId, string exteranalOpenId)
    {
        UserId = userId;
        ExteranalOpenId = exteranalOpenId;
    }
}
