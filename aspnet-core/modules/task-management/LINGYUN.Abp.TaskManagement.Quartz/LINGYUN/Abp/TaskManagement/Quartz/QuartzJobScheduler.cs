using Quartz;
using System.Threading.Tasks;

namespace LINGYUN.Abp.TaskManagement.Quartz;
public class QuartzJobScheduler : IJobScheduler
{
    protected IScheduler Scheduler { get; }

    public QuartzJobScheduler(IScheduler scheduler)
    {
        Scheduler = scheduler;
    }

    public virtual async Task<bool> ExistsAsync(string group, string name)
    {
        throw new System.NotImplementedException();
    }

    public Task PauseAsync(string group, string name)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> QueueAsync(JobInfo job)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> RefreshAsync(JobInfo job)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> RemoveAsync(string group, string name)
    {
        throw new System.NotImplementedException();
    }

    public Task ResumeAsync(string group, string name)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> ShutdownAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> StartAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> StopAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task TriggerAsync(JobInfo job)
    {
        throw new System.NotImplementedException();
    }
}
