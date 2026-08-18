using Quartz;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzCronValidator : ICronValidator
{
    public virtual Task<bool> ValidateAsync(string? cron)
    {
        if (cron.IsNullOrWhiteSpace())
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(CronExpression.IsValidExpression(cron));
    }
}
