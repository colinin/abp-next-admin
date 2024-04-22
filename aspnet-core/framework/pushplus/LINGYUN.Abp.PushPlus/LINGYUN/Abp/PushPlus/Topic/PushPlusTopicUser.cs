using LINGYUN.Abp.PushPlus.User;
using Newtonsoft.Json;

namespace LINGYUN.Abp.PushPlus.Topic;

public class PushPlusTopicUser
{
    /// <summary>
    /// 用户编号；可用于删除用户
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
    /// <summary>
    /// 昵称
    /// </summary>
    [JsonProperty("nickName")]
    public string NickName { get; set; }
    /// <summary>
    /// 用户微信openId
    /// </summary>
    [JsonProperty("openId")]
    public string OpenId { get; set; }
    /// <summary>
    /// 头像url地址
    /// </summary>
    [JsonProperty("headImgUrl")]
    public string HeadImgUrl { get; set; }
    /// <summary>
    /// 头像url地址
    /// </summary>
    [JsonProperty("userSex")]
    public PushPlusUserSex? UserSex { get; set; }
    /// <summary>
    /// 是否绑定手机；0-未绑定，1-已绑定
    /// </summary>
    [JsonProperty("havePhone")]
    public PushPlusUserPhoneBindStatus? HavePhone { get; set; }
    /// <summary>
    /// 是否关注微信公众号；0-未关注，1-已关注
    /// </summary>
    [JsonProperty("isFollow")]
    public PushPlusUserFollowStatus? IsFollow { get; set; }
    /// <summary>
    /// 邮箱验证状态；0-未验证，1-待验证，2-已验证
    /// </summary>
    [JsonProperty("emailStatus")]
    public PushPlusUserEmailStatus? EmailStatus { get; set; }
}
