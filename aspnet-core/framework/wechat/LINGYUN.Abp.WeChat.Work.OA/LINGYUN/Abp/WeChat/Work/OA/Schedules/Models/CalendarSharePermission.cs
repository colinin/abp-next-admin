using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日历通知范围成员权限
/// </summary>
[Description("日历通知范围成员权限")]
public enum CalendarSharePermission
{
    /// <summary>
    /// 可查看
    /// </summary>
    [Description("可查看")]
    View = 1,
    /// <summary>
    /// 仅查看闲忙状态
    /// </summary>
    [Description("仅查看闲忙状态")]
    OnlyBusy = 3
}
