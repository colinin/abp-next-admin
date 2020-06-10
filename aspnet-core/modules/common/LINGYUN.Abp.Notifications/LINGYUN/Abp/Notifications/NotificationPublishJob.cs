using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationPublishJob : AsyncBackgroundJob<NotificationPublishJobArgs>, ITransientDependency
    {
        protected INotificationStore Store { get; }
        protected IServiceProvider ServiceProvider { get; }

        public NotificationPublishJob(
            INotificationStore store,
            IServiceProvider serviceProvider)
        {
            Store = store;
            ServiceProvider = serviceProvider;
        }

        public override async Task ExecuteAsync(NotificationPublishJobArgs args)
        {
            var providerType = Type.GetType(args.ProviderType);

            if (ServiceProvider.GetRequiredService(providerType) is INotificationPublishProvider publishProvider)
            {
                var notification = await Store.GetNotificationOrNullAsync(args.TenantId, args.NotificationId);

                await publishProvider.PublishAsync(notification, args.UserIdentifiers);
            }
        }
    }
}
