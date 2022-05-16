using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[Dependency(TryRegister = true)]
internal class InMemoryJobStore : IJobStore, ISingletonDependency
{
    private readonly List<JobInfo> _memoryJobStore;

    public InMemoryJobStore()
    {
        _memoryJobStore = new List<JobInfo>();
    }

    public Task<List<JobInfo>> GetAllPeriodTasksAsync(CancellationToken cancellationToken = default)
    {
        var status = new JobStatus[] { JobStatus.Running, JobStatus.FailedRetry };

        var jobs = _memoryJobStore
            .Where(x => !x.IsAbandoned)
            .Where(x => x.JobType == JobType.Period && status.Contains(x.Status))
            .Where(x => (x.MaxCount == 0 || x.TriggerCount < x.MaxCount) || (x.MaxTryCount == 0 || x.TryCount < x.MaxTryCount))
            .OrderByDescending(x => x.Priority)
            .ToList();

        return Task.FromResult(jobs);
    }

    public Task<List<JobInfo>> GetWaitingListAsync(int maxResultCount, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var status = new JobStatus[] { JobStatus.Running, JobStatus.FailedRetry };

        var jobs = _memoryJobStore
            .Where(x => !x.IsAbandoned)
            .Where(x => x.JobType != JobType.Period && status.Contains(x.Status))
            .Where(x => (x.MaxCount == 0 || x.TriggerCount < x.MaxCount) || (x.MaxTryCount == 0 || x.TryCount < x.MaxTryCount))
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.TryCount)
            .ThenBy(x => x.NextRunTime)
            .Take(maxResultCount)
            .ToList();

        return Task.FromResult(jobs);
    }

    public Task<JobInfo> FindAsync(string jobId)
    {
        var job = _memoryJobStore.FirstOrDefault(x => x.Id.Equals(jobId));
        return Task.FromResult(job);
    }

    public Task StoreAsync(JobInfo jobInfo)
    {
        var job = _memoryJobStore.FirstOrDefault(x => x.Id.Equals(jobInfo.Id));
        if (job != null)
        {
            job.NextRunTime = jobInfo.NextRunTime;
            job.LastRunTime = jobInfo.LastRunTime;
            job.Status = jobInfo.Status;
            job.TriggerCount = jobInfo.TriggerCount;
            job.TryCount = jobInfo.TryCount;
            job.IsAbandoned = jobInfo.IsAbandoned;
        }
        else
        {
            _memoryJobStore.Add(jobInfo);
        }
        return Task.CompletedTask;
    }

    public async Task RemoveAsync(string jobId)
    {
        var job = await FindAsync(jobId);
        if (job != null)
        {
            _memoryJobStore.Remove(job);
        }
    }

    public Task StoreLogAsync(JobEventData eventData)
    {
        return Task.CompletedTask;
    }

    public Task CleanupAsync(int maxResultCount, TimeSpan jobExpiratime, CancellationToken cancellationToken = default)
    {
        var expiratime = DateTime.Now - jobExpiratime;

        var expriaJobs = _memoryJobStore
            .Where(x => x.Status == JobStatus.Completed &&
                expiratime.CompareTo(x.LastRunTime ?? x.EndTime ?? x.CreationTime) <= 0)
            .Take(maxResultCount);

        _memoryJobStore.RemoveAll(expriaJobs);

        return Task.CompletedTask;
    }
}
