using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Listener;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzTriggerListener : TriggerListenerSupport, ISingletonDependency
{
    protected const string LockKeyFormat = "p:abp-background-tasks,job:{0},key:{1}";

    public override string Name => "QuartzTriggerListener";

    public ILogger<QuartzTriggerListener> Logger { protected get; set; }

    protected AbpBackgroundTasksOptions Options { get; }
    protected IJobLockProvider JobLockProvider { get; }

    public QuartzTriggerListener(
        IJobLockProvider jobLockProvider,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        JobLockProvider = jobLockProvider;
        Options = options.Value;

        Logger = NullLogger<QuartzTriggerListener>.Instance;
    }

    public override async Task<bool> VetoJobExecution(
        ITrigger trigger,
        IJobExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        if (!Options.NodeName.IsNullOrWhiteSpace())
        {
            context.MergedJobDataMap.TryGetValue(nameof(JobInfo.NodeName), out var jobNode);
            if (!Equals(Options.NodeName, jobNode))
            {
                Logger.LogDebug("the job does not belong to the current node and will be ignored by the scheduler.");
                return true;
            }
        }
        context.MergedJobDataMap.TryGetValue(nameof(JobInfo.Id), out var jobId);
        context.MergedJobDataMap.TryGetValue(nameof(JobInfo.LockTimeOut), out var lockTime);
        if (jobId != null && lockTime != null && int.TryParse(lockTime.ToString(), out var time) && time > 0)
        {
            // 传递令牌将清除本次锁, 那并无意义
            if (!await JobLockProvider.TryLockAsync(NormalizeKey(context, jobId), time))
            {
                Logger.LogDebug("The exclusive job is already in use by another scheduler. Ignore this schedule.");
                return true;
            }
        }
        
        return false;
    }

    public override async Task TriggerComplete(
        ITrigger trigger, 
        IJobExecutionContext context, 
        SchedulerInstruction triggerInstructionCode,
        CancellationToken cancellationToken = default)
    {
        if (context.MergedJobDataMap.TryGetValue(nameof(JobInfo.Id), out var jobId) &&
            context.MergedJobDataMap.ContainsKey(nameof(JobInfo.LockTimeOut)))
        {
            await JobLockProvider.TryReleaseAsync(NormalizeKey(context, jobId));
        }
    }

    protected virtual string NormalizeKey(IJobExecutionContext context, object jobId)
    {
        return string.Format(LockKeyFormat, context.JobDetail.JobType.Name, jobId.ToString());
    }
}
