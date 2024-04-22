﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Quartz;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzJobCreator : IQuartzJobCreator, ISingletonDependency
{
    public ILogger<QuartzJobCreator> Logger { protected get; set; }

    protected IClock Clock { get; }
    protected IQuartzKeyBuilder KeyBuilder { get; }
    protected IJobDefinitionManager JobDefinitionManager { get; }
    public QuartzJobCreator(
        IClock clock,
        IQuartzKeyBuilder keyBuilder,
        IJobDefinitionManager jobDefinitionManager)
    {
        Clock = clock;
        KeyBuilder = keyBuilder;
        JobDefinitionManager = jobDefinitionManager;

        Logger = NullLogger<QuartzJobCreator>.Instance;
    }

    public IJobDetail CreateJob(JobInfo job)
    {
        var jobDefinition = JobDefinitionManager.GetOrNull(job.Type);
        var jobType = jobDefinition?.JobType ?? Type.GetType(job.Type);
        if (jobType == null)
        {
            //Logger.LogWarning($"The task: {job.Group} - {job.Name}: {job.Type} is not registered and cannot create an instance of the performer type.");
            //return null;

            // 运行时搜寻本地作业
            jobType = typeof(QuartzJobSearchJobAdapter);
        }
        else
        {
            if (!typeof(IJob).IsAssignableFrom(jobType))
            {
                var adapterType = typeof(QuartzJobSimpleAdapter<>);
                jobType = adapterType.MakeGenericType(jobType);
            }
        }

        if (jobType == null)
        {
            return null;
        }

        // 改为 JobId作为名称
        var jobBuilder = JobBuilder.Create(jobType)
                .WithIdentity(KeyBuilder.CreateJobKey(job))
                .WithDescription(job.Description);
        // 多节点任务需要
        jobBuilder.UsingJobData(nameof(JobInfo.NodeName), job.NodeName);
        // 查询任务需要
        jobBuilder.UsingJobData(nameof(JobInfo.Id), job.Id);
        // 有些场景需要
        jobBuilder.UsingJobData(nameof(JobInfo.Name), job.Name);
        jobBuilder.UsingJobData(nameof(JobInfo.Type), job.Type);
        jobBuilder.UsingJobData(nameof(JobInfo.Group), job.Group);
        // 计算增量需要
        jobBuilder.UsingJobData(nameof(JobInfo.TriggerCount), job.TriggerCount);
        // 独占任务需要
        jobBuilder.UsingJobData(nameof(JobInfo.LockTimeOut), job.LockTimeOut);
        // 传递的作业参数
        jobBuilder.UsingJobData(new JobDataMap(job.Args));
        if (job.TenantId.HasValue)
        {
            // 用于多租户场景
            jobBuilder.UsingJobData(nameof(JobInfo.TenantId), job.TenantId.ToString());
        }

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
                if (job.CalcCanBeTriggered() == 0)
                {
                    Logger.LogWarning($"The task: {job.Group} - {job.Name} reached trigger peak and the task trigger could not be created.");
                    return null;
                }
                triggerBuilder
                    .WithIdentity(KeyBuilder.CreateTriggerKey(job))
                    .WithDescription(job.Description)
                    .EndAt(job.EndTime)
                    .ForJob(KeyBuilder.CreateJobKey(job))
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
                var maxCount = job.CalcCanBeTriggered();
                if (maxCount == 0)
                {
                    Logger.LogWarning($"The task: {job.Group} - {job.Name} reached trigger peak and the task trigger could not be created.");
                    return null;
                }

                // Quartz 需要减一位
                maxCount -= 1;
                if (maxCount < -1)
                {
                    maxCount = -1;
                }

                triggerBuilder
                    .WithIdentity(KeyBuilder.CreateTriggerKey(job))
                    .WithDescription(job.Description)
                    .StartAt(Clock.Now.AddSeconds(job.Interval))
                    .EndAt(job.EndTime)
                    .ForJob(KeyBuilder.CreateJobKey(job))
                    .WithPriority((int)job.Priority);

                // Quartz约定. 重复间隔不能为0
                // fix throw Quartz.SchedulerException: Repeat Interval cannot be zero.
                var scheduleBuilder = SimpleScheduleBuilder.Create();
                // TODO: 不能用Quartz自带的重试机制
                // scheduleBuilder.WithRepeatCount(maxCount);
                if (job.JobType == JobType.Persistent)
                {
                    scheduleBuilder.WithRepeatCount(maxCount);
                }
                if (job.Interval > 0)
                {
                    scheduleBuilder.WithIntervalInSeconds(job.Interval);
                }
                else
                {
                    scheduleBuilder.WithIntervalInSeconds(300);
                }

                triggerBuilder.WithSchedule(scheduleBuilder);

                break;
        }

        return triggerBuilder.Build();
    }
}
