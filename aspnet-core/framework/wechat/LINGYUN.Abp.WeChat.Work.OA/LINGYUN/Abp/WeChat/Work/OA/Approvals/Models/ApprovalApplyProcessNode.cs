using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 申请流程节点
/// </summary>
public class ApprovalApplyProcessNode
{
    /// <summary>
    /// 节点类型 1:审批人 2:抄送人 3:办理人
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public ApprovalProcessNodeType Type { get; set; }
    /// <summary>
    /// 多人审批方式 1-会签；2-或签 3-依次审批
    /// </summary>
    /// <remarks>
    /// type为1、3时必填	
    /// </remarks>
    [NotNull]
    [JsonProperty("apv_rel")]
    [JsonPropertyName("apv_rel")]
    public byte? ApvRel { get; set; }
    /// <summary>
    /// 用户id
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    public ApprovalApplyProcessNode()
    {

    }

    private ApprovalApplyProcessNode(ApprovalProcessNodeType type, string userId, byte? apvRel = null)
    {
        Type = type;
        UserId = userId;
        ApvRel = apvRel;
    }
    /// <summary>
    /// 审批人节点
    /// </summary>
    /// <param name="userId">审批人Id</param>
    /// <param name="apvRel">多人审批方式 1-会签；2-或签 3-依次审批</param>
    /// <returns></returns>
    public static ApprovalApplyProcessNode Approver(string userId, byte apvRel)
    {
        return new ApprovalApplyProcessNode(ApprovalProcessNodeType.Approver, userId, apvRel);
    }
    /// <summary>
    /// 办理人节点
    /// </summary>
    /// <param name="userId">办理人Id</param>
    /// <param name="apvRel">多人审批方式 1-会签；2-或签 3-依次审批</param>
    /// <returns></returns>
    public static ApprovalApplyProcessNode Handler(string userId, byte apvRel)
    {
        return new ApprovalApplyProcessNode(ApprovalProcessNodeType.Handler, userId, apvRel);
    }
    /// <summary>
    /// 抄送人节点
    /// </summary>
    /// <param name="userId">抄送人Id</param>
    /// <returns></returns>
    public static ApprovalApplyProcessNode To(string userId)
    {
        return new ApprovalApplyProcessNode(ApprovalProcessNodeType.CC, userId);
    }
}
