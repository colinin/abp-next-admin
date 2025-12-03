using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 审批申请备注信息
/// </summary>
public class ApprovalComment
{
    /// <summary>
    /// 备注人信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("commentUserInfo")]
    [JsonPropertyName("commentUserInfo")]
    public ApprovalUser CommentUserInfo { get; set; }
    /// <summary>
    /// 审批申请提交时间,Unix时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("commenttime")]
    [JsonPropertyName("commenttime")]
    public long CommentTime { get; set; }
    /// <summary>
    /// 备注文本内容
    /// </summary>
    [NotNull]
    [JsonProperty("commentcontent")]
    [JsonPropertyName("commentcontent")]
    public string CommentContent { get; set; }
    /// <summary>
    /// 备注id
    /// </summary>
    [NotNull]
    [JsonProperty("commentid")]
    [JsonPropertyName("commentid")]
    public string CommentId { get; set; }
    /// <summary>
    /// 备注附件id，可能有多个，微盘文件无法获取
    /// </summary>
    [CanBeNull]
    [JsonProperty("media_id")]
    [JsonPropertyName("media_id")]
    public List<string> MediaId { get; set; }
}
