using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Calendar;

[Dependency(TryRegister = true)]
public class NullWorkdayCalendarStore : IWorkdayCalendarStore, ISingletonDependency
{
    public Task<WorkdayCalendarInfo?> FindDefaultCalendarAsync()
    {
        return Task.FromResult<WorkdayCalendarInfo?>(null);
    }

    public Task<SpecialDateInfo?> FindSpecialDateAsync(DateOnly date)
    {
        return Task.FromResult<SpecialDateInfo?>(null);
    }
}
