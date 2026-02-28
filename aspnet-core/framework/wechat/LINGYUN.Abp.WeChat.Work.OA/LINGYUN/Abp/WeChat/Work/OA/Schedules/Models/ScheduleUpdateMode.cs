using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日程更新操作模式
/// </summary>
[Description("日程更新操作模式")]
public enum ScheduleUpdateMode : uint
{
    /// <summary>
    /// 全部修改
    /// </summary>
    [Description("全部修改")]
    ModifyAll = 0,
    /// <summary>
    /// 仅修改此日程
    /// </summary>
    [Description("仅修改此日程")]
    ModifyThisSchedule = 1,
    /// <summary>
    /// 修改将来的所有日程
    /// </summary>
    [Description("修改将来的所有日程")]
    ModifyAllFutureSchedules = 2,
}
