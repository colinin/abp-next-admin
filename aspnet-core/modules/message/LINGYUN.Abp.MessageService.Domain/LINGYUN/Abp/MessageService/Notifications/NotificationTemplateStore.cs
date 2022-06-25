using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MessageService.Notifications;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
[ExposeServices(typeof(INotificationTemplateStore))]
public class NotificationTemplateStore : INotificationTemplateStore
{
    protected INotificationTemplateRepository NotificationTemplateRepository { get; }

    public NotificationTemplateStore(
        INotificationTemplateRepository notificationTemplateRepository)
    {
        NotificationTemplateRepository = notificationTemplateRepository;
    }

    public async virtual Task<string> GetContentOrNullAsync(string templateName, string culture = null, CancellationToken cancellationToken = default)
    {
        var template = await NotificationTemplateRepository.GetByNameAsync(
            templateName, 
            culture ?? CultureInfo.CurrentCulture.Name,
            cancellationToken);

        return template?.Content;
    }
}
