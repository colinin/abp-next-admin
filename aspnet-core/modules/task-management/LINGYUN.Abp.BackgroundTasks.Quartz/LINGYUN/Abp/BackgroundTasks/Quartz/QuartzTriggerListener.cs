using Quartz;
using Quartz.Listener;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzTriggerListener : TriggerListenerSupport, ISingletonDependency
{
    protected const string LockKeyFormat = "p:abp-background-tasks,job:{0},key:{1}";

    public override string Name => "QuartzTriggerListener";

    protected IJobLockProvider JobLockProvider { get; }

    public QuartzTriggerListener(
        IJobLockProvider jobLockProvider)
    {
        JobLockProvider = jobLockProvider;
    }

    public override async Task<bool> VetoJobExecution(
        ITrigger trigger,
        IJobExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        context.MergedJobDataMap.TryGetValue(nameof(JobInfo.Id), out var jobId);
        context.MergedJobDataMap.TryGetValue(nameof(JobInfo.LockTimeOut), out var lockTime);
        if (jobId != null && lockTime != null && lockTime is int time && time > 0)
        {
            
            return !await JobLockProvider.TryLockAsync(NormalizeKey(context, jobId), time, cancellationToken);
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
            await JobLockProvider.TryReleaseAsync(NormalizeKey(context, jobId), cancellationToken);
        }
    }

    protected virtual string NormalizeKey(IJobExecutionContext context, object jobId)
    {
        return string.Format(LockKeyFormat, context.JobDetail.JobType.Name, jobId.ToString());
    }
}
