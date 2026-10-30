using Elsa.Studio.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Notifications.Blazor.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.Studio.Notifications.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationsUIHintHandlers(this IServiceCollection services)
    {
        return services.AddUIHintHandler<NotificationPickerHandler>();
    }
}
