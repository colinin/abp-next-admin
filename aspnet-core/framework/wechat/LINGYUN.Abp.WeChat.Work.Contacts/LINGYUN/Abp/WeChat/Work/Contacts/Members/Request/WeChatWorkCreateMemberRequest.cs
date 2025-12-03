using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Common.Members.Models;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
/// <summary>
/// 创建成员请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90195" />
/// </remarks>
public class WeChatWorkCreateMemberRequest : WeChatWorkRequest
{
    /// <summary>
    /// 成员UserID。对应管理端的账号，企业内必须唯一。长度为1~64个字节。只能由数字、字母和“_-@.”四种字符组成，且第一个字符必须是数字或字母。系统进行唯一性检查时会忽略大小写。
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    [StringLength(64, MinimumLength = 1)]
    public string UserId { get; set; }
    /// <summary>
    /// 成员名称。长度为1~64个utf8字符
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    [StringLength(64, MinimumLength = 1)]
    public string Name { get; set; }
    /// <summary>
    /// 成员别名。长度1~64个utf8字符
    /// </summary>
    [CanBeNull]
    [JsonProperty("alias")]
    [JsonPropertyName("alias")]
    [StringLength(64, MinimumLength = 1)]
    public string? Alias { get; set; }
    /// <summary>
    /// 手机号码。企业内必须唯一，mobile/email二者不能同时为空 ，中国大陆手机号码可省略“+86”，其他国家或地区必须要带上国际码。
    /// </summary>
    [CanBeNull]
    [JsonProperty("mobile")]
    [JsonPropertyName("mobile")]
    public string? Mobile { get; set; }
    /// <summary>
    /// 成员所属部门id列表，不超过100个。当不填写department或id为0时，成员会放在其他（待设置部门）下，当填写的部门不存在时，会在在其他（待设置部门）下新建对应部门
    /// </summary>
    [CanBeNull]
    [JsonProperty("department")]
    [JsonPropertyName("department")]
    public int[]? Department { get; set; }
    /// <summary>
    /// 部门内的排序值，默认为0，成员次序以创建时间从小到大排列。个数必须和参数department的个数一致，数值越大排序越前面。有效的值范围是[0, 2^32)
    /// </summary>
    [CanBeNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int[]? Order { get; set; }
    /// <summary>
    /// 职务信息。长度为0~128个字符
    /// </summary>
    [CanBeNull]
    [StringLength(128)]
    [JsonProperty("position")]
    [JsonPropertyName("position")]
    public string? Position { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    [CanBeNull]
    [JsonProperty("gender")]
    [JsonPropertyName("gender")]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.EnumToNumberStringConverter<Gender>))]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.EnumToNumberStringConverter<Gender>))]
    public Gender? Gender { get; set; }
    /// <summary>
    /// 邮箱。可填写企业已有的邮箱账号，方便同事获取成员的邮箱账号以发邮件。长度6~64个字节，且为有效的email格式。企业内必须唯一，mobile/email二者不能同时为空。境外成员可用此邮箱登录企业微信。
    /// </summary>
    [CanBeNull]
    [StringLength(64)]
    [JsonProperty("email")]
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    /// <summary>
    /// 如果企业已开通腾讯企业邮（企业微信邮箱），设置该值可创建企业邮箱账号。长度6~64个字节，且为有效的企业邮箱格式。企业内必须唯一。未填写则系统会为用户生成默认企业邮箱（由系统生成的邮箱可修改一次）
    /// </summary>
    [CanBeNull]
    [StringLength(64)]
    [JsonProperty("biz_mail")]
    [JsonPropertyName("biz_mail")]
    public string? BizEmail { get; set; }
    /// <summary>
    /// 座机。32字节以内，由纯数字、“-”、“+”或“,”组成。
    /// </summary>
    [CanBeNull]
    [StringLength(32)]
    [JsonProperty("telephone")]
    [JsonPropertyName("telephone")]
    public string? TelePhone { get; set; }
    /// <summary>
    /// 个数必须和参数department的个数一致，表示在所在的部门内是否为部门负责人。1表示为部门负责人，0表示非部门负责人。在审批(自建、第三方)等应用里可以用来标识上级审批人
    /// </summary>
    [CanBeNull]
    [JsonProperty("is_leader_in_dept")]
    [JsonPropertyName("is_leader_in_dept")]
    public int[]? IsLeaderInDept { get; set; }
    /// <summary>
    /// 直属上级UserID，设置范围为企业内成员，可以设置最多1个上级
    /// </summary>
    [CanBeNull]
    [JsonProperty("direct_leader")]
    [JsonPropertyName("direct_leader")]
    public string[]? DirectLeader { get; set; }
    /// <summary>
    /// 成员头像的mediaid，通过素材管理接口上传图片获得的mediaid
    /// </summary>
    [CanBeNull]
    [JsonProperty("avatar_mediaid")]
    [JsonPropertyName("avatar_mediaid")]
    public string? AvatarMediaId { get; set; }
    /// <summary>
    /// 启用/禁用成员。1表示启用成员，0表示禁用成员
    /// </summary>
    [CanBeNull]
    [JsonProperty("enable")]
    [JsonPropertyName("enable")]
    public int? Enable { get; set; }
    /// <summary>
    /// 扩展属性。扩展属性字段需要先在WEB管理端添加，见扩展属性添加方法，否则忽略未知属性的赋值。字段详情见成员扩展属性
    /// </summary>
    [CanBeNull]
    [JsonProperty("extattr")]
    [JsonPropertyName("extattr")]
    public MemberExternalAttribute? ExternalAttribute { get; set; }
    /// <summary>
    /// 是否邀请该成员使用企业微信（将通过微信服务通知或短信或邮件下发邀请，每天自动下发一次，最多持续3个工作日），默认值为true。
    /// </summary>
    [CanBeNull]
    [JsonProperty("to_invite")]
    [JsonPropertyName("to_invite")]
    public bool? ToInvite { get; set; }
    /// <summary>
    /// 成员对外属性，字段详情见对外属性
    /// </summary>
    [CanBeNull]
    [JsonProperty("external_profile")]
    [JsonPropertyName("external_profile")]
    public ExternalProfile? ExternalProfile { get; set; }
    /// <summary>
    /// 对外职务，如果设置了该值，则以此作为对外展示的职务，否则以position来展示。长度12个汉字内
    /// </summary>
    [CanBeNull]
    [StringLength(12)]
    [JsonProperty("external_position")]
    [JsonPropertyName("external_position")]
    public string? ExternalPosition { get; set; }
    /// <summary>
    /// 视频号名字（设置后，成员将对外展示该视频号）。须从企业绑定到企业微信的视频号中选择，可在“我的企业”页中查看绑定的视频号
    /// </summary>
    [CanBeNull]
    [JsonProperty("nickname")]
    [JsonPropertyName("nickname")]
    public string? NickName { get; set; }
    /// <summary>
    /// 地址。长度最大128个字符
    /// </summary>
    [CanBeNull]
    [StringLength(128)]
    [JsonProperty("address")]
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    /// <summary>
    /// 主部门
    /// </summary>
    [CanBeNull]
    [JsonProperty("main_department")]
    [JsonPropertyName("main_department")]
    public int? MainDepartment { get; set; }
    protected override void Validate()
    {
        Check.NotNullOrWhiteSpace(UserId, nameof(UserId), 64, 1);
        Check.NotNullOrWhiteSpace(Name, nameof(Name), 64, 1);
        Check.Length(Alias, nameof(Alias), 64, 1);
        Check.Length(Position, nameof(Position), 128);
        Check.Length(Email, nameof(Email), 64, 6);
        Check.Length(BizEmail, nameof(BizEmail), 64, 6);
        Check.Length(TelePhone, nameof(TelePhone), 32);
        Check.Length(ExternalPosition, nameof(ExternalPosition), 12);
        Check.Length(Address, nameof(Address), 128);
        // mobile/email二者不能同时为空
        if (Mobile.IsNullOrWhiteSpace() && Email.IsNullOrWhiteSpace())
        {
            throw new ArgumentException("mobile and email cannot be empty at the same time!");
        }
        // 成员所属部门id列表，不超过100个
        if (Department?.Length > 0)
        {
            // 成员所属部门id列表，不超过100个
            if (Department?.Length > 100)
            {
                throw new ArgumentException("The list of department ids to which members belong shall not exceed 100!");
            }
            // 部门排序列表个数必须和参数department的个数一致
            if (Order?.Length != Department?.Length)
            {
                throw new ArgumentException("The number of Order must be consistent with the number of the parameter \"department\"!");
            }
            // 部门负责人列表个数必须和参数department的个数一致
            if (IsLeaderInDept?.Length != Department?.Length)
            {
                throw new ArgumentException("The number of IsLeaderInDept must be consistent with the number of the parameter \"Department\"!");
            }
        }
        // 最多只可设置一个直属上级
        if (DirectLeader?.Length > 1)
        {
            throw new ArgumentException("At most, only one direct superior can be set up!");
        }
        // 只可存在两种值
        if (Enable.HasValue && Enable != 0)
        {
            Enable = 1;
        }
    }
}
