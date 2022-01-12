using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzJobExecutorProvider : IQuartzJobExecutorProvider, ISingletonDependency
{
    public ILogger<QuartzJobExecutorProvider> Logger { protected get; set; }

    protected IClock Clock { get; }
    protected AbpBackgroundTasksOptions Options { get; }

    public QuartzJobExecutorProvider(
        IClock clock,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        Clock = clock;
        Options = options.Value;

        Logger = NullLogger<QuartzJobExecutorProvider>.Instance;
    }

    public IJobDetail CreateJob(JobInfo job)
    {
        var jobType = Type.GetType(job.Type) ?? Options.JobProviders.GetOrDefault(job.Type);
        if (jobType == null)
        {
            Logger.LogWarning($"The task: {job.Group} - {job.Name}: {job.Type} is not registered and cannot create an instance of the performer type.");
            return null;
        }

        var adapterType = typeof(QuartzJobSimpleAdapter<>);

        // 注释, 通过触发器监听锁定
        //if (job.LockTimeOut > 0)
        //{
        //    adapterType = typeof(QuartzJobConcurrentAdapter<>);
        //}

        if (!typeof(IJob).IsAssignableFrom(jobType))
        {
            jobType = adapterType.MakeGenericType(jobType);
        }

        var jobBuilder = JobBuilder.Create(jobType)
                .WithIdentity(job.Name, job.Group)
                .WithDescription(job.Description);

        jobBuilder.UsingJobData(nameof(JobInfo.Id), job.Id);
        jobBuilder.UsingJobData(nameof(JobInfo.LockTimeOut), job.LockTimeOut);
        jobBuilder.UsingJobData(new JobDataMap(job.Args));

        return jobBuilder.Build();
    }

    public ITrigger CreateTrigger(JobInfo job)
    {
        var triggerBuilder = TriggerBuilder.Create();

        switch (job.JobType)
        {
            case JobType.Period:
                if (!CronExpression.IsValidExpression(job.Cron))
                {
                    Logger.LogWarning($"The task: {job.Group} - {job.Name} periodic task Cron expression was invalid and the task trigger could not be created.");
                    return null;
                }
                triggerBuilder
                    .WithIdentity(job.Name, job.Group)
                    .WithDescription(job.Description)
                    .EndAt(job.EndTime)
                    .ForJob(job.Name, job.Group)
                    .WithPriority((int)job.Priority)
                    .WithCronSchedule(job.Cron);
                if (job.BeginTime > Clock.Now)
                {
                    triggerBuilder = triggerBuilder.StartAt(job.BeginTime);
                }
                break;
            case JobType.Once:
            case JobType.Persistent:
            default:
                // Quartz 需要减一位
                var maxCount = job.MaxCount <= 0 ? -1 : job.MaxCount - 1;
                if (job.JobType == JobType.Once)
                {
                    maxCount = 0;
                }
                if (job.Status == JobStatus.FailedRetry && job.TryCount < job.MaxTryCount)
                {
                    maxCount = job.MaxTryCount <= 0 ? -1 : job.MaxTryCount - 1;
                }
                triggerBuilder
                    .WithIdentity(job.Name, job.Group)
                    .WithDescription(job.Description)
                    .StartAt(Clock.Now.AddSeconds(job.Interval))
                    .EndAt(job.EndTime)
                    .ForJob(job.Name, job.Group)
                    .WithPriority((int)job.Priority)
                    .WithSimpleSchedule(x =>
                        x.WithIntervalInSeconds(job.Interval)
                         .WithRepeatCount(maxCount));
                break;
        }

        return triggerBuilder.Build();
    }
}
