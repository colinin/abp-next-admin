using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[DisableAuditing]
[DisableJobAction]
public class BackgroundCheckingJob : IJobRunnable
{
    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        try
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundTasksOptions>>().Value;
            var store = context.ServiceProvider.GetRequiredService<IJobStore>();
            var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();

            context.TryGetMultiTenantId(out var tenantId);

            using (currentTenant.Change(tenantId))
            {
                var runingTasks = await store.GetRuningListAsync(
                    options.MaxJobCheckCount, context.CancellationToken);

                if (!runingTasks.Any())
                {
                    return;
                }

                var jobScheduler = context.ServiceProvider.GetRequiredService<IJobScheduler>();

                foreach (var job in runingTasks)
                {
                    // 当标记为运行中的作业不在调度器中时，改变为已停止作业
                    if (!await jobScheduler.ExistsAsync(job, context.CancellationToken))
                    {
                        job.Status = JobStatus.Stopped;

                        await store.StoreAsync(job);
                    }
                }
            }
        }
        catch(Exception ex)
        {
            context.ServiceProvider
                .GetService<ILogger<BackgroundCheckingJob>>()
                ?.LogError(ex, "check job status error.");
        }
    }
}
