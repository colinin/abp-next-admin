using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.BackgroundTasks.EventBus;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IJobDispatcher),
    typeof(DistributedJobDispatcher))]
public class DistributedJobDispatcher : IJobDispatcher, ITransientDependency
{
    protected IDistributedEventBus EventBus { get; }

    public DistributedJobDispatcher(IDistributedEventBus eventBus)
    {
        EventBus = eventBus;
    }

    public async virtual Task<bool> DispatchAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        var eventData = new JobStartEventData
        {
            TenantId = job.TenantId,
            NodeName = job.NodeName,
            IdList = new List<string> { job.Id },
        };

        await EventBus.PublishAsync(eventData);

        return true;
    }

    public async virtual Task<bool> DispatchAsync(
        IEnumerable<JobInfo> jobs,
        string nodeName = null,
        Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        var eventData = new JobStartEventData
        {
            TenantId = tenantId,
            NodeName = nodeName,
            IdList = jobs.Select(job => job.Id).ToList(),
        };

        await EventBus.PublishAsync(eventData);

        return true;
    }
}
