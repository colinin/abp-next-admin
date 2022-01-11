using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobRunnableExecuter : IJobRunnableExecuter, ISingletonDependency
{
    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        Guid? tenantId = null;
        if (context.JobData.TryGetValue(nameof(IMultiTenant.TenantId), out var tenant) &&
            Guid.TryParse(tenant?.ToString(), out var tid))
        {
            tenantId = tid;
        }

        var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();
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
