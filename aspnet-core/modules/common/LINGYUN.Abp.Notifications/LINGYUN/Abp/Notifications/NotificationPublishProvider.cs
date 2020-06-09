using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications
{
    public abstract class NotificationPublishProvider : INotificationPublishProvider, ITransientDependency
    {
        public abstract string Name { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected NotificationPublishProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers);
    }
}
