using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 审批节点状态
/// </summary>
public enum ApprovalSpRecordStatus : byte
{
    /// <summary>
    /// 审批中
    /// </summary>
    [Description("审批中")]
    UnderApproval = 1,
    /// <summary>
    /// 已同意
    /// </summary>
    [Description("已同意")]
    Passed = 2,
    /// <summary>
    /// 已驳回
    /// </summary>
    [Description("已驳回")]
    Rejected = 3,
    /// <summary>
    /// 已转审
    /// </summary>
    [Description("已转审")]
    Transferred = 4,
    /// <summary>
    /// 已退回
    /// </summary>
    [Description("已退回")]
    Returned = 11,
    /// <summary>
    /// 已加签
    /// </summary>
    [Description("已加签")]
    Signed = 12,
    /// <summary>
    /// 已同意并加签
    /// </summary>
    [Description("已同意并加签")]
    AgreedAndSigned = 13,
}
