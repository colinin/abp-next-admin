using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 节点类型
/// </summary>
[Description("节点类型")]
public enum CustomerStrategyRangeType
{
    /// <summary>
    /// 成员
    /// </summary>
    [Description("成员")]
    Member = 1,
    /// <summary>
    /// 部门
    /// </summary>
    [Description("部门")]
    Part = 2
}
