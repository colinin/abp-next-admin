using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[DisableAuditing]
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
