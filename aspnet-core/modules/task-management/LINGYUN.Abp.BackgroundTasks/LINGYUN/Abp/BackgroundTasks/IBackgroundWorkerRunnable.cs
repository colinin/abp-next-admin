using Volo.Abp.BackgroundWorkers;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IBackgroundWorkerRunnable : IJobRunnable
{
    JobInfo? BuildWorker(IBackgroundWorker worker);
}
