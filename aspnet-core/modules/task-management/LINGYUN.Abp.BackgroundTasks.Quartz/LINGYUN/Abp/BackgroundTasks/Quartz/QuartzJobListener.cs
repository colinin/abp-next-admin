using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Quartz;
using Quartz.Listener;
using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzJobListener : JobListenerSupport, ISingletonDependency
{
    public ILogger<QuartzJobListener> Logger { protected get; set; }

    public override string Name => "QuartzJobListener";

    protected IClock Clock { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    public QuartzJobListener(
        IClock clock,
        IServiceScopeFactory serviceScopeFactory)
    {
        Clock = clock;
        ServiceScopeFactory = serviceScopeFactory;

        Logger = NullLogger<QuartzJobListener>.Instance;
    }

    public override Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        var jobName = context.MergedJobDataMap.Get(nameof(JobInfo.Name))?.ToString();
        if (jobName.IsNullOrWhiteSpace())
        {
            var jobType = context.JobDetail.JobType;
            jobName = !jobType.IsGenericType ? jobType.Name : jobType.GetGenericArguments()[0].Name;
        }
        
        // 作业被锁定才记录warn事件
        if (context.TryGetCache("JobLocked", out var time) && time != null && int.TryParse(time.ToString(), out var lockTime))
        {
            Logger.LogWarning($"The task {jobName} could not be performed, Because it is being scheduled by another job scheduler");
        }

        return Task.FromResult(-1);
    }

    public override async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var jobId = context.GetString(nameof(JobInfo.Id));
            if (jobId.IsNullOrWhiteSpace())
            {
                return;
            }

            if (context.Trigger is ISimpleTrigger simpleTrigger)
            {
                // 增量数据写入临时数据, 仅对长期作业有效, 一次性作业永远为1
                context.Put(nameof(JobInfo.TriggerCount), simpleTrigger.TimesTriggered);
            }

            using var scope = ServiceScopeFactory.CreateScope();
            var jobEventData = new JobEventData(
                jobId,
                context.JobDetail.JobType,
                context.JobDetail.Key.Group,
                context.JobDetail.Key.Name,
                context.MergedJobDataMap.ToImmutableDictionary(),
                cancellationToken: cancellationToken)
            {
                Result = context.Result?.ToString()
            };

            context.TryGetMultiTenantId(out var tenantId);
            jobEventData.TenantId = tenantId;

            var eventContext = new JobEventContext(
                scope.ServiceProvider,
                jobEventData);

            var trigger = scope.ServiceProvider.GetRequiredService<IJobEventTrigger>();
            await trigger.OnJobBeforeExecuted(eventContext);
        }
        catch (Exception ex)
        {
            Logger.LogError($"The event before the task execution is abnormal：{ex}");
        }
    }

    public override async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
    {
        try
        {
            var jobId = context.GetString(nameof(JobInfo.Id));
            if (jobId.IsNullOrWhiteSpace())
            {
                return;
            }

            using var scope = ServiceScopeFactory.CreateScope();
            var jobType = context.JobDetail.JobType;
            if (jobType.IsGenericType)
            {
                jobType = jobType.GetGenericArguments()[0];
            }

            var jobEventData = new JobEventData(
                jobId,
                jobType,
                context.JobDetail.Key.Group,
                context.JobDetail.Key.Name,
                context.MergedJobDataMap.ToImmutableDictionary(),
                jobException,
                cancellationToken)
            {
                Status = JobStatus.Running
            };

            if (context.Trigger is ISimpleTrigger simpleTrigger)
            {
                jobEventData.Triggered = simpleTrigger.TimesTriggered;
                jobEventData.RepeatCount = simpleTrigger.RepeatCount;
            }
            jobEventData.Description = context.JobDetail.Description;
            jobEventData.RunTime = Clock.Normalize(context.FireTimeUtc.LocalDateTime);
            var lastRunTime = context.PreviousFireTimeUtc?.LocalDateTime ?? context.Trigger.GetPreviousFireTimeUtc()?.LocalDateTime;
            if (lastRunTime.HasValue)
            {
                jobEventData.LastRunTime = Clock.Normalize(lastRunTime.Value);
            }
            var nextRunTime = context.NextFireTimeUtc?.LocalDateTime ?? context.Trigger.GetNextFireTimeUtc()?.LocalDateTime;
            if (nextRunTime.HasValue)
            {
                jobEventData.NextRunTime = Clock.Normalize(nextRunTime.Value);
            }
            if (context.Result != null)
            {
                jobEventData.Result = context.Result?.ToString();
            }

            context.TryGetMultiTenantId(out var tenantId);
            jobEventData.TenantId = tenantId;

            var eventContext = new JobEventContext(
                scope.ServiceProvider,
                jobEventData);

            var trigger = scope.ServiceProvider.GetRequiredService<IJobEventTrigger>();
            await trigger.OnJobAfterExecuted(eventContext);
        }
        catch (Exception ex)
        {
            Logger.LogError($"The event is abnormal after the task is executed：{ex}");
        }
    }
}
