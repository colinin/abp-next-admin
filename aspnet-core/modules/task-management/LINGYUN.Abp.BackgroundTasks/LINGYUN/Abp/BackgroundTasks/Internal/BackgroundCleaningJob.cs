using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

internal class BackgroundCleaningJob : IJobRunnable
{
    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundTasksOptions>>().Value;
        var store = context.ServiceProvider.GetRequiredService<IJobStore>();

        await store.CleanupAsync(
            options.MaxJobCleanCount,
            options.JobExpiratime);
    }
}
