using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Notifications.SignalR.Hubs
{
    [Authorize]
    public class NotificationsHub : OnlineClientHubBase
    {
        private INotificationStore _notificationStore;
        protected INotificationStore NotificationStore => LazyGetRequiredService(ref _notificationStore);

        [HubMethodName("GetNotification")]
        public virtual async Task<ListResultDto<NotificationInfo>> GetNotificationAsync(
            NotificationReadState readState = NotificationReadState.UnRead, int maxResultCount = 10)
        {
            var userNotifications = await NotificationStore.GetUserNotificationsAsync(CurrentTenant.Id, CurrentUser.GetId(), readState, maxResultCount);

            return new ListResultDto<NotificationInfo>(userNotifications);
        }

        [HubMethodName("ChangeState")]
        public virtual async Task ChangeStateAsync(long id, NotificationReadState readState = NotificationReadState.Read)
        {
            await NotificationStore.ChangeUserNotificationReadStateAsync(CurrentTenant.Id, CurrentUser.GetId(), id, readState);
        }


    }
}
