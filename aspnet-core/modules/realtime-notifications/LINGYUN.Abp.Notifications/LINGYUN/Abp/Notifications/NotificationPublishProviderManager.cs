using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

public class NotificationPublishProviderManager : INotificationPublishProviderManager, ISingletonDependency
{
    public List<INotificationPublishProvider> Providers => _lazyProviders.Value;

    private readonly Lazy<List<INotificationPublishProvider>> _lazyProviders;

    public NotificationPublishProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpNotificationsPublishOptions> options)
    {
        _lazyProviders = new Lazy<List<INotificationPublishProvider>>(
             () => options.Value
                 .PublishProviders
                 .Select(type => serviceProvider.GetRequiredService(type) as INotificationPublishProvider)
                 .ToList(),
             true
         );
    }
}
