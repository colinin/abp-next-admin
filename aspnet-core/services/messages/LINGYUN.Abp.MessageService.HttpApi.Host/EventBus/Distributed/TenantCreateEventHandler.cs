using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.MultiTenancy;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.EventBus.Distributed
{
    public class TenantCreateEventHandler : IDistributedEventHandler<CreateEventData>, ITransientDependency
    {
        protected ILogger<TenantCreateEventHandler> Logger { get; }
        protected INotificationSender NotificationSender { get; }
        protected INotificationSubscriptionManager NotificationSubscriptionManager { get; }

        public TenantCreateEventHandler(
            INotificationSender notificationSender,
            INotificationSubscriptionManager notificationSubscriptionManager,
            ILogger<TenantCreateEventHandler> logger)
        {
            Logger = logger;
            NotificationSender = notificationSender;
            NotificationSubscriptionManager = notificationSubscriptionManager;
        }

        public async Task HandleEventAsync(CreateEventData eventData)
        {
            var tenantAdminUserIdentifier = new UserIdentifier(eventData.AdminUserId, eventData.AdminEmailAddress);

            Logger.LogInformation("tenant administrator subscribes to new tenant events..");
            // 租户管理员订阅事件
            await NotificationSubscriptionManager
                .SubscribeAsync(
                    eventData.Id,
                    tenantAdminUserIdentifier,
                    TenantNotificationNames.NewTenantRegistered);

            var notificationData = new NotificationData();
            notificationData.WriteLocalizedData(
                new LocalizableStringInfo(
                    LocalizationResourceNameAttribute.GetName(typeof(MessageServiceResource)),
                    "NewTenantRegisteredNotificationTitle",
                    new Dictionary<object, object>
                    {
                            { "User", eventData.Name }
                    }),
                new LocalizableStringInfo(
                    LocalizationResourceNameAttribute.GetName(typeof(MessageServiceResource)),
                    "NewTenantRegisteredNotificationMessage",
                    new Dictionary<object, object>
                    {
                            { "User", eventData.Name}
                    }),
                DateTime.Now, eventData.AdminEmailAddress);

            Logger.LogInformation("publish new tenant notification..");
            // 发布租户创建通知
            await NotificationSender
                .SendNofiterAsync(
                    TenantNotificationNames.NewTenantRegistered,
                    notificationData,
                    tenantAdminUserIdentifier,
                    eventData.Id,
                    NotificationSeverity.Success);
        }
    }
}
