using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Calendar;

public interface IWorkdayCalendarService
{
    /// <summary>
    /// 检查指定时间是否在工作日
    /// </summary>
    Task<bool> IsWorkdayAsync(DateOnly date);
    /// <summary>
    /// 检查指定时间是否在工作时间
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    Task<bool> IsInWorkTimeAsync(DateTime dateTime);
    /// <summary>
    /// 获取下一个工作日
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    Task<DateOnly> GetNextWorkdayAsync(DateOnly date);
}
