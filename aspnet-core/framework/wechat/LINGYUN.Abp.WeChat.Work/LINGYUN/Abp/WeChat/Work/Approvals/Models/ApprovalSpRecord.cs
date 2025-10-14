using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 审批流程信息
/// </summary>
public class ApprovalSpRecord
{
    /// <summary>
    /// 审批节点状态
    /// </summary>
    [NotNull]
    [JsonProperty("sp_status")]
    [JsonPropertyName("sp_status")]
    public ApprovalSpRecordStatus SpStatus { get; set; }
    /// <summary>
    /// 节点审批方式
    /// </summary>
    [NotNull]
    [JsonProperty("approverattr")]
    [JsonPropertyName("approverattr")]
    public ApprovalApproverAttr ApproverAttr { get; set; }
    /// <summary>
    /// 审批节点详情,一个审批节点有多个审批人
    /// </summary>
    [NotNull]
    [JsonProperty("details")]
    [JsonPropertyName("details")]
    public List<ApprovalSpRecordDetail> Details { get; set; }
}
