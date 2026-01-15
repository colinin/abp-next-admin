using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 节点审批方式
/// </summary>
public enum ApprovalApproverAttr : byte
{
    /// <summary>
    /// 或签
    /// </summary>
    [Description("或签")]
    OrSign = 1,
    /// <summary>
    /// 会签
    /// </summary>
    [Description("会签")]
    CoSign = 2,
}
