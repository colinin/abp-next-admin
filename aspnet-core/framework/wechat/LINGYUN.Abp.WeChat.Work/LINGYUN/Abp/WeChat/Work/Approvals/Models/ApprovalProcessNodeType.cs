using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 节点类型
/// </summary>
public enum ApprovalProcessNodeType : byte
{
    /// <summary>
    /// 审批人
    /// </summary>
    [Description("审批人")]
    Approver = 1,
    /// <summary>
    /// 抄送人
    /// </summary>
    [Description("抄送人")]
    CC = 2,
    /// <summary>
    /// 办理人
    /// </summary>
    [Description("办理人")]
    Handler = 3,
}
