using LINGYUN.Abp.WeChat.Work.Authorize.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Authorize.Response;
/// <summary>
/// 企业微信用户详情响应
/// </summary>
public class WeChatWorkUserDetailResponse : WeChatWorkResponse
{
    /// <summary>
    /// 成员UserID
    /// </summary>
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 性别。
    /// 0表示未定义，
    /// 1表示男性，
    /// 2表示女性。
    /// 仅在用户同意snsapi_privateinfo授权时返回真实值，否则返回0
    /// </summary>
    [JsonProperty("gender")]
    [JsonPropertyName("gender")]
    public WeChatWorkGender Gender { get; set; }
    /// <summary>
    /// 头像url。
    /// 仅在用户同意snsapi_privateinfo授权时返回真实头像，否则返回默认头像
    /// </summary>
    [JsonProperty("avatar")]
    [JsonPropertyName("avatar")]
    public string Avatar { get; set; }
    /// <summary>
    /// 员工个人二维码（扫描可添加为外部联系人）
    /// 仅在用户同意snsapi_privateinfo授权时返回
    /// </summary>
    [JsonProperty("qr_code")]
    [JsonPropertyName("qr_code")]
    public string QrCode { get; set; }
    /// <summary>
    /// 手机
    /// 仅在用户同意snsapi_privateinfo授权时返回，第三方应用不可获取
    /// </summary>
    [JsonProperty("mobile")]
    [JsonPropertyName("mobile")]
    public string Mobile { get; set; }
    /// <summary>
    /// 邮箱
    /// 仅在用户同意snsapi_privateinfo授权时返回，第三方应用不可获取
    /// </summary>
    [JsonProperty("email")]
    [JsonPropertyName("email")]
    public string Email { get; set; }
    /// <summary>
    /// 企业邮箱
    /// 仅在用户同意snsapi_privateinfo授权时返回，第三方应用不可获取
    /// </summary>
    [JsonProperty("biz_mail")]
    [JsonPropertyName("biz_mail")]
    public string WorkEmail { get; set; }
    /// <summary>
    /// 地址
    /// 仅在用户同意snsapi_privateinfo授权时返回，第三方应用不可获取
    /// </summary>
    [JsonProperty("address")]
    [JsonPropertyName("address")]
    public string Address { get; set; }
}
