using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobRunnableExecuter : IJobRunnableExecuter, ITransientDependency
{
    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();

        context.TryGetMultiTenantId(out var tenantId);

        using (currentTenant.Change(tenantId))
        {
            await InternalExecuteAsync(context);
        }
    }

    private async static Task InternalExecuteAsync(JobRunnableContext context)
    {
        var jobRunnable = context.ServiceProvider.GetService(context.JobType)
            ?? Activator.CreateInstance(context.JobType);

        Check.NotNull(jobRunnable, nameof(jobRunnable));

        await ((IJobRunnable)jobRunnable).ExecuteAsync(context);
    }
}
