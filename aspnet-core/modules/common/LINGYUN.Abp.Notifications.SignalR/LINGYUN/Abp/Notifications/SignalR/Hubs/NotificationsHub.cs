using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Notifications.SignalR.Hubs
{
    [Authorize]
    public class NotificationsHub : OnlineClientHubBase
    {
        private INotificationStore _notificationStore;
        protected INotificationStore NotificationStore => LazyGetRequiredService(ref _notificationStore);

        [HubMethodName("GetNotification")]
        public virtual async Task<ListResultDto<NotificationInfo>> GetNotificationAsync(NotificationReadState readState = NotificationReadState.UnRead)
        {
            var userNotifications = await NotificationStore.GetUserNotificationsAsync(CurrentTenant.Id, CurrentUser.GetId(), readState);

            return new ListResultDto<NotificationInfo>(userNotifications);
        }
    }
}
