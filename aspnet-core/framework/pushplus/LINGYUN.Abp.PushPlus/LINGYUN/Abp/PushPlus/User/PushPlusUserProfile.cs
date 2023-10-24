using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.PushPlus.User;

public class PushPlusUserProfile
{
    /// <summary>
    /// 用户微信的openId
    /// </summary>
    [JsonProperty("openId")]
    public string OpenId { get; set; }
    /// <summary>
    /// 用户微信的unionId
    /// </summary>
    [JsonProperty("unionId")]
    public string UnionId { get; set; }
    /// <summary>
    /// 昵称
    /// </summary>
    [JsonProperty("nickName")]
    public string NickName { get; set; }
    /// <summary>
    /// 头像
    /// </summary>
    [JsonProperty("headImgUrl")]
    public string HeadImgUrl { get; set; }
    /// <summary>
    /// 性别；
    /// 0-未设置，
    /// 1-男，
    /// 2-女
    /// </summary>
    [JsonProperty("userSex")]
    public PushPlusUserSex? Sex { get; set; }
    /// <summary>
    /// 用户令牌
    /// </summary>
    [JsonProperty("token")]
    public string Token { get; set; }
    /// <summary>
    /// 手机号
    /// </summary>
    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }
    /// <summary>
    /// 邮箱
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; }
    /// <summary>
    /// 邮箱验证状态；
    /// 0-未验证，
    /// 1-待验证，
    /// 2-已验证
    /// </summary>
    [JsonProperty("emailStatus")]
    public PushPlusUserEmailStatus? EmailStatus { get; set; }
    /// <summary>
    /// 生日
    /// </summary>
    [JsonProperty("birthday")]
    public DateTime? Birthday { get; set; }
    /// <summary>
    /// 用户积分
    /// </summary>
    [JsonProperty("points")]
    public int? Points { get; set; }
}
