using System;

namespace LINGYUN.Abp.Calendar;
/// <summary>
/// 工作时间范围
/// </summary>
public class WorkTimeRange
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public TimeSpan StartTime { get; set; }
    /// <summary>
    /// 截止时间
    /// </summary>
    public TimeSpan EndTime { get; set; }
    public WorkTimeRange()
    {

    }

    public WorkTimeRange(TimeSpan startTime, TimeSpan endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
}
