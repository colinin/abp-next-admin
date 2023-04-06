using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(TryRegister = true)]
public class NullJobDispatcher : IJobDispatcher, ISingletonDependency
{
    public static readonly IJobDispatcher Instance = new NullJobDispatcher();
    public Task<bool> DispatchAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task<bool> DispatchAsync(
        IEnumerable<JobInfo> jobs, 
        string nodeName = null,
        Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}
