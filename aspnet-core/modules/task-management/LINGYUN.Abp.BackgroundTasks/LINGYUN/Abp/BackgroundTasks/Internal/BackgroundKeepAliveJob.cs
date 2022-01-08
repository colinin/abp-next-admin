using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[DisableAuditing]
internal class BackgroundKeepAliveJob : IJobRunnable
{
    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        var store = context.ServiceProvider.GetRequiredService<IJobStore>();

        var periodJobs = await store.GetAllPeriodTasksAsync();

        if (!periodJobs.Any())
        {
            return;
        }

        var jobScheduler = context.ServiceProvider.GetRequiredService<IJobScheduler>();

        await jobScheduler.QueuesAsync(periodJobs);
    }
}
