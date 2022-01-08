using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[DisableAuditing]
internal class BackgroundPollingJob : IJobRunnable
{
    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundTasksOptions>>().Value;
        var store = context.ServiceProvider.GetRequiredService<IJobStore>();

        var waitingJobs = await store.GetWaitingListAsync(options.MaxJobFetchCount);

        if (!waitingJobs.Any())
        {
            return;
        }

        var jobScheduler = context.ServiceProvider.GetRequiredService<IJobScheduler>();

        foreach (var job in waitingJobs)
        {
            await jobScheduler.QueueAsync(job);
        }
    }
}
