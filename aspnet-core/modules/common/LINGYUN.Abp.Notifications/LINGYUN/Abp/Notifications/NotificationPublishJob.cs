using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationPublishJob : AsyncBackgroundJob<NotificationPublishJobArgs>, ITransientDependency
    {
        protected AbpNotificationOptions Options { get; }
        protected INotificationStore Store { get; }
        protected IServiceProvider ServiceProvider { get; }

        public NotificationPublishJob(
            IOptions<AbpNotificationOptions> options,
            INotificationStore store,
            IServiceProvider serviceProvider)
        {
            Store = store;
            Options = options.Value;
            ServiceProvider = serviceProvider;
        }

        public async override Task ExecuteAsync(NotificationPublishJobArgs args)
        {
            var providerType = Type.GetType(args.ProviderType);
            if (providerType != null)
            {
                if (ServiceProvider.GetRequiredService(providerType) is INotificationPublishProvider publishProvider)
                {
                    var notification = await Store.GetNotificationOrNullAsync(args.TenantId, args.NotificationId);
                    notification.Data = NotificationDataConverter.Convert(notification.Data);

                    var notificationDataMapping = Options.NotificationDataMappings
                        .GetMapItemOrDefault(notification.Name, publishProvider.Name);
                    if (notificationDataMapping != null)
                    {
                        notification.Data = notificationDataMapping.MappingFunc(notification.Data);
                    }
                    await publishProvider.PublishAsync(notification, args.UserIdentifiers);
                }
            }
        }
    }
}
