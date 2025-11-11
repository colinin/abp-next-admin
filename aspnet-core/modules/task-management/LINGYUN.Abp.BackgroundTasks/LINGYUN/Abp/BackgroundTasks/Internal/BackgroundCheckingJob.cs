using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[DisableAuditing]
[DisableJobAction]
public class BackgroundCheckingJob : IJobRunnable
{
    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        context.TryGetMultiTenantId(out var tenantId);

        var jobDispatcher = context.GetRequiredService<IJobDispatcher>();
        var jobStateChecker = context.GetRequiredService<IJobStateChecker>();

        await jobStateChecker.CheckRuningJobAsync(tenantId, context.CancellationToken);

        await jobDispatcher.CheckRuningJobAsync(tenantId, context.CancellationToken);
    }
}
