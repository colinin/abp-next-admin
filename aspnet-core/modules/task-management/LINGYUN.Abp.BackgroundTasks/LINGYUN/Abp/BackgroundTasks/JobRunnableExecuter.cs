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
    protected const string LockKeyFormat = "p:{0},job:{1},key:{2}";

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

            // 某些提供者如果无法保证锁一致性, 那么需要用分布式锁
            if (lockTime != null && (lockTime is int time && time > 0))
            {
                var jobId = context.JobData.GetOrDefault(nameof(JobInfo.Id));
                var jobLockKey = string.Format(LockKeyFormat, tenantId?.ToString() ?? "Default", context.JobType.Name, jobId);
                var distributedLock = context.ServiceProvider.GetRequiredService<IAbpDistributedLock>();

                var handle = await distributedLock.TryAcquireAsync(jobLockKey, TimeSpan.FromSeconds(time));
                if (handle == null)
                {
                    // 抛出异常 通过监听器使其重试
                    throw new AbpBackgroundTaskConcurrentException(context.JobType);
                }

                await using (handle)
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
