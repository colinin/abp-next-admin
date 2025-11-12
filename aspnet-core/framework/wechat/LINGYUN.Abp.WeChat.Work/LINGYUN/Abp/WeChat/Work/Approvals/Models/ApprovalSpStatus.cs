using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 申请单状态
/// </summary>
public enum ApprovalSpStatus : byte
{
    /// <summary>
    /// 审批中
    /// </summary>
    [Description("审批中")]
    UnderApproval = 1,
    /// <summary>
    /// 已通过
    /// </summary>
    [Description("已通过")]
    Passed = 2,
    /// <summary>
    /// 已驳回
    /// </summary>
    [Description("已驳回")]
    Rejected = 3,
    /// <summary>
    /// 已撤销
    /// </summary>
    [Description("已撤销")]
    Revoked = 4,
    /// <summary>
    /// 通过后撤销
    /// </summary>
    [Description("通过后撤销")]
    RevokeAfterApproval = 6,
    /// <summary>
    /// 已删除
    /// </summary>
    [Description("已删除")]
    Deleted = 7,
    /// <summary>
    /// 已支付
    /// </summary>
    [Description("已支付")]
    Paid = 10,
}
