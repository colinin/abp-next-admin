using Quartz;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IJobScheduler),
    typeof(IJobPublisher),
    typeof(QuartzJobScheduler))]
public class QuartzJobScheduler : IJobScheduler, IJobPublisher, ISingletonDependency
{
    protected IJobStore JobStore { get; }
    protected IScheduler Scheduler { get; }
    protected IQuartzKeyBuilder KeyBuilder { get; }
    protected IQuartzJobCreator QuartzJobCreator { get; }

    public QuartzJobScheduler(
        IJobStore jobStore,
        IScheduler scheduler,
        IQuartzKeyBuilder keyBuilder,
        IQuartzJobCreator quartzJobCreator)
    {
        JobStore = jobStore;
        Scheduler = scheduler;
        KeyBuilder = keyBuilder;
        QuartzJobCreator = quartzJobCreator;
    }

    public async virtual Task<bool> ExistsAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return await Scheduler.CheckExists(BuildJobKey(job), cancellationToken);
    }

    public async virtual Task PauseAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        var jobKey = BuildJobKey(job);
        if (await Scheduler.CheckExists(jobKey, cancellationToken))
        {
            var triggers = await Scheduler.GetTriggersOfJob(jobKey, cancellationToken);
            foreach (var trigger in triggers)
            {
                await Scheduler.PauseTrigger(trigger.Key, cancellationToken);
            }

            // 当暂停作业时, 需要停止已经在执行中的作业
            await Scheduler.Interrupt(jobKey, cancellationToken);
        }
    }

    public async virtual Task<bool> PublishAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return await QueueAsync(job, cancellationToken);
    }

    public async virtual Task<bool> QueueAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        var jobKey = BuildJobKey(job);
        if (await Scheduler.CheckExists(jobKey, cancellationToken))
        {
            return false;
        }

        var jobDetail = QuartzJobCreator.CreateJob(job);
        if (jobDetail == null)
        {
            return false;
        }

        var jobTrigger = QuartzJobCreator.CreateTrigger(job);
        if (jobTrigger == null)
        {
            return false;
        }

        await Scheduler.ScheduleJob(jobDetail, jobTrigger, cancellationToken);

        return await Scheduler.CheckExists(jobTrigger.Key, cancellationToken);
    }

    public async virtual Task QueuesAsync(IEnumerable<JobInfo> jobs, CancellationToken cancellationToken = default)
    {
        var jobDictionary = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
        foreach (var job in jobs)
        {
            var jobDetail = QuartzJobCreator.CreateJob(job);
            if (jobDetail == null)
            {
                continue;
            }

            var jobTrigger = QuartzJobCreator.CreateTrigger(job);
            if (jobTrigger == null)
            {
                continue;
            }

            jobDictionary[jobDetail] = new ITrigger[] { jobTrigger };
        }

        await Scheduler.ScheduleJobs(jobDictionary, true, cancellationToken);
    }

    public async virtual Task<bool> RemoveAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        var jobKey = BuildJobKey(job);
        if (!await Scheduler.CheckExists(jobKey, cancellationToken))
        {
            return false;
        }

        var triggers = await Scheduler.GetTriggersOfJob(jobKey, cancellationToken);
        foreach (var trigger in triggers)
        {
            await Scheduler.PauseTrigger(trigger.Key, cancellationToken);
        }

        // 当删除作业时, 需要停止已经在执行中的作业
        await Scheduler.Interrupt(jobKey, cancellationToken);

        await Scheduler.DeleteJob(jobKey, cancellationToken);

        return !await Scheduler.CheckExists(jobKey, cancellationToken);
    }

    public async virtual Task ResumeAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        var jobKey = BuildJobKey(job);
        if (await Scheduler.CheckExists(jobKey, cancellationToken))
        {
            var triggers = await Scheduler.GetTriggersOfJob(jobKey, cancellationToken);
            foreach (var trigger in triggers)
            {
                await Scheduler.ResumeTrigger(trigger.Key, cancellationToken);
            }
        }
    }

    public async virtual Task<bool> ShutdownAsync(CancellationToken cancellationToken = default)
    {
        await StopAsync(cancellationToken);

        await Scheduler.Shutdown(true, cancellationToken);

        return Scheduler.IsShutdown;
    }

    public async virtual Task<bool> StartAsync(CancellationToken cancellationToken = default)
    {
        if (Scheduler.InStandbyMode)
        {
            await Scheduler.Start(cancellationToken);
        }
        return Scheduler.InStandbyMode;
    }

    public async virtual Task<bool> StopAsync(CancellationToken cancellationToken = default)
    {
        if (!Scheduler.InStandbyMode)
        {
            //等待任务运行完成
            await Scheduler.Standby(cancellationToken);
        }
        return !Scheduler.InStandbyMode;
    }

    public async virtual Task TriggerAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        var jobKey = BuildJobKey(job);
        if (!await Scheduler.CheckExists(jobKey, cancellationToken))
        {
            await QueueAsync(job, cancellationToken);
        }
        else
        {
            await Scheduler.TriggerJob(jobKey, cancellationToken);
        }
    }

    private JobKey BuildJobKey(JobInfo job)
    {
        return KeyBuilder.CreateJobKey(job);
    }
}
