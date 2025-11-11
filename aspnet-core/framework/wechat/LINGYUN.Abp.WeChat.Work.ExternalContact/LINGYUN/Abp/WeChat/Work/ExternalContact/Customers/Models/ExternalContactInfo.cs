using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 外部联系人信息
/// </summary>
public class ExternalContactInfo
{
    /// <summary>
    /// 外部联系人的userid
    /// </summary>
    [NotNull]
    [JsonProperty("external_userid")]
    [JsonPropertyName("external_userid")]
    public string ExternalUserId { get; set; }
    /// <summary>
    /// 外部联系人的名称
    /// </summary>
    /// <remarks>
    /// 如果是微信用户，则返回其微信昵称。<br />
    /// 如果是企业微信联系人，则返回其设置对外展示的别名或实名
    /// </remarks>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    /// <summary>
    /// 外部联系人头像
    /// </summary>
    [CanBeNull]
    [JsonProperty("avatar")]
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
    /// <summary>
    /// 外部联系人的类型
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public ExternalContactType Type { get; set; }
    /// <summary>
    /// 外部联系人性别
    /// </summary>
    [NotNull]
    [JsonProperty("gender")]
    [JsonPropertyName("gender")]
    public ExternalContactGender Gender { get; set; }
    /// <summary>
    /// 外部联系人在微信开放平台的唯一身份标识（微信unionid），通过此字段企业可将外部联系人与公众号/小程序用户关联起来。<br />
    /// 仅当联系人类型是微信用户，且企业绑定了微信开发者ID有此字段
    /// </summary>
    [CanBeNull]
    [JsonProperty("unionid")]
    [JsonPropertyName("unionid")]
    public string? UnionId { get; set; }
    /// <summary>
    /// 外部联系人的职位，如果外部企业或用户选择隐藏职位，则不返回，仅当联系人类型是企业微信用户时有此字段
    /// </summary>
    [CanBeNull]
    [JsonProperty("position")]
    [JsonPropertyName("position")]
    public string? Position { get; set; }
    /// <summary>
    /// 外部联系人所在企业的简称，仅当联系人类型是企业微信用户时有此字段
    /// </summary>
    [CanBeNull]
    [JsonProperty("corp_name")]
    [JsonPropertyName("corp_name")]
    public string? CorpName { get; set; }
    /// <summary>
    /// 外部联系人所在企业的主体名称，仅当联系人类型是企业微信用户时有此字段<br />
    /// 仅企业自建应用可获取
    /// </summary>
    [CanBeNull]
    [JsonProperty("corp_full_name")]
    [JsonPropertyName("corp_full_name")]
    public string? CorpFullName { get; set; }
    /// <summary>
    /// 外部联系人的自定义展示信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("external_profile")]
    [JsonPropertyName("external_profile")]
    public ExternalProfile? ExternalProfile { get; set; }
}
