using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
/// <summary>
/// 手机号获取userid请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95402" />
/// </remarks>
public class WeChatWorkGetUserIdByMobileRequest : WeChatWorkRequest
{
    /// <summary>
    /// 用户在企业微信通讯录中的手机号码。长度为5~32个字节
    /// </summary>
    [NotNull]
    [JsonProperty("mobile")]
    [JsonPropertyName("mobile")]
    public string Mobile { get; }
    public WeChatWorkGetUserIdByMobileRequest(string mobile)
    {
        Mobile = Check.NotNullOrWhiteSpace(mobile, nameof(mobile), 32, 5);
    }
}
