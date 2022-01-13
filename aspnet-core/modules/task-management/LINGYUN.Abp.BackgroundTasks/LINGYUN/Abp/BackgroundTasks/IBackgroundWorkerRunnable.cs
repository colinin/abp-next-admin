using Volo.Abp.BackgroundWorkers;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IBackgroundWorkerRunnable : IJobRunnable
{
#nullable enable
    JobInfo? BuildWorker(IBackgroundWorker worker);
#nullable disable
}
