using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Models;
/// <summary>
/// 客户群详情
/// </summary>
public class GroupChatInfo
{
    /// <summary>
    /// 客户群ID
    /// </summary>
    [NotNull]
    [JsonProperty("chat_id")]
    [JsonPropertyName("chat_id")]
    public string ChatId { get; set; }
    /// <summary>
    /// 群名
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    /// <summary>
    /// 群主ID
    /// </summary>
    [NotNull]
    [JsonProperty("owner")]
    [JsonPropertyName("owner")]
    public string Owner { get; set; }
    /// <summary>
    /// 群的创建时间
    /// </summary>
    [NotNull]
    [JsonProperty("create_time")]
    [JsonPropertyName("create_time")]
    public long CreateTime { get; set; }
    /// <summary>
    /// 群公告
    /// </summary>
    [CanBeNull]
    [JsonProperty("notice")]
    [JsonPropertyName("notice")]
    public string Notice { get; set; }
    /// <summary>
    /// 群成员列表
    /// </summary>
    [NotNull]
    [JsonProperty("member_list")]
    [JsonPropertyName("member_list")]
    public GroupChatMember[] MemberList { get; set; }
    /// <summary>
    /// 群管理员列表
    /// </summary>
    [NotNull]
    [JsonProperty("admin_list")]
    [JsonPropertyName("admin_list")]
    public GroupChatManager[] AdminList { get; set; }
    /// <summary>
    /// 当前群成员版本号。可以配合客户群变更事件减少主动调用本接口的次数
    /// </summary>
    [NotNull]
    [JsonProperty("member_version")]
    [JsonPropertyName("member_version")]
    public string MemberVersion { get; set; }
}
