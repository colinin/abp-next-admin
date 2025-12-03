using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 重复类型
/// </summary>
[Description("重复类型")]
public enum ScheduleReminderRepeatType : uint
{
    /// <summary>
    /// 每日
    /// </summary>
    [Description("每日")]
    Daily = 0,
    /// <summary>
    /// 每周
    /// </summary>
    [Description("每周")]
    Weekly = 1,
    /// <summary>
    /// 每月
    /// </summary>
    [Description("每月")]
    EveryMonth = 2,
    /// <summary>
    /// 每年
    /// </summary>
    [Description("每年")]
    EveryYear = 5,
    /// <summary>
    /// 工作日
    /// </summary>
    [Description("工作日")]
    WorkingDays = 7,
}
