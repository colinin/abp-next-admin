using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

internal class DefaultBackgroundWorker : BackgroundService
{
    private readonly IJobStore _jobStore;
    private readonly IJobPublisher _jobPublisher;
    private readonly AbpBackgroundTasksOptions _options;

    public DefaultBackgroundWorker(
        IJobStore jobStore,
        IJobPublisher jobPublisher,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        _jobStore = jobStore;
        _jobPublisher = jobPublisher;
        _options = options.Value;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await QueuePollingJob();
        await QueueCleaningJob();
    }

    private async Task QueuePollingJob()
    {
        if (_options.JobFetchEnabled)
        {
            var pollingJob = BuildPollingJobInfo();
            await _jobPublisher.PublishAsync(pollingJob);
        }
    }

    private async Task QueueCleaningJob()
    {
        if (_options.JobCleanEnabled)
        {
            var cleaningJob = BuildCleaningJobInfo();
            await _jobPublisher.PublishAsync(cleaningJob);
        }
    }

    private JobInfo BuildPollingJobInfo()
    {
        return new JobInfo
        {
            Id = "Polling",
            Name = nameof(BackgroundPollingJob),
            Group = "Polling",
            Description = "Polling tasks to be executed",
            Args = new Dictionary<string, object>(),
            Status = JobStatus.Running,
            BeginTime = DateTime.Now,
            CreationTime = DateTime.Now,
            Cron = _options.JobFetchCronExpression,
            JobType = JobType.Period,
            Priority = JobPriority.High,
            LockTimeOut = _options.JobFetchLockTimeOut,
            Type = typeof(BackgroundPollingJob).AssemblyQualifiedName,
        };
    }

    private JobInfo BuildCleaningJobInfo()
    {
        return new JobInfo
        {
            Id = "Cleaning",
            Name = nameof(BackgroundCleaningJob),
            Group = "Cleaning",
            Description = "Cleaning tasks to be executed",
            Args = new Dictionary<string, object>(),
            Status = JobStatus.Running,
            BeginTime = DateTime.Now,
            CreationTime = DateTime.Now,
            Cron = _options.JobCleanCronExpression,
            JobType = JobType.Period,
            Priority = JobPriority.High,
            Type = typeof(BackgroundCleaningJob).AssemblyQualifiedName,
        };
    }
}
