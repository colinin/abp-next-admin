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
    public class MyNotificationAppService : ApplicationService, IMyNotificationAppService
    {
        protected INotificationSender NotificationSender { get; }

        protected INotificationStore NotificationStore { get; }

        protected INotificationDefinitionManager NotificationDefinitionManager { get; }

        public MyNotificationAppService(
            INotificationStore notificationStore,
            INotificationSender notificationSender,
            INotificationDefinitionManager notificationDefinitionManager)
        {
            NotificationStore = notificationStore;
            NotificationSender = notificationSender;
            NotificationDefinitionManager = notificationDefinitionManager;
        }

        public virtual async Task SendNofiterAsync(NotificationSendDto input)
        {
            UserIdentifier user = null;
            if (input.ToUserId.HasValue)
            {
                user = new UserIdentifier(input.ToUserId.Value, input.ToUserName);
            }
            await NotificationSender
                .SendNofiterAsync(
                    input.Name,
                    input.Data,
                    user,
                    CurrentTenant.Id,
                    input.Severity);
        }

        public virtual async Task DeleteAsync(long id)
        {
            await NotificationStore
                .DeleteUserNotificationAsync(
                    CurrentTenant.Id,
                    CurrentUser.GetId(),
                    id);
        }

        public virtual Task<ListResultDto<NotificationGroupDto>> GetAssignableNotifiersAsync()
        {
            var groups = new List<NotificationGroupDto>();

            foreach (var group in NotificationDefinitionManager.GetGroups())
            {
                if (!group.AllowSubscriptionToClients)
                {
                    continue;

                }
                var notificationGroup = new NotificationGroupDto
                {
                    Name = group.Name,
                    DisplayName = group.DisplayName.Localize(StringLocalizerFactory)
                };

                foreach (var notification in group.Notifications)
                {
                    if (!notification.AllowSubscriptionToClients)
                    {
                        continue;
                    }

                    var notificationChildren = new NotificationDto
                    {
                        Name = notification.Name,
                        DisplayName = notification.DisplayName.Localize(StringLocalizerFactory),
                        Description = notification.Description.Localize(StringLocalizerFactory),
                        Lifetime = notification.NotificationLifetime,
                        Type = notification.NotificationType
                    };

                    notificationGroup.Notifications.Add(notificationChildren);
                }

                groups.Add(notificationGroup);
            }

            return Task.FromResult(new ListResultDto<NotificationGroupDto>(groups));
        }

        public virtual async Task<NotificationInfo> GetAsync(long id)
        {
            return await NotificationStore
                .GetNotificationOrNullAsync(CurrentTenant.Id, id);
        }

        public virtual async Task<PagedResultDto<NotificationInfo>> GetListAsync(UserNotificationGetByPagedDto input)
        {
            var notificationCount = await NotificationStore
                .GetUserNotificationsCountAsync(
                    CurrentTenant.Id, 
                    CurrentUser.GetId(),
                    input.Filter,
                    input.ReadState);

            var notifications = await NotificationStore
                .GetUserNotificationsAsync(
                    CurrentTenant.Id, CurrentUser.GetId(),
                    input.Filter, input.Sorting,
                    input.ReadState, input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<NotificationInfo>(notificationCount, notifications);
        }
    }
}
