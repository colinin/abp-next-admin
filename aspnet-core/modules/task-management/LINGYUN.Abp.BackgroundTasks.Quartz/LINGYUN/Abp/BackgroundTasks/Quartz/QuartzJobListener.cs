using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Quartz;
using Quartz.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzJobListener : JobListenerSupport, ISingletonDependency
{
    public ILogger<QuartzJobListener> Logger { protected get; set; }

    public override string Name => "QuartzJobListener";

    protected IJobEventProvider EventProvider { get; }
    protected IServiceProvider ServiceProvider { get; }

    public QuartzJobListener(
        IServiceProvider serviceProvider,
        IJobEventProvider eventProvider)
    {
        ServiceProvider = serviceProvider;
        EventProvider = eventProvider;

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
        var jobId = context.GetString(nameof(JobInfo.Id));
        if (Guid.TryParse(jobId, out var jobUUId))
        {
            try
            {
                using var scope = ServiceProvider.CreateScope();
                var jobEventData = new JobEventData(
                    jobUUId,
                    context.JobDetail.JobType,
                    context.JobDetail.Key.Group,
                    context.JobDetail.Key.Name)
                {
                    Result = context.Result?.ToString()
                };

                var jobEventList = EventProvider.GetAll();
                var eventContext = new JobEventContext(
                    scope.ServiceProvider,
                    jobEventData);

                var index = 0;
                var taskList = new Task[jobEventList.Count];
                foreach (var jobEvent in jobEventList)
                {
                    taskList[index] = jobEvent.OnJobBeforeExecuted(eventContext);
                    index++;
                }

                await Task.WhenAll(taskList);
            }
            catch (Exception ex)
            {
                Logger.LogError($"The event before the task execution is abnormal：{ex}");
            }
        }
    }

    public override async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var jobId = context.GetString(nameof(JobInfo.Id));
            if (Guid.TryParse(jobId, out var jobUUId))
            {
                var jobType = context.JobDetail.JobType;
                if (jobType.IsGenericType)
                {
                    jobType = jobType.GetGenericArguments()[0];
                }

                var jobEventData = new JobEventData(
                    jobUUId,
                    jobType,
                    context.JobDetail.Key.Group,
                    context.JobDetail.Key.Name,
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
                jobEventData.RunTime = context.FireTimeUtc.LocalDateTime;
                jobEventData.LastRunTime = context.PreviousFireTimeUtc?.LocalDateTime;
                jobEventData.NextRunTime = context.NextFireTimeUtc?.LocalDateTime;
                if (context.Result != null)
                {
                    jobEventData.Result = context.Result?.ToString();
                }
                var tenantIdString = context.GetString(nameof(IMultiTenant.TenantId));
                if (Guid.TryParse(tenantIdString, out var tenantId))
                {
                    jobEventData.TenantId = tenantId;
                }

                var jobEventList = EventProvider.GetAll();
                var eventContext = new JobEventContext(
                    scope.ServiceProvider,
                    jobEventData);

                var index = 0;
                var taskList = new Task[jobEventList.Count];
                foreach (var jobEvent in jobEventList)
                {
                    taskList[index] = jobEvent.OnJobAfterExecuted(eventContext);
                    index++;
                }

                await Task.WhenAll(taskList);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"The event is abnormal after the task is executed：{ex}");
        }
    }
}
