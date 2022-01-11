using JetBrains.Annotations;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(TryRegister = true)]
public class NullJobExceptionNotifier : IJobExceptionNotifier, ISingletonDependency
{
    public Task NotifyAsync([NotNull] JobExceptionNotificationContext context)
    {
        return Task.CompletedTask;
    }
}
