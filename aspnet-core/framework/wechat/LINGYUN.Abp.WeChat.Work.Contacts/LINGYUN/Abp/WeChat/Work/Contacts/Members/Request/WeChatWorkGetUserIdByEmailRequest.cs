using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
/// <summary>
/// 邮箱获取userid请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95402" />
/// </remarks>
public class WeChatWorkGetUserIdByEmailRequest : WeChatWorkRequest
{
    /// <summary>
    /// 邮箱
    /// </summary>
    [NotNull]
    [JsonProperty("email")]
    [JsonPropertyName("email")]
    public string Email { get; }
    /// <summary>
    /// 邮箱类型
    /// </summary>
    [CanBeNull]
    [JsonProperty("email_type")]
    [JsonPropertyName("email_type")]
    public EmailType? EmailType { get; }
    public WeChatWorkGetUserIdByEmailRequest(string email, EmailType? emailType = null)
    {
        Email = Check.NotNullOrWhiteSpace(email, nameof(email));
        EmailType = emailType;
    }
}
