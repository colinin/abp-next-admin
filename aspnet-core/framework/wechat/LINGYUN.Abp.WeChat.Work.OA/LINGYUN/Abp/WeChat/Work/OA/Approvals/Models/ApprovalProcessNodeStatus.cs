using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 节点状态
/// </summary>
public enum ApprovalProcessNodeStatus : byte
{
    /// <summary>
    /// 审批中
    /// </summary>
    [Description("审批中")]
    UnderApproval = 1,
    /// <summary>
    /// 同意
    /// </summary>
    [Description("同意")]
    Agree = 2,
    /// <summary>
    /// 驳回
    /// </summary>
    [Description("驳回")]
    Rejection = 3,
    /// <summary>
    /// 转审
    /// </summary>
    [Description("转审")]
    TransferForReview = 4,
    /// <summary>
    /// 退回给指定审批人
    /// </summary>
    [Description("退回给指定审批人")]
    ReturnToApprover = 11,
    /// <summary>
    /// 加签
    /// </summary>
    [Description("加签")]
    Additional = 12,
    /// <summary>
    /// 同意并加签
    /// </summary>
    [Description("同意并加签")]
    AgreeAndSign = 13,
    /// <summary>
    /// 办理
    /// </summary>
    [Description("办理")]
    Processing = 14,
    /// <summary>
    /// 转交
    /// </summary>
    [Description("转交")]
    Transfer = 15,
}
