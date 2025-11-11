using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Models;
/// <summary>
/// 客户群跟进状态过滤
/// </summary>
[Description("客户群跟进状态过滤")]
public enum StatusFilter
{
    /// <summary>
    /// 所有列表
    /// </summary>
    [Description("所有列表")]
    All = 0,
    /// <summary>
    /// 跟进人离职
    /// </summary>
    [Description("跟进人离职")]
    Leaves = 1,
    /// <summary>
    /// 离职继承中
    /// </summary>
    [Description("离职继承中")]
    Resiging = 2,
    /// <summary>
    /// 离职继承完成
    /// </summary>
    [Description(" 离职继承完成")]
    ResignCompleted = 3,
}
