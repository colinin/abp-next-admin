using System;

namespace LINGYUN.Abp.Calendar;

public  class AbpCalendarOptions
{
    /// <summary>
    /// 默认工作日范围
    /// 默认: 周一到周五
    /// </summary>
    public DayOfWeek[] DefaultWorkdays { get; set; }
    /// <summary>
    /// 是否启用工作时间限制
    /// 默认: true
    /// </summary>
    public bool EnableWorkTimeRestriction { get; set; }
    /// <summary>
    /// 默认工作时间范围
    /// 默认: 09:00-12:00、13:00-18:00
    /// </summary>
    public WorkTimeRange[] DefaultWorkTimes { get; set; }

    public AbpCalendarOptions()
    {
        DefaultWorkdays = new[]
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };
        EnableWorkTimeRestriction = true;
        DefaultWorkTimes = new[]
        {
            new WorkTimeRange(new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0)),
            new WorkTimeRange(new TimeSpan(13, 0, 0), new TimeSpan(18, 0, 0)),
        };
    }
}
