using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日程删除操作模式
/// </summary>
[Description("日程删除操作模式")]
public enum ScheduleDeleteMode : uint
{
    /// <summary>
    /// 删除所有日程
    /// </summary>
    [Description("删除所有日程")]
    DeleteAll = 0,
    /// <summary>
    /// 仅删除此日程
    /// </summary>
    [Description("仅删除此日程")]
    DeleteThisSchedule = 1,
    /// <summary>
    /// 删除本次及后续日程
    /// </summary>
    [Description("删除本次及后续日程")]
    DeleteAllFutureSchedules = 2,
}
