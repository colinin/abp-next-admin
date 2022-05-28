using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.RealTime.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Localization;
using Volo.Abp.Users;

namespace LY.MicroService.RealtimeMessage.EventBus
{
    public class UserCreateSendWelcomeEventHandler : ILocalEventHandler<EntityCreatedEventData<UserEto>>, ITransientDependency
    {
        private readonly INotificationSender _notificationSender;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        public UserCreateSendWelcomeEventHandler(
            INotificationSender notificationSender,
            INotificationSubscriptionManager notificationSubscriptionManager
            )
        {
            _notificationSender = notificationSender;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<UserEto> eventData)
        {
            var userIdentifer = new UserIdentifier(eventData.Entity.Id, eventData.Entity.UserName);
            // 订阅用户欢迎消息
            await SubscribeInternalNotifers(userIdentifer, eventData.Entity.TenantId);

            var userWelcomeNotifictionData = new NotificationData();

            userWelcomeNotifictionData.TrySetData("user", eventData.Entity.UserName);
            userWelcomeNotifictionData
                .WriteLocalizedData(
                    new LocalizableStringInfo(
                        LocalizationResourceNameAttribute.GetName(typeof(MessageServiceResource)),
                        "WelcomeToApplicationFormUser",
                        new Dictionary<object, object>
                        {
                            { "User", eventData.Entity.UserName }
                        }),
                    new LocalizableStringInfo(
                        LocalizationResourceNameAttribute.GetName(typeof(MessageServiceResource)),
                        "WelcomeToApplicationFormUser",
                        new Dictionary<object, object>
                        {
                            { "User", eventData.Entity.UserName }
                        }),
                    DateTime.Now, eventData.Entity.UserName);

            await _notificationSender
                .SendNofiterAsync(
                    UserNotificationNames.WelcomeToApplication,
                    userWelcomeNotifictionData,
                    userIdentifer,
                    eventData.Entity.TenantId,
                    NotificationSeverity.Info
                );
        }

        private async Task SubscribeInternalNotifers(UserIdentifier userIdentifer, Guid? tenantId = null)
        {
            // 订阅内置通知
            await _notificationSubscriptionManager
                .SubscribeAsync(
                    tenantId,
                    userIdentifer,
                    DefaultNotifications.SystemNotice);
            await _notificationSubscriptionManager
                .SubscribeAsync(
                    tenantId,
                    userIdentifer,
                    DefaultNotifications.OnsideNotice);
            await _notificationSubscriptionManager
                .SubscribeAsync(
                    tenantId,
                    userIdentifer,
                    DefaultNotifications.ActivityNotice);

            // 订阅用户欢迎消息
            await _notificationSubscriptionManager
                .SubscribeAsync(
                    tenantId,
                    userIdentifer,
                    UserNotificationNames.WelcomeToApplication);
        }
    }
}
