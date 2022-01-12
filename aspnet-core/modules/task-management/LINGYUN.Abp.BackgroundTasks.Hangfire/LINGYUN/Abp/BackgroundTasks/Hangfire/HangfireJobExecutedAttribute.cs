using Hangfire.Common;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Hangfire;

public class HangfireJobExecutedAttribute : JobFilterAttribute, IServerFilter
{
    public ILogger<HangfireJobExecutedAttribute> Logger { protected get; set; }
    public IServiceProvider ServiceProvider { get; set; }

    public HangfireJobExecutedAttribute()
    {
        Logger = NullLogger<HangfireJobExecutedAttribute>.Instance;
    }

    public HangfireJobExecutedAttribute(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

        Logger = NullLogger<HangfireJobExecutedAttribute>.Instance;
    }

    public async void OnPerformed(PerformedContext filterContext)
    {
        if (Guid.TryParse(filterContext.BackgroundJob.Id, out var jobUUId))
        {
            try
            {
                var jobEventProvider = ServiceProvider.GetRequiredService<IJobEventProvider>();
                var jobEventList = jobEventProvider.GetAll();
                if (!jobEventList.Any())
                {
                    return;
                }

                using var scope = ServiceProvider.CreateScope();

                var jobGroup = filterContext.Connection
                    .GetJobParameter(filterContext.BackgroundJob.Id, nameof(JobInfo.Group));
                var jobName = filterContext.Connection
                    .GetJobParameter(filterContext.BackgroundJob.Id, nameof(JobInfo.Name));

                var jobEventData = new JobEventData(
                    jobUUId,
                    filterContext.BackgroundJob.Job.Type,
                    jobGroup,
                    jobName)
                {
                    Result = filterContext.Result?.ToString()
                };

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

    public async void OnPerforming(PerformingContext filterContext)
    {
        var lockTime = filterContext.Connection
            .GetJobParameter(filterContext.BackgroundJob.Id, nameof(JobInfo.LockTimeOut));

        if (!lockTime.IsNullOrWhiteSpace() && int.TryParse(lockTime, out var time) && time > 0)
        {
            var jobLockProvider = ServiceProvider.GetRequiredService<IJobLockProvider>();
            if (!await jobLockProvider.TryLockAsync(
                filterContext.BackgroundJob.Id,
                time, 
                filterContext.CancellationToken.ShutdownToken))
            {
                filterContext.Canceled = true;
            }
        }
    }
}
