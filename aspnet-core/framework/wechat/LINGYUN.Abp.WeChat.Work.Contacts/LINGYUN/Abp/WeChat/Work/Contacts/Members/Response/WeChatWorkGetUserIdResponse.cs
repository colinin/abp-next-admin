using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Response;
/// <summary>
/// 获取userid响应参数
/// </summary>
public class WeChatWorkGetUserIdResponse : WeChatWorkResponse
{
    /// <summary>
    /// 	成员UserID。对应管理端的账号，企业内必须唯一。不区分大小写，长度为1~64个字节。注意：第三方应用获取的值是密文的userid
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
}
