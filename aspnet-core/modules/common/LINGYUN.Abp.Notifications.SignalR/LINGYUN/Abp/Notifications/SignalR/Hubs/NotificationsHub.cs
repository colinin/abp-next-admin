using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Notifications.SignalR.Hubs
{
    [Authorize]
    public class NotificationsHub : AbpHub
    {
        protected INotificationStore NotificationStore => LazyServiceProvider.LazyGetRequiredService<INotificationStore>();

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            if (CurrentTenant.IsAvailable)
            {
                // 以租户为分组，将用户加入租户通讯组
                await Groups.AddToGroupAsync(Context.ConnectionId, CurrentTenant.GetId().ToString(), Context.ConnectionAborted);
            }
            else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Global", Context.ConnectionAborted);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);


            if (CurrentTenant.IsAvailable)
            {
                // 以租户为分组，将移除租户通讯组
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, CurrentTenant.GetId().ToString(), Context.ConnectionAborted);
            }
            else
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Global", Context.ConnectionAborted);
            }
        }

        // [HubMethodName("MySubscriptions")]
        [HubMethodName("my-subscriptions")]
        public virtual async Task<ListResultDto<NotificationSubscriptionInfo>> GetMySubscriptionsAsync()
        {
            var subscriptions = await NotificationStore
                .GetUserSubscriptionsAsync(CurrentTenant.Id, CurrentUser.GetId());

            return new ListResultDto<NotificationSubscriptionInfo>(subscriptions);
        }

        [UnitOfWork]
        // [HubMethodName("GetNotification")]
        [HubMethodName("get-notifications")]
        public virtual async Task<ListResultDto<NotificationInfo>> GetNotificationAsync()
        {
            var userNotifications = await NotificationStore
                .GetUserNotificationsAsync(CurrentTenant.Id, CurrentUser.GetId(), NotificationReadState.UnRead, 10);

            return new ListResultDto<NotificationInfo>(userNotifications);
        }

        // [HubMethodName("ChangeState")]
        [HubMethodName("change-state")]
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
