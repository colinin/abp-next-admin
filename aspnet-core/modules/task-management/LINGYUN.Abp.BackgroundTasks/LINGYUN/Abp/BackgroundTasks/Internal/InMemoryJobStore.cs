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
    private readonly static object _jobSync = new();

    public InMemoryJobStore()
    {
        _memoryJobStore = new List<JobInfo>();
    }

    public virtual Task<List<JobInfo>> GetAllPeriodTasksAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var status = new JobStatus[] { JobStatus.Queuing, JobStatus.FailedRetry };

        var jobs = _memoryJobStore
            .Where(x => !x.IsAbandoned)
            .Where(x => x.JobType == JobType.Period && status.Contains(x.Status))
            .Where(x => (x.MaxCount == 0 || x.TriggerCount < x.MaxCount) || (x.MaxTryCount == 0 || x.TryCount < x.MaxTryCount))
            .OrderByDescending(x => x.Priority)
            .ToList();

        return Task.FromResult(jobs);
    }

    public virtual Task<List<JobInfo>> GetRuningListAsync(int maxResultCount, string nodeName = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var status = new JobStatus[] { JobStatus.Running };

        var jobs = _memoryJobStore
            .Where(x => x.NodeName == nodeName && status.Contains(x.Status))
            .Take(maxResultCount)
            .ToList();

        return Task.FromResult(jobs);
    }

    public virtual Task<List<JobInfo>> GetWaitingListAsync(int maxResultCount, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var now = DateTime.Now;
        var status = new JobStatus[] { JobStatus.Queuing, JobStatus.FailedRetry };

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

    public Task<JobInfo> FindAsync(
        string jobId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var job = _memoryJobStore.FirstOrDefault(x => x.Id.Equals(jobId));
        return Task.FromResult(job);
    }

    public virtual Task StoreAsync(
        JobInfo jobInfo,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock(_jobSync)
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
        }

        return Task.CompletedTask;
    }

    public virtual Task RemoveAsync(
        string jobId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (_jobSync)
        {
            var job = _memoryJobStore.FirstOrDefault(x => x.Id.Equals(jobId));
            if (job != null)
            {
                _memoryJobStore.Remove(job);
            }
        }

        return Task.CompletedTask;
    }

    public virtual Task StoreLogAsync(JobEventData eventData)
    {
        eventData.CancellationToken.ThrowIfCancellationRequested();

        return Task.CompletedTask;
    }

    public virtual Task<List<JobInfo>> CleanupAsync(
        int maxResultCount, 
        TimeSpan jobExpiratime,
        string nodeName = null, 
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var expriaJobs = new List<JobInfo>();

        lock (_jobSync)
        {
            var expiratime = DateTime.Now - jobExpiratime;

            expriaJobs = _memoryJobStore
                    .WhereIf(!nodeName.IsNullOrWhiteSpace(), x => x.NodeName == nodeName)
                    .Where(x => x.Status == JobStatus.Completed &&
                        expiratime.CompareTo(x.LastRunTime ?? x.EndTime ?? x.CreationTime) <= 0)
                    .Take(maxResultCount)
                    .ToList();

            _memoryJobStore.RemoveAll(expriaJobs);
        }

        return Task.FromResult(expriaJobs);
    }
}
