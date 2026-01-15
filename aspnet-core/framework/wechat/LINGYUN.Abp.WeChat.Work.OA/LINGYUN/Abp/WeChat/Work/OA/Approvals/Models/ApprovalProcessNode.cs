using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 流程节点
/// </summary>
public class ApprovalProcessNode
{
    /// <summary>
    /// 节点类型 1:审批人 2:抄送人 3:办理人
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public ApprovalProcessNodeType Type { get; set; }
    /// <summary>
    /// 节点状态 1-审批中；2-同意；3-驳回；4-转审；11-退回给指定审批人；12-加签；13-同意并加签；14-办理；15-转交
    /// </summary>
    [NotNull]
    [JsonProperty("sp_status")]
    [JsonPropertyName("sp_status")]
    public ApprovalProcessNodeStatus Status { get; set; }
    /// <summary>
    /// 多人审批方式 1-会签；2-或签 3-依次审批
    /// </summary>
    [CanBeNull]
    [JsonProperty("apv_rel")]
    [JsonPropertyName("apv_rel")]
    public byte? ApvRel { get; set; }
    /// <summary>
    /// 子节点列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("sub_node_list")]
    [JsonPropertyName("sub_node_list")]
    public List<ApprovalProcessSubNode> SubNodes { get; set; }
}
