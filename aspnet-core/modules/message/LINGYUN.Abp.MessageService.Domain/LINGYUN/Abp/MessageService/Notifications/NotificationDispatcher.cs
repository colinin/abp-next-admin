using LINGYUN.Abp.MessageService.Subscriptions;
using LINGYUN.Abp.MessageService.Utils;
using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class NotificationDispatcher : INotificationDispatcher, ITransientDependency
    {
        protected IJsonSerializer JsonSerializer { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected ISubscribeStore SubscribeStore { get; }
        protected INotificationStore NotificationStore { get; }
        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected INotificationPublisher NotificationPublisher { get; }

        protected ISnowflakeIdGenerator SnowflakeIdGenerator { get; }

        [UnitOfWork]
        public virtual async Task DispatcheAsync(NotificationInfo notification)
        {
            using (CurrentTenant.Change(notification.TenantId))
            {
                var subscribeUsers = await SubscribeStore.GetUserSubscribesAsync(notification.Name);
                foreach(var userId in subscribeUsers)
                {
                    await NotificationStore.InsertUserNotificationAsync(notification, userId);
                }
                await CurrentUnitOfWork.SaveChangesAsync();

                await NotifyAsync(notification.Data, notification.TenantId, subscribeUsers);
            }
        }

        protected virtual async Task NotifyAsync(NotificationData data, Guid? tenantId, IEnumerable<Guid> userIds)
        {
            await NotificationPublisher.PublishAsync(data, userIds, tenantId);
        }
    }
}
