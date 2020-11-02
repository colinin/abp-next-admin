using LINGYUN.Abp.RealTime.Client;
using LINGYUN.Abp.RealTime.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Notifications.SignalR.Hubs
{
    [Authorize]
    public class NotificationsHub : OnlineClientHubBase
    {
        private INotificationStore _notificationStore;
        protected INotificationStore NotificationStore => LazyGetRequiredService(ref _notificationStore);

        protected override async Task OnClientConnectedAsync(IOnlineClient client)
        {
            await base.OnClientConnectedAsync(client);

            if (client.TenantId.HasValue)
            {
                // 以租户为分组，将用户加入租户通讯组
                await Groups.AddToGroupAsync(client.ConnectionId, client.TenantId.Value.ToString(), Context.ConnectionAborted);
            }
            else
            {
                await Groups.AddToGroupAsync(client.ConnectionId, "Global", Context.ConnectionAborted);
            }
        }

        protected override async Task OnClientDisconnectedAsync(IOnlineClient client)
        {
            await base.OnClientDisconnectedAsync(client);

            if (client.TenantId.HasValue)
            {
                // 以租户为分组，将移除租户通讯组
                await Groups.RemoveFromGroupAsync(client.ConnectionId, client.TenantId.Value.ToString(), Context.ConnectionAborted);
            }
            else
            {
                await Groups.RemoveFromGroupAsync(client.ConnectionId, "Global", Context.ConnectionAborted);
            }
        }

        [HubMethodName("MySubscriptions")]
        public virtual async Task<ListResultDto<NotificationSubscriptionInfo>> GetMySubscriptionsAsync()
        {
            var subscriptions = await NotificationStore
                .GetUserSubscriptionsAsync(CurrentTenant.Id, CurrentUser.GetId());

            return new ListResultDto<NotificationSubscriptionInfo>(subscriptions);
        }

        [UnitOfWork]
        [HubMethodName("GetNotification")]
        public virtual async Task<ListResultDto<NotificationInfo>> GetNotificationAsync()
        {
            var userNotifications = await NotificationStore
                .GetUserNotificationsAsync(CurrentTenant.Id, CurrentUser.GetId(), NotificationReadState.UnRead, 10);

            return new ListResultDto<NotificationInfo>(userNotifications);
        }

        [HubMethodName("ChangeState")]
        public virtual async Task ChangeStateAsync(string id, NotificationReadState readState = NotificationReadState.Read)
        {
            await NotificationStore
                .ChangeUserNotificationReadStateAsync(
                    CurrentTenant.Id,
                    CurrentUser.GetId(), 
                    long.Parse(id), 
                    readState,
                    Context.ConnectionAborted);
        }
    }
}
