using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(TryRegister = true)]
public class NullJobScheduler : IJobScheduler, ISingletonDependency
{
    public Task<bool> ExistsAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task PauseAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<bool> QueueAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task QueuesAsync(IEnumerable<JobInfo> jobs, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<bool> RemoveAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task ResumeAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<bool> ShutdownAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task<bool> StartAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task<bool> StopAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task TriggerAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
