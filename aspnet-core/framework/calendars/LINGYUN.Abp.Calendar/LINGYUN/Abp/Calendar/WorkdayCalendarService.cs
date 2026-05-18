using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Calendar;

public class WorkdayCalendarService : IWorkdayCalendarService, ITransientDependency
{
    private readonly AbpCalendarOptions _options;
    private readonly IWorkdayCalendarStore _workdayCalendarStore;

    public WorkdayCalendarService(
        IOptions<AbpCalendarOptions> options, 
        IWorkdayCalendarStore workdayCalendarStore)
    {
        _options = options.Value;
        _workdayCalendarStore = workdayCalendarStore;
    }

    public async virtual Task<DateOnly> GetNextWorkdayAsync(DateOnly date)
    {
        var nextDay = date.AddDays(1);
        // 以31天为限
        var maxAttempts = 30;

        for (var i = 0; i < maxAttempts; i++)
        {
            if (await IsWorkdayAsync(nextDay))
            {
                return nextDay;
            }
            nextDay = nextDay.AddDays(1);
        }

        // 如果30天内都找不到工作日，返回原日期+1天
        return date.AddDays(1);
    }

    public async virtual Task<bool> IsWorkdayAsync(DateOnly date)
    {
        // 特殊工作日定义
        var specialDate = await _workdayCalendarStore.FindSpecialDateAsync(date);
        if (specialDate != null)
        {
            return specialDate.Type == SpecialDateType.SpecialWorkday;
        }
        // 获取默认工作日历定义
        var calendar = await _workdayCalendarStore.FindDefaultCalendarAsync();
        if (calendar == null)
        {
            // 未定义默认工作日历, 获取系统定义工作日范围
            return _options.DefaultWorkdays.Contains(date.DayOfWeek);
        }
        // 特殊工作日范围
        if (calendar.SpecialDates != null &&
            calendar.SpecialDates.Any(x => x.Date == date && x.Type == SpecialDateType.SpecialWorkday))
        {
            return true;
        }
        // 工作日范围
        if (!calendar.Workdays.Contains(date.DayOfWeek))
        {
            return false;
        }

        return false;
    }

    public async virtual Task<bool> IsInWorkTimeAsync(DateTime time)
    {
        var nowDate = DateOnly.FromDateTime(time);
        // 特殊工作日定义
        var specialDate = await _workdayCalendarStore.FindSpecialDateAsync(nowDate);
        if (specialDate != null)
        {
            return specialDate.Type == SpecialDateType.SpecialWorkday;
        }
        // 获取默认工作日历定义
        var calendar = await _workdayCalendarStore.FindDefaultCalendarAsync();
        if (calendar == null)
        {
            // 未定义默认工作日历, 获取系统定义工作日范围
            if (!_options.DefaultWorkdays.Contains(nowDate.DayOfWeek))
            {
                return false;
            }
            if (_options.EnableWorkTimeRestriction &&
                !_options.DefaultWorkTimes.Any(timeRange => timeRange.StartTime >= time.TimeOfDay && timeRange.EndTime <= time.TimeOfDay))
            {
                return false;
            }

            return true;
        }
        // 特殊工作日范围
        if (calendar.SpecialDates != null &&
            calendar.SpecialDates.Any(x => x.Date == nowDate && x.Type == SpecialDateType.SpecialWorkday))
        {
            return true;
        }
        // 工作日范围
        if (!calendar.Workdays.Contains(nowDate.DayOfWeek))
        {
            return false;
        }
        // 工作时间范围
        if (calendar.EnableWorkTimeRestriction && 
            calendar.WorkTimes != null)
        {
            return calendar.WorkTimes.Any(x => x.StartTime >= time.TimeOfDay && x.EndTime <= time.TimeOfDay);
        }

        return false;
    }
}
