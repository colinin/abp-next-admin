using LINGYUN.Abp.BackgroundTasks;
using Quartz;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.TaskManagement.Quartz;
public class QuartzJobScheduler : IJobScheduler
{
    protected IScheduler Scheduler { get; }

    public QuartzJobScheduler(IScheduler scheduler)
    {
        Scheduler = scheduler;
    }

    public Task<bool> QueueAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task QueuesAsync(IEnumerable<JobInfo> jobs, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> ExistsAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task TriggerAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task PauseAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task ResumeAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> RemoveAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> StartAsync(CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> StopAsync(CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> ShutdownAsync(CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }
}
