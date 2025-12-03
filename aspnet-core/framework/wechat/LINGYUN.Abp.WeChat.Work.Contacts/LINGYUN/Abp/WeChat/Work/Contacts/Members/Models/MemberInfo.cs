using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Common.Members.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 成员详情
/// </summary>
public class MemberInfo
{
    /// <summary>
    /// 全局唯一
    /// </summary>
    [NotNull]
    [JsonProperty("open_userid")]
    [JsonPropertyName("open_userid")]
    public string OpenUserId { get; set; }
    /// <summary>
    /// 成员UserID
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 成员名称
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    /// <summary>
    /// 手机号码
    /// </summary>
    [CanBeNull]
    [JsonProperty("mobile")]
    [JsonPropertyName("mobile")]
    public string? Mobile { get; set; }
    /// <summary>
    /// 成员所属部门id列表
    /// </summary>
    [NotNull]
    [JsonProperty("department")]
    [JsonPropertyName("department")]
    public int[] Department { get; set; }
    /// <summary>
    /// 主部门
    /// </summary>
    [CanBeNull]
    [JsonProperty("main_department")]
    [JsonPropertyName("main_department")]
    public int? MainDepartment { get; set; }
    /// <summary>
    /// 部门内的排序值，默认为0。数量必须和department一致，数值越大排序越前面。值范围是[0, 2^32)
    /// </summary>
    [NotNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int[]? Order { get; set; }
    /// <summary>
    /// 员工个人二维码
    /// </summary>
    [CanBeNull]
    [JsonProperty("qr_code")]
    [JsonPropertyName("qr_code")]
    public string? QrCode { get; set; }
    /// <summary>
    /// 职务信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("position")]
    [JsonPropertyName("position")]
    public string? Position { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    [CanBeNull]
    [JsonProperty("gender")]
    [JsonPropertyName("gender")]
    public Gender? Gender { get; set; }
    /// <summary>
    /// 邮箱
    /// </summary>
    [CanBeNull]
    [JsonProperty("email")]
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    /// <summary>
    /// 企业邮箱
    /// </summary>
    [CanBeNull]
    [JsonProperty("biz_mail")]
    [JsonPropertyName("biz_mail")]
    public string? BizEmail { get; set; }
    /// <summary>
    /// 表示在所在的部门内是否为部门负责人，数量与department一致
    /// </summary>
    [CanBeNull]
    [JsonProperty("is_leader_in_dept")]
    [JsonPropertyName("is_leader_in_dept")]
    public int[]? IsLeaderInDept { get; set; }
    /// <summary>
    /// 直属上级UserID，返回在应用可见范围内的直属上级列表，最多有1个直属上级
    /// </summary>
    [CanBeNull]
    [JsonProperty("direct_leader")]
    [JsonPropertyName("direct_leader")]
    public string[]? DirectLeader { get; set; }
    /// <summary>
    /// 头像url
    /// </summary>
    [CanBeNull]
    [JsonProperty("avatar")]
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
    /// <summary>
    /// 头像缩略图url
    /// </summary>
    [CanBeNull]
    [JsonProperty("thumb_avatar")]
    [JsonPropertyName("thumb_avatar")]
    public string? ThumbAvatar { get; set; }
    /// <summary>
    /// 座机
    /// </summary>
    [CanBeNull]
    [JsonProperty("telephone")]
    [JsonPropertyName("telephone")]
    public string? TelePhone { get; set; }
    /// <summary>
    /// 别名
    /// </summary>
    [CanBeNull]
    [JsonProperty("alias")]
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }
    /// <summary>
    /// 地址
    /// </summary>
    [CanBeNull]
    [JsonProperty("address")]
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    /// <summary>
    /// 激活状态
    /// </summary>
    [NotNull]
    [JsonProperty("status")]
    [JsonPropertyName("status")]
    public MemberStatus Status { get; set; }
    /// <summary>
    /// 扩展属性
    /// </summary>
    [CanBeNull]
    [JsonProperty("extattr")]
    [JsonPropertyName("extattr")]
    public MemberExternalAttribute? ExternalAttribute { get; set; }
    /// <summary>
    /// 对外职务，如果设置了该值，则以此作为对外展示的职务，否则以position来展示。
    /// </summary>
    [CanBeNull]
    [JsonProperty("external_position")]
    [JsonPropertyName("external_position")]
    public string? ExternalPosition { get; set; }
    /// <summary>
    /// 外部联系人的自定义展示信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("external_profile")]
    [JsonPropertyName("external_profile")]
    public ExternalProfile? ExternalProfile { get; set; }
}
