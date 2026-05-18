using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Calendar;

public interface IWorkdayCalendarStore
{
    /// <summary>
    /// 获取特殊日期
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns></returns>
    Task<SpecialDateInfo?> FindSpecialDateAsync(DateOnly date);
    /// <summary>
    /// 获取默认工作日历
    /// </summary>
    Task<WorkdayCalendarInfo?> FindDefaultCalendarAsync();
}
