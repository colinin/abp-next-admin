using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobRunnableExecuter : IJobRunnableExecuter, ISingletonDependency
{
    protected const string LockKeyFormat = "job:{0},key:{1}";

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
            context.JobData.TryGetValue(nameof(JobInfo.LockTimeOut), out var lockTime);

            if (lockTime != null && (lockTime is int time && time > 0))
            {
                var jobId = context.JobData.GetOrDefault(nameof(JobInfo.Id));
                var jobLockKey = string.Format(LockKeyFormat, context.JobType.Name, jobId);
                var distributedLock = context.ServiceProvider.GetRequiredService<IAbpDistributedLock>();
                await using (await distributedLock.TryAcquireAsync(jobLockKey, TimeSpan.FromSeconds(time)))
                {
                    await InternalExecuteAsync(context);
                }
            }
            else
            {
                await InternalExecuteAsync(context);
            }
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
