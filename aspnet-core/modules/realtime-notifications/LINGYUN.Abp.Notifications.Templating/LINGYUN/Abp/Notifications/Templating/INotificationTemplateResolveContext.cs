using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications.Templating;
public interface INotificationTemplateResolveContext : IServiceProviderAccessor
{
    NotificationTemplate Template { get; }

    object Model { get; set; }

    bool Handled { get; set; }
}
