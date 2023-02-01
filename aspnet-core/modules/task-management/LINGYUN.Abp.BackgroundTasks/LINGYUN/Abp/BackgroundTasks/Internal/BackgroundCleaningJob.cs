using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[DisableAuditing]
[DisableJobAction]
public class BackgroundCleaningJob : IJobRunnable
{
    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundTasksOptions>>().Value;
        var store = context.ServiceProvider.GetRequiredService<IJobStore>();
        var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();

        context.TryGetMultiTenantId(out var tenantId);

        using (currentTenant.Change(tenantId))
        {
            var expiredJobs = await store.CleanupAsync(
                options.MaxJobCleanCount,
                options.JobExpiratime,
                context.CancellationToken);

            var jobScheduler = context.ServiceProvider.GetRequiredService<IJobScheduler>();

            foreach (var expiredJob in expiredJobs)
            {
                // 从队列强制移除作业
                await jobScheduler.RemoveAsync(expiredJob, context.CancellationToken);
            }
        }
    }
}
