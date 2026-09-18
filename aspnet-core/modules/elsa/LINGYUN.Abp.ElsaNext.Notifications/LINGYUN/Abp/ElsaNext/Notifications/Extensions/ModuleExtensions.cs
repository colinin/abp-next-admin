using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.Notifications.Features;
using System;

namespace LINGYUN.Abp.ElsaNext.Notifications.Extensions;

public static class ModuleExtensions
{
    public static IModule UseAbpNotifications(this IModule configuration, Action<NotificationsFeature>? configure = null)
    {
        configuration.Configure(configure);
        return configuration;
    }
}
