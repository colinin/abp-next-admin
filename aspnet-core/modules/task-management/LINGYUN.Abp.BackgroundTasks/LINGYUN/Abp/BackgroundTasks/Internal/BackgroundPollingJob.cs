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

        // TODO: 如果积压有大量持续性任务, 可能后面的队列无法被检索到
        // 尽量让任务重复次数在可控范围内
        // 需要借助队列提供者来持久化已入队任务
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
