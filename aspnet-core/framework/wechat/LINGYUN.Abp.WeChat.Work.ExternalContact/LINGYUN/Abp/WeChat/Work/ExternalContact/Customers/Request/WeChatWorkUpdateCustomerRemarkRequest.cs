using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
/// <summary>
/// 修改客户备注信息请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92115" />
/// </remarks>
public class WeChatWorkUpdateCustomerRemarkRequest : WeChatWorkRequest
{
    /// <summary>
    /// 企业成员的userid
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; }
    /// <summary>
    /// 外部联系人userid
    /// </summary>
    [NotNull]
    [JsonProperty("external_userid")]
    [JsonPropertyName("external_userid")]
    public string ExternalUserId { get; }
    /// <summary>
    /// 此用户对外部联系人的备注，最多20个字符
    /// </summary>
    [CanBeNull]
    [StringLength(20)]
    [JsonProperty("remark")]
    [JsonPropertyName("remark")]
    public string? Remark { get; }
    /// <summary>
    /// 此用户对外部联系人的描述，最多150个字符
    /// </summary>
    [CanBeNull]
    [StringLength(150)]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string? Description { get; }
    /// <summary>
    /// 此用户对外部联系人备注的所属公司名称，最多20个字符
    /// </summary>
    [CanBeNull]
    [StringLength(20)]
    [JsonProperty("remark_company")]
    [JsonPropertyName("remark_company")]
    public string? RemarkCompany { get; }
    /// <summary>
    /// 此用户对外部联系人备注的手机号
    /// </summary>
    [CanBeNull]
    [JsonProperty("remark_mobiles")]
    [JsonPropertyName("remark_mobiles")]
    public List<string>? RemarkMobiles { get; }
    /// <summary>
    /// 备注图片的mediaid
    /// </summary>
    [CanBeNull]
    [JsonProperty("remark_pic_mediaid")]
    [JsonPropertyName("remark_pic_mediaid")]
    public string? RemarkPictureMediaId { get; }
    public WeChatWorkUpdateCustomerRemarkRequest(
        string userId, 
        string externalUserId, 
        string? remark = null, 
        string? description = null, 
        string? remarkCompany = null,
        List<string>? remarkMobiles = null, 
        string? remarkPictureMediaId = null)
    {
        UserId = Check.NotNullOrWhiteSpace(userId, nameof(userId));
        ExternalUserId = Check.NotNullOrWhiteSpace(externalUserId, nameof(externalUserId));
        Remark = Check.Length(remark, nameof(remark), 20);
        Description = Check.Length(description, nameof(description), 150);
        RemarkCompany = Check.Length(remarkCompany, nameof(remarkCompany), 20);
        RemarkMobiles = remarkMobiles;
        RemarkPictureMediaId = remarkPictureMediaId;
    }
}
