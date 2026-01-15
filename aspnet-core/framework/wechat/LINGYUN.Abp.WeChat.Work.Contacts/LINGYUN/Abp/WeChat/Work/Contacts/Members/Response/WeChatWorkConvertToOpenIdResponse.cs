using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Response;
/// <summary>
/// userid转openid响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90202" />
/// </remarks>
public class WeChatWorkConvertToOpenIdResponse : WeChatWorkResponse
{
    /// <summary>
    /// 企业微信成员userid对应的openid
    /// </summary>
    [NotNull]
    [JsonProperty("openid")]
    [JsonPropertyName("openid")]
    public string OpenId { get; set; }
}
