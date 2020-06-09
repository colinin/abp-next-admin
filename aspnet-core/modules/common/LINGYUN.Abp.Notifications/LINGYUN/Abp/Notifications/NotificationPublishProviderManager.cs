using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationPublishProviderManager : INotificationPublishProviderManager, ISingletonDependency
    {
        public List<INotificationPublishProvider> Providers => _lazyProviders.Value;

        protected AbpNotificationOptions Options { get; }

        private readonly Lazy<List<INotificationPublishProvider>> _lazyProviders;

        public NotificationPublishProviderManager(
            IServiceProvider serviceProvider,
            IOptions<AbpNotificationOptions> options)
        {
            Options = options.Value;

            _lazyProviders = new Lazy<List<INotificationPublishProvider>>(
                () => Options
                    .PublishProviders
                    .Select(type => serviceProvider.GetRequiredService(type) as INotificationPublishProvider)
                    .ToList(),
                true
            );
        }
    }
}
