using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 外部联系人列表
/// </summary>
public class ExternalContactList
{
    /// <summary>
    /// 客户的基本信息
    /// </summary>
    [NotNull]
    [JsonProperty("external_contact")]
    [JsonPropertyName("external_contact")]
    public ExternalContactInfo ExternalContact { get; set; }
    /// <summary>
    /// 企业成员客户跟进信息
    /// </summary>
    [NotNull]
    [JsonProperty("follow_info")]
    [JsonPropertyName("follow_info")]
    public FollowUser FollowUser { get; set; }
}
