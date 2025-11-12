using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 添加了外部联系人的企业成员
/// </summary>
public class FollowUser
{
    /// <summary>
    /// 添加了此外部联系人的企业成员userid
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 发起添加的userid<br />
    /// 如果成员主动添加，为成员的userid；<br />
    /// 如果是客户主动添加，则为客户的外部联系人userid；<br />
    /// 如果是内部成员共享/管理员分配，则为对应的成员/管理员userid
    /// </summary>
    [NotNull]
    [JsonProperty("oper_userid")]
    [JsonPropertyName("oper_userid")]
    public string OperUserId { get; set; }
    /// <summary>
    /// 企业自定义的state参数，用于区分客户具体是通过哪个「联系我」或获客链接添加
    /// </summary>
    [CanBeNull]
    [JsonProperty("state")]
    [JsonPropertyName("state")]
    public string? State { get; set; }
    /// <summary>
    /// 该成员对此外部联系人的备注
    /// </summary>
    [CanBeNull]
    [JsonProperty("remark")]
    [JsonPropertyName("remark")]
    public string? Remark { get; set; }
    /// <summary>
    /// 该成员对此外部联系人的描述
    /// </summary>
    [CanBeNull]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    /// <summary>
    /// 该成员添加此外部联系人的时间
    /// </summary>
    [NotNull]
    [JsonProperty("createtime")]
    [JsonPropertyName("createtime")]
    public long Createtime { get; set; }
    /// <summary>
    /// 外部联系人所打标签列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("tags")]
    [JsonPropertyName("tags")]
    public List<FollowUserTag>? Tags { get; set; }
    /// <summary>
    /// 该成员对此微信客户备注的企业名称（仅微信客户有该字段）
    /// </summary>
    [CanBeNull]
    [JsonProperty("remark_corp_name")]
    [JsonPropertyName("remark_corp_name")]
    public string? RemarkCorpName { get; set; }
    /// <summary>
    /// 该成员对此客户备注的手机号码，代开发自建应用需要管理员授权才可以获取
    /// </summary>
    [CanBeNull]
    [JsonProperty("remark_mobiles")]
    [JsonPropertyName("remark_mobiles")]
    public List<string>? RemarkMobiles { get; set; }
    /// <summary>
    /// 该成员添加此客户的来源
    /// </summary>
    [NotNull]
    [JsonProperty("add_way")]
    [JsonPropertyName("add_way")]
    public FollowUserAddWay AddWay { get; set; }
    /// <summary>
    /// 该成员添加此客户的来源add_way为10时，对应的视频号信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("wechat_channels")]
    [JsonPropertyName("wechat_channels")]
    public FollowUserWechatChannel? WechatChannel { get; set; }
}
