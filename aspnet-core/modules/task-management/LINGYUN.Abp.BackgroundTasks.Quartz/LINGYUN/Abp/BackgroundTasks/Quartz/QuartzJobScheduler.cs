using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

[Dependency(ReplaceServices = true)]
public class QuartzJobScheduler : IJobScheduler, ISingletonDependency
{
    protected IJobStore JobStore { get; }
    protected IScheduler Scheduler { get; }
    protected IQuartzJobExecutorProvider QuartzJobExecutor { get; }

    public QuartzJobScheduler(
        IJobStore jobStore,
        IScheduler scheduler,
        IQuartzJobExecutorProvider quartzJobExecutor)
    {
        JobStore = jobStore;
        Scheduler = scheduler;
        QuartzJobExecutor = quartzJobExecutor;
    }

    public virtual async Task<bool> ExistsAsync(JobInfo job)
    {
        var jobKey = new JobKey(job.Name, job.Group);
        return await Scheduler.CheckExists(jobKey);
    }

    public virtual async Task PauseAsync(JobInfo job)
    {
        var jobKey = new JobKey(job.Name, job.Group);
        if (await Scheduler.CheckExists(jobKey))
        {
            var triggers = await Scheduler.GetTriggersOfJob(jobKey);
            foreach (var trigger in triggers)
            {
                await Scheduler.PauseTrigger(trigger.Key);
            }
        }
    }

    public virtual async Task<bool> QueueAsync(JobInfo job)
    {
        var jobKey = new JobKey(job.Name, job.Group);
        if (await Scheduler.CheckExists(jobKey))
        {
            return false;
        }

        var jobDetail = QuartzJobExecutor.CreateJob(job);
        if (jobDetail == null)
        {
            return false;
        }

        var jobTrigger = QuartzJobExecutor.CreateTrigger(job);
        if (jobTrigger == null)
        {
            return false;
        }

        await Scheduler.ScheduleJob(jobDetail, jobTrigger);

        return await Scheduler.CheckExists(jobTrigger.Key);
    }

    public virtual async Task QueuesAsync(IEnumerable<JobInfo> jobs)
    {
        var jobDictionary = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
        foreach (var job in jobs)
        {
            var jobDetail = QuartzJobExecutor.CreateJob(job);
            if (jobDetail == null)
            {
                continue;
            }

            var jobTrigger = QuartzJobExecutor.CreateTrigger(job);
            if (jobTrigger == null)
            {
                continue;
            }

            jobDictionary[jobDetail] = new ITrigger[] { jobTrigger };
        }

        await Scheduler.ScheduleJobs(jobDictionary, false);
    }

    public virtual async Task<bool> RemoveAsync(JobInfo job)
    {
        var jobKey = new JobKey(job.Name, job.Group);
        if (!await Scheduler.CheckExists(jobKey))
        {
            return false;
        }

        var triggers = await Scheduler.GetTriggersOfJob(jobKey);
        foreach (var trigger in triggers)
        {
            await Scheduler.PauseTrigger(trigger.Key);
        }
        await Scheduler.DeleteJob(jobKey);

        return !await Scheduler.CheckExists(jobKey);
    }

    public virtual async Task ResumeAsync(JobInfo job)
    {
        var jobKey = new JobKey(job.Name, job.Group);
        if (await Scheduler.CheckExists(jobKey))
        {
            var triggers = await Scheduler.GetTriggersOfJob(jobKey);
            foreach (var trigger in triggers)
            {
                await Scheduler.ResumeTrigger(trigger.Key);
            }
        }
    }

    public virtual async Task<bool> ShutdownAsync()
    {
        await StopAsync();

        await Scheduler.Shutdown(true);

        return Scheduler.IsShutdown;
    }

    public virtual async Task<bool> StartAsync()
    {
        if (Scheduler.InStandbyMode)
        {
            await Scheduler.Start();
        }
        return Scheduler.InStandbyMode;
    }

    public virtual async Task<bool> StopAsync()
    {
        if (!Scheduler.InStandbyMode)
        {
            //等待任务运行完成
            await Scheduler.Standby();
        }
        return !Scheduler.InStandbyMode;
    }

    public virtual async Task TriggerAsync(JobInfo job)
    {
        var jobKey = new JobKey(job.Name, job.Group);
        if (!await Scheduler.CheckExists(jobKey))
        {
            await QueueAsync(job);
        }
        else
        {
            await Scheduler.TriggerJob(jobKey);
        }
    }
}
