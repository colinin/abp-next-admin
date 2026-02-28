using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Models;
/// <summary>
/// 外部联系人
/// </summary>
public class ExternalContactInfo
{
    /// <summary>
    /// 是否被成员标记为客户
    /// </summary>
    [NotNull]
    [JsonProperty("is_customer")]
    [JsonPropertyName("is_customer")]
    public bool IsCustomer { get; set; }
    /// <summary>
    /// 外部联系人临时ID
    /// </summary>
    /// <remarks>
    /// 
    /// 外部联系人临时id是一个外部联系人的唯一标识，企业可根据此id对外部联系人进行去重统计。<br />
    /// 但外部联系人临时id仅在一轮遍历查询（从首个分页查询开始到最后一个分页查询完毕）中唯一；<br />
    /// 每次请求首个数据分页（cursor为空）时，返回的外部联系人临时id和next_cursor将发生变化。
    /// </remarks>
    [NotNull]
    [JsonProperty("tmp_openid")]
    [JsonPropertyName("tmp_openid")]
    public string TmpOpenId { get; set; }
    /// <summary>
    /// 外部联系人的externaluserid（如果是客户才返回）
    /// </summary>
    [CanBeNull]
    [JsonProperty("external_userid")]
    [JsonPropertyName("external_userid")]
    public string? ExternalUserId { get; set; }
    /// <summary>
    /// 脱敏后的外部联系人昵称（如果是其他外部联系人才返回）
    /// </summary>
    [CanBeNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    /// <summary>
    /// 添加此外部联系人的企业成员或外部联系人所在群聊的群主userid
    /// </summary>
    [CanBeNull]
    [JsonProperty("follow_userid")]
    [JsonPropertyName("follow_userid")]
    public string? FollowUserId { get; set; }
    /// <summary>
    /// 外部联系人所在的群聊ID（如果群聊被成员标记为客户群才返回）
    /// </summary>
    [CanBeNull]
    [JsonProperty("chat_id")]
    [JsonPropertyName("chat_id")]
    public string? ChatId { get; set; }
    /// <summary>
    /// 外部联系人所在群聊的群名（如果群聊未被成员标记为客户群才返回）
    /// </summary>
    [CanBeNull]
    [JsonProperty("chat_name")]
    [JsonPropertyName("chat_name")]
    public string? ChatName { get; set; }
    /// <summary>
    /// 外部联系人首次添加/进群的时间
    /// </summary>
    [NotNull]
    [JsonProperty("add_time")]
    [JsonPropertyName("add_time")]
    public long AddTime { get; set; }
}
