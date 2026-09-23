using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(TryRegister = true)]
public class CronNotNullValidator : ICronValidator, ISingletonDependency
{
    public Task<bool> ValidateAsync(string? cron)
    {
        return Task.FromResult(!cron.IsNullOrWhiteSpace());
    }
}
