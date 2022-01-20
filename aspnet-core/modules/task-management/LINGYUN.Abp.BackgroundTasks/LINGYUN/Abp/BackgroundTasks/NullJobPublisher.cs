using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(TryRegister = true)]
public class NullJobPublisher : IJobPublisher, ISingletonDependency
{
    public Task<bool> PublishAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}
