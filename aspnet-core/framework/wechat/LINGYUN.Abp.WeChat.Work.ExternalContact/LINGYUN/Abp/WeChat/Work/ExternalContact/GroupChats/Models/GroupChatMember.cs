using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Models;
/// <summary>
/// 群成员
/// </summary>
public class GroupChatMember
{
    /// <summary>
    /// 群成员id
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 成员类型
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public GroupChatMemberType Type { get; set; }
    /// <summary>
    /// 外部联系人在微信开放平台的唯一身份标识（微信unionid），通过此字段企业可将外部联系人与公众号/小程序用户关联起来。<br />
    /// 仅当群成员类型是微信用户（包括企业成员未添加好友），且企业绑定了微信开发者ID有此字段
    /// </summary>
    [CanBeNull]
    [JsonProperty("unionid")]
    [JsonPropertyName("unionid")]
    public string? UnionId { get; set; }
    /// <summary>
    /// 入群时间
    /// </summary>
    [NotNull]
    [JsonProperty("join_time")]
    [JsonPropertyName("join_time")]
    public long JoinTime { get; set; }
    /// <summary>
    /// 入群方式
    /// </summary>
    [NotNull]
    [JsonProperty("join_scene")]
    [JsonPropertyName("join_scene")]
    public GroupChatMemberJoinScene JoinScene { get; set; }
    /// <summary>
    /// 邀请者。目前仅当是由本企业内部成员邀请入群时会返回该值
    /// </summary>
    [CanBeNull]
    [JsonProperty("invitor")]
    [JsonPropertyName("invitor")]
    public GroupChatInvitor? Invitor { get; set; }
    /// <summary>
    /// 在群里的昵称
    /// </summary>
    [CanBeNull]
    [JsonProperty("group_nickname")]
    [JsonPropertyName("group_nickname")]
    public string? GroupNickname { get; set; }
    /// <summary>
    /// 名字。仅当 need_name = 1 时返回<br />
    /// 如果是微信用户，则返回其在微信中设置的名字<br />
    /// 如果是企业微信联系人，则返回其设置对外展示的别名或实名
    /// </summary>
    [CanBeNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
