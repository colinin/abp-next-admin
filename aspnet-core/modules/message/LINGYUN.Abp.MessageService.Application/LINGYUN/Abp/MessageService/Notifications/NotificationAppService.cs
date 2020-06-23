using LINGYUN.Abp.MessageService.Permissions;
using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Notifications
{
    [Authorize]
    public class NotificationAppService : ApplicationService, INotificationAppService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        public NotificationAppService(
            INotificationRepository notificationRepository,
            IUserNotificationRepository userNotificationRepository)
        {
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
        }

        public virtual async Task ChangeUserNotificationReadStateAsync(UserNotificationChangeReadStateDto userNotificationChangeRead)
        {
            await _userNotificationRepository.ChangeUserNotificationReadStateAsync(
                CurrentUser.GetId(), userNotificationChangeRead.NotificationId, userNotificationChangeRead.ReadState);

        }

        [Authorize(MessageServicePermissions.Notification.Delete)]
        public virtual async Task DeleteAsync(NotificationGetByIdDto notificationGetById)
        {
            await _notificationRepository.DeleteAsync(notificationGetById.NotificationId);
        }

        public virtual async Task DeleteUserNotificationAsync(NotificationGetByIdDto notificationGetById)
        {
            var notify = await _userNotificationRepository
                .GetByIdAsync(CurrentUser.GetId(), notificationGetById.NotificationId);
            await _userNotificationRepository.DeleteAsync(notify.Id);
        }

        public virtual async Task<NotificationInfo> GetAsync(NotificationGetByIdDto notificationGetById)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationGetById.NotificationId);

            return ObjectMapper.Map<Notification, NotificationInfo>(notification);
        }

        public virtual async Task<PagedResultDto<NotificationInfo>> GetUserNotificationsAsync(UserNotificationGetByPagedDto userNotificationGetByPaged)
        {
            var notificationCount = await _userNotificationRepository
                .GetCountAsync(CurrentUser.GetId(), userNotificationGetByPaged.Filter,
                                userNotificationGetByPaged.ReadState);

            var notifications = await _userNotificationRepository
                .GetNotificationsAsync(CurrentUser.GetId(), userNotificationGetByPaged.Filter,
                userNotificationGetByPaged.Sorting, userNotificationGetByPaged.ReadState,
                userNotificationGetByPaged.SkipCount, userNotificationGetByPaged.MaxResultCount
                );

            return new PagedResultDto<NotificationInfo>(notificationCount,
                ObjectMapper.Map<List<Notification>, List<NotificationInfo>>(notifications));
        }
    }
}
