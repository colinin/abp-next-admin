using System;

namespace LINGYUN.Abp.Calendar;

/// <summary>
/// 工作日历信息
/// </summary>
public class WorkdayCalendarInfo
{
    /// <summary>
    /// 日历名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 默认日历
    /// </summary>
    public bool IsDefault { get; set; }
    /// <summary>
    /// 工作日范围
    /// </summary>
    public DayOfWeek[] Workdays { get; set; }
    /// <summary>
    /// 工作时间范围
    /// </summary>
    public WorkTimeRange[]? WorkTimes { get; set; }
    /// <summary>
    /// 是否启用工作时间限制
    /// </summary>
    public bool EnableWorkTimeRestriction { get; set; }
    /// <summary>
    /// 特殊日期列表
    /// </summary>
    public SpecialDateInfo[]? SpecialDates { get; set; }
}
