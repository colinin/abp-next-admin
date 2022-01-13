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
        // 仅轮询宿主端
        await QueuePollingJob();
        await QueueCleaningJob();

        // 周期性任务改为手动入队
        // await QueueKeepAliveJob();
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
            Id = "KeepAlive",
            Name = nameof(BackgroundKeepAliveJob),
            Group = "KeepAlive",
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
