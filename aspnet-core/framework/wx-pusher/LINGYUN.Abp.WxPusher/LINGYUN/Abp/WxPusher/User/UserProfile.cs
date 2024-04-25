using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.WxPusher.User;

[Serializable]
public class UserProfile
{
    /// <summary>
    /// 用户uid
    /// </summary>
    [JsonProperty("uid")]
    public string Uid { get; set; }
    /// <summary>
    /// 用户关注的应用或者主题id，根据type来区分
    /// </summary>
    [JsonProperty("appOrTopicId")]
    public long? AppOrTopicId { get; set; }
    /// <summary>
    /// 新用户微信不再返回 ，强制返回空
    /// </summary>
    [JsonProperty("headImg")]
    public string HeadImg { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonProperty("createTime")]
    public long CreateTime { get; set; }
    /// <summary>
    /// 新用户微信不再返回 ，强制返回空
    /// </summary>
    [JsonProperty("nickName")]
    public string NickName { get; set; }
    /// <summary>
    /// 是否拉黑
    /// </summary>
    [JsonProperty("reject")]
    public bool Reject { get; set; }
    /// <summary>
    /// 如果调用删除或者拉黑接口，需要这个id
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
    /// <summary>
    /// 关注类型，
    /// 0：关注应用，
    /// 1：关注topic
    /// </summary>
    [JsonProperty("type")]
    public FlowType Type { get; set; }
    /// <summary>
    /// 关注的应用或者主题名字
    /// </summary>
    [JsonProperty("target")]
    public string Target { get; set; }
    /// <summary>
    /// 0表示用户不是付费用户，大于0表示用户付费订阅到期时间，毫秒级时间戳
    /// </summary>
    [JsonProperty("payEndTime")]
    public long PayEndTime { get; set; }
}
