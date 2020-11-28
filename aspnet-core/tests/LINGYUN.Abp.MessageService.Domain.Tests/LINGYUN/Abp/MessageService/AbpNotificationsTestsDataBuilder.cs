using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MessageService
{
    public class AbpNotificationsTestDataBuilder : ITransientDependency
    {
        private readonly INotificationStore _notificationStore;
        private readonly IUserNotificationRepository _userNotificationRepository;
        public AbpNotificationsTestDataBuilder(
            INotificationStore notificationStore,
            IUserNotificationRepository userNotificationRepository)
        {
            _notificationStore = notificationStore;
            _userNotificationRepository = userNotificationRepository;
        }

        public async Task BuildAsync()
        {
            await AddNotificationsAsync();
        }

        private async Task AddUserNotificationsAsync(NotificationInfo notificationInfo, IEnumerable<Guid> userIds)
        {
            await _notificationStore.InsertUserNotificationsAsync(notificationInfo, userIds);
        }

        private async Task AddNotificationsAsync()
        {
            var notification1 = new NotificationInfo
            {
                Name = NotificationsTestsNames.Test1,
                Severity = NotificationSeverity.Success,
                CreationTime = DateTime.Now,
                Lifetime = NotificationLifetime.OnlyOne,
                Type = NotificationType.Application
            };

            var notification2 = new NotificationInfo
            {
                Name = NotificationsTestsNames.Test2,
                Severity = NotificationSeverity.Success,
                CreationTime = DateTime.Now,
                Lifetime = NotificationLifetime.Persistent,
                Type = NotificationType.Application
            };

            var notification3 = new NotificationInfo
            {
                Name = NotificationsTestsNames.Test3,
                Severity = NotificationSeverity.Success,
                CreationTime = DateTime.Now,
                Lifetime = NotificationLifetime.OnlyOne,
                Type = NotificationType.User
            };

            await _notificationStore.InsertNotificationAsync(notification1);
            await _notificationStore.InsertNotificationAsync(notification2);
            await _notificationStore.InsertNotificationAsync(notification3);

            NotificationsTestConsts.NotificationId1 = notification1.GetId();
            NotificationsTestConsts.NotificationId2 = notification2.GetId();
            NotificationsTestConsts.NotificationId3 = notification3.GetId();

            await AddUserNotificationsAsync(notification2, new Guid[] { NotificationsTestConsts.User1Id, NotificationsTestConsts.User2Id });
            await AddUserNotificationsAsync(notification3, new Guid[] { NotificationsTestConsts.User1Id });

            await _notificationStore.ChangeUserNotificationReadStateAsync(null, NotificationsTestConsts.User1Id, NotificationsTestConsts.NotificationId3, NotificationReadState.Read);
        }
    }
}
