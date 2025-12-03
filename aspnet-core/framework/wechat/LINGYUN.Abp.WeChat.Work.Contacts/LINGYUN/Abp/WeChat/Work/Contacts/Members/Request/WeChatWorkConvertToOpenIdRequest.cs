using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
/// <summary>
/// userid转openid请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90202" />
/// </remarks>
public class WeChatWorkConvertToOpenIdRequest : WeChatWorkRequest
{
    /// <summary>
    /// 企业内的成员id
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; }
    public WeChatWorkConvertToOpenIdRequest(string userId)
    {
        UserId = Check.NotNullOrWhiteSpace(userId, nameof(userId));
    }
}
