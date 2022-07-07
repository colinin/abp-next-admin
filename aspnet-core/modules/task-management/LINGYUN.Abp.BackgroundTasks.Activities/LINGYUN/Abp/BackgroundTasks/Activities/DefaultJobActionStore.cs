using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Activities;

[Dependency(TryRegister = true)]
public class DefaultJobActionStore : IJobActionStore, ISingletonDependency
{
    public Task<List<JobAction>> GetActionsAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<JobAction>());
    }
}
