using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.BackgroundTasks.EventBus;
public class JobSynchronizer :
    IDistributedEventHandler<JobStartEventData>,
    IDistributedEventHandler<JobStopEventData>,
    IDistributedEventHandler<JobTriggerEventData>,
    IDistributedEventHandler<JobPauseEventData>,
    IDistributedEventHandler<JobResumeEventData>,
    ITransientDependency
{
    protected IJobStore JobStore { get; }
    protected IJobScheduler JobScheduler { get; }
    protected AbpBackgroundTasksOptions BackgroundTasksOptions { get; }

    public JobSynchronizer(
        IJobStore jobStore,
        IJobScheduler jobScheduler,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        JobStore = jobStore;
        JobScheduler = jobScheduler;
        BackgroundTasksOptions = options.Value;
    }

    public async virtual Task HandleEventAsync(JobStartEventData eventData)
    {
        if (string.Equals(eventData.NodeName, BackgroundTasksOptions.NodeName))
        {
            foreach (var jobId in eventData.IdList)
            {
                var jobInfo = await JobStore.FindAsync(jobId);

                if (jobInfo == null)
                {
                    continue;
                }

                await JobScheduler.QueueAsync(jobInfo);
            }
        }
    }

    public async virtual Task HandleEventAsync(JobStopEventData eventData)
    {
        if (string.Equals(eventData.NodeName, BackgroundTasksOptions.NodeName))
        {
            foreach (var jobId in eventData.IdList)
            {
                var jobInfo = await JobStore.FindAsync(jobId);

                if (jobInfo == null)
                {
                    continue;
                }

                await JobScheduler.RemoveAsync(jobInfo);
            }
        }
    }

    public async virtual Task HandleEventAsync(JobTriggerEventData eventData)
    {
        if (string.Equals(eventData.NodeName, BackgroundTasksOptions.NodeName))
        {
            foreach (var jobId in eventData.IdList)
            {
                var jobInfo = await JobStore.FindAsync(jobId);

                if (jobInfo == null)
                {
                    continue;
                }

                await JobScheduler.TriggerAsync(jobInfo);
            }
        }
    }

    public async virtual Task HandleEventAsync(JobPauseEventData eventData)
    {
        if (string.Equals(eventData.NodeName, BackgroundTasksOptions.NodeName))
        {
            foreach (var jobId in eventData.IdList)
            {
                var jobInfo = await JobStore.FindAsync(jobId);

                if (jobInfo == null)
                {
                    continue;
                }

                await JobScheduler.PauseAsync(jobInfo);
            }
        }
    }

    public async virtual Task HandleEventAsync(JobResumeEventData eventData)
    {
        if (string.Equals(eventData.NodeName, BackgroundTasksOptions.NodeName))
        {
            foreach (var jobId in eventData.IdList)
            {
                var jobInfo = await JobStore.FindAsync(jobId);

                if (jobInfo == null)
                {
                    continue;
                }

                await JobScheduler.ResumeAsync(jobInfo);
            }
        }
    }
}
