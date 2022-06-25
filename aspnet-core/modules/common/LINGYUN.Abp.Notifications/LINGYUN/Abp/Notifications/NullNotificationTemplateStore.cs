using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

[Dependency(TryRegister = true)]
public class NullNotificationTemplateStore : INotificationTemplateStore, ISingletonDependency
{
    public readonly static INotificationTemplateStore Instance = new NullNotificationTemplateStore();

    public Task<string> GetOrNullAsync(string templateName, string culture = null, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<string>(null);
    }
}
