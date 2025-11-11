using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Models;
/// <summary>
/// 客户群跟进状态
/// </summary>
[Description("客户群跟进状态")]
public enum GroupChatStatus
{
    /// <summary>
    /// 跟进人正常
    /// </summary>
    [Description("跟进人正常")]
    Normal = 0,
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
