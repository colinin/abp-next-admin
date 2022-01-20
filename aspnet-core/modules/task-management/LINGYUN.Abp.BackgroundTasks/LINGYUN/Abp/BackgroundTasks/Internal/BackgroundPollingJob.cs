using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[DisableAuditing]
public class BackgroundPollingJob : IJobRunnable
{
    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundTasksOptions>>().Value;
        var store = context.ServiceProvider.GetRequiredService<IJobStore>();
        var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();

        context.TryGetMultiTenantId(out var tenantId);

        using (currentTenant.Change(tenantId))
        {
            var waitingJobs = await store.GetWaitingListAsync(options.MaxJobFetchCount);

            if (!waitingJobs.Any())
            {
                return;
            }

            var jobPublisher = context.ServiceProvider.GetRequiredService<IJobPublisher>();

            foreach (var job in waitingJobs)
            {
                await jobPublisher.PublishAsync(job);
            }
        }
    }
}
