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
        var jobType = context.JobDetail.JobType;
        if (jobType.IsGenericType)
        {
            jobType = jobType.GetGenericTypeDefinition();
        }
        Logger.LogInformation($"The task {jobType.Name} could not be performed...");

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

            using var scope = ServiceScopeFactory.CreateScope();
            var jobEventData = new JobEventData(
                jobId,
                context.JobDetail.JobType,
                context.JobDetail.Key.Group,
                context.JobDetail.Key.Name,
                context.MergedJobDataMap.ToImmutableDictionary())
            {
                Result = context.Result?.ToString()
            };

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
                jobException)
            {
                Status = JobStatus.Running
            };

            if (context.Trigger is ISimpleTrigger simpleTrigger)
            {
                jobEventData.Triggered = simpleTrigger.TimesTriggered;
                jobEventData.RepeatCount = simpleTrigger.RepeatCount;
            }
            jobEventData.Description = context.JobDetail.Description;
            jobEventData.RunTime = Clock.Now;
            jobEventData.LastRunTime = context.PreviousFireTimeUtc?.LocalDateTime
                 ?? context.Trigger.GetPreviousFireTimeUtc()?.LocalDateTime;
            jobEventData.NextRunTime = context.NextFireTimeUtc?.LocalDateTime
                ?? context.Trigger.GetNextFireTimeUtc()?.LocalDateTime;
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
