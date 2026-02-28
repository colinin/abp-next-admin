using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
/// <summary>
/// openid转userid请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90202" />
/// </remarks>
public class WeChatWorkConvertToUserIdRequest : WeChatWorkRequest
{
    /// <summary>
    /// 在使用企业支付之后，返回结果的openid
    /// </summary>
    [NotNull]
    [JsonProperty("openid")]
    [JsonPropertyName("openid")]
    public string OpenId { get; }
    public WeChatWorkConvertToUserIdRequest(string openId)
    {
        OpenId = Check.NotNullOrWhiteSpace(openId, nameof(openId));
    }
}

