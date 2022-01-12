using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Hangfire;

[Dependency(ReplaceServices = true)]
public class HangfireJobScheduler : IJobScheduler, ISingletonDependency
{
    public ILogger<HangfireJobScheduler> Logger { protected get; set; }
    protected AbpBackgroundTasksOptions Options { get; }

    protected JobStorage JobStorage { get; }
    protected IRecurringJobManager RecurringJobManager { get; }

    public HangfireJobScheduler(
        JobStorage jobStorage,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        Options = options.Value;

        JobStorage = jobStorage;
        RecurringJobManager = new RecurringJobManager(jobStorage);

        Logger = NullLogger<HangfireJobScheduler>.Instance;
    }

    public Task<bool> ExistsAsync(JobInfo job)
    {
        var monitor = JobStorage.GetMonitoringApi();

        monitor.JobDetails(job.);
    }

    public Task PauseAsync(JobInfo job)
    {
        throw new NotImplementedException();
    }

    public virtual Task<bool> QueueAsync(JobInfo job)
    {
        var jobType = Options.JobProviders.GetOrDefault(job.Type) ?? Type.GetType(job.Type, false);
        if (jobType == null)
        {
            Logger.LogWarning($"The task: {job.Group} - {job.Name}: {job.Type} is not registered and cannot create an instance of the performer type.");
            return Task.FromResult(false);
        }
        var jobData = job.Args;
        jobData[nameof(JobInfo.Id)] = job.Id;
        jobData[nameof(JobInfo.Group)] = job.Group;
        jobData[nameof(JobInfo.Name)] = job.Name;

        switch (job.JobType)
        {
            case JobType.Once:
                var jobId = BackgroundJob.Schedule<HangfireJobSimpleAdapter>(
                    adapter => adapter.ExecuteAsync(jobType, jobData.ToImmutableDictionary()),
                    TimeSpan.FromSeconds(job.Interval));
                job.Args["hangfire"] = jobId;
                break;
            case JobType.Persistent:
                var minuteInterval = job.Interval / 60;
                if (minuteInterval < 1)
                {
                    minuteInterval = 1;
                }
                RecurringJob.AddOrUpdate<HangfireJobSimpleAdapter>(
                    adapter => adapter.ExecuteAsync(jobType, jobData.ToImmutableDictionary()),
                    Cron.MinuteInterval(minuteInterval));
                break;
            case JobType.Period:
                RecurringJob.AddOrUpdate<HangfireJobSimpleAdapter>(
                    adapter => adapter.ExecuteAsync(jobType, jobData.ToImmutableDictionary()),
                    job.Cron,
                    queue: job.Group);
                break;
        }

        return Task.FromResult(true);
    }

    public Task QueuesAsync(IEnumerable<JobInfo> jobs)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(JobInfo job)
    {
        throw new NotImplementedException();
    }

    public Task ResumeAsync(JobInfo job)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ShutdownAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> StartAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> StopAsync()
    {
        throw new NotImplementedException();
    }

    public Task TriggerAsync(JobInfo job)
    {
        throw new NotImplementedException();
    }
}
