using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
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

    private async Task InternalExecuteAsync(JobRunnableContext context)
    {
        var jobRunnable = context.ServiceProvider.GetService(context.JobType);
        if (jobRunnable == null)
        {
            jobRunnable = Activator.CreateInstance(context.JobType);
        }
        await ((IJobRunnable)jobRunnable).ExecuteAsync(context);
    }
}
