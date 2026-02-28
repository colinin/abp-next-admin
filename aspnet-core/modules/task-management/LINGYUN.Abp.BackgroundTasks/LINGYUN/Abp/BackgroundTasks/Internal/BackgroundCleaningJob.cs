using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[DisableAuditing]
[DisableJobAction]
public class BackgroundCleaningJob : IJobRunnable
{
    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        context.TryGetMultiTenantId(out var tenantId);

        var jobDispatcher = context.GetRequiredService<IJobDispatcher>();
        var jobStateChecker = context.GetRequiredService<IJobStateChecker>();

        await jobStateChecker.CleanExpiredJobAsync(tenantId, context.CancellationToken);

        await jobDispatcher.CleanExpiredJobAsync(tenantId, context.CancellationToken);
    }
}
