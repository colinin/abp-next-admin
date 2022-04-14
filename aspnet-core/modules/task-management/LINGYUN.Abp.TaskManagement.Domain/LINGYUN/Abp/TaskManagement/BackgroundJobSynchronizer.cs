using LINGYUN.Abp.BackgroundTasks;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.TaskManagement;
public class BackgroundJobSynchronizer :
    IDistributedEventHandler<EntityCreatedEto<BackgroundJobEto>>,
    IDistributedEventHandler<EntityDeletedEto<BackgroundJobEto>>,
    ITransientDependency
{
    protected IJobStore JobStore { get; }
    protected IJobScheduler JobScheduler { get; }
    protected AbpBackgroundTasksOptions BackgroundTasksOptions { get; }

    public BackgroundJobSynchronizer(
        IJobStore jobStore,
        IJobScheduler jobScheduler,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        JobStore = jobStore;
        JobScheduler = jobScheduler;
        BackgroundTasksOptions = options.Value;
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<BackgroundJobEto> eventData)
    {
        if (string.Equals(eventData.Entity.NodeName, BackgroundTasksOptions.NodeName))
        {
            var jobInfo = await JobStore.FindAsync(eventData.Entity.Id);

            if (jobInfo == null)
            {
                return;
            }

            await JobScheduler.RemoveAsync(jobInfo);
        }
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<BackgroundJobEto> eventData)
    {
        if (string.Equals(eventData.Entity.NodeName, BackgroundTasksOptions.NodeName))
        {
            var jobInfo = await JobStore.FindAsync(eventData.Entity.Id);

            if (jobInfo == null)
            {
                return;
            }

            if (eventData.Entity.IsEnabled && jobInfo.JobType == JobType.Period)
            {
                await JobScheduler.QueueAsync(jobInfo);
            }
        }
    }
}
