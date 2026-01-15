using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 子节点
/// </summary>
public class ApprovalProcessSubNode
{
    /// <summary>
    /// 处理人信息
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 审批/办理意见
    /// </summary>
    [NotNull]
    [JsonProperty("speech")]
    [JsonPropertyName("speech")]
    public string Speech { get; set; }
    /// <summary>
    /// 子节点状态 1-审批中；2-同意；3-驳回；4-转审；11-退回给指定审批人；12-加签；13-同意并加签；14-办理；15-转交
    /// </summary>
    [NotNull]
    [JsonProperty("sp_yj")]
    [JsonPropertyName("sp_yj")]
    public ApprovalProcessNodeStatus Status { get; set; }
    /// <summary>
    /// 操作时间
    /// </summary>
    [NotNull]
    [JsonProperty("sptime")]
    [JsonPropertyName("sptime")]
    public long SpTime { get; set; }
    /// <summary>
    /// 附件，微盘文件无法获取
    /// </summary>
    [CanBeNull]
    [JsonProperty("media_ids")]
    [JsonPropertyName("media_ids")]
    public List<string> MediaIds { get; set; }
}
