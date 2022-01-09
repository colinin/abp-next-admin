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
    private readonly IJobScheduler _jobScheduler;
    private readonly AbpBackgroundTasksOptions _options;

    public DefaultBackgroundWorker(
        IJobStore jobStore,
        IJobScheduler jobScheduler,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        _jobStore = jobStore;
        _jobScheduler = jobScheduler;
        _options = options.Value;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await QueuePollingJob();
        await QueueKeepAliveJob();
        await QueueCleaningJob();
    }

    private async Task QueueKeepAliveJob()
    {
        var keepAliveJob = BuildKeepAliveJobInfo();
        await _jobScheduler.QueueAsync(keepAliveJob);
    }

    private async Task QueuePollingJob()
    {
        var pollingJob = BuildPollingJobInfo();
        await _jobScheduler.QueueAsync(pollingJob);
    }

    private async Task QueueCleaningJob()
    {
        var cleaningJob = BuildCleaningJobInfo();
        await _jobScheduler.QueueAsync(cleaningJob);
    }

    private JobInfo BuildKeepAliveJobInfo()
    {
        return new JobInfo
        {
            Id = Guid.Parse("8F50C5D9-5691-4B99-A52B-CABD91D93C89"),
            Name = nameof(BackgroundKeepAliveJob),
            Group = "Default",
            Description = "Add periodic tasks",
            Args = new Dictionary<string, object>(),
            Status = JobStatus.Running,
            BeginTime = DateTime.Now,
            CreationTime = DateTime.Now,
            JobType = JobType.Once,
            Priority = JobPriority.High,
            MaxCount = 1,
            Interval = 30,
            Type = typeof(BackgroundKeepAliveJob).AssemblyQualifiedName,
        };
    }

    private JobInfo BuildPollingJobInfo()
    {
        return new JobInfo
        {
            Id = Guid.Parse("C51152E9-F0B8-4252-8352-283BE46083CC"),
            Name = nameof(BackgroundPollingJob),
            Group = "Default",
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
            Id = Guid.Parse("AAAF8783-FA06-4CF9-BDCA-11140FB2478F"),
            Name = nameof(BackgroundCleaningJob),
            Group = "Default",
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
