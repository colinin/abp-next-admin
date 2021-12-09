using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.Notifications;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace LY.MicroService.RealtimeMessage.EventBus.Distributed
{
    public class UserCreateJoinIMEventHandler : ILocalEventHandler<EntityCreatedEventData<UserEto>>, ITransientDependency
    {
        private readonly IChatDataSeeder _chatDataSeeder;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        public UserCreateJoinIMEventHandler(
            IChatDataSeeder chatDataSeeder,
            INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _chatDataSeeder = chatDataSeeder;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }
        /// <summary>
        /// 接收添加用户事件,初始化IM用户种子
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task HandleEventAsync(EntityCreatedEventData<UserEto> eventData)
        {
            await SeedChatDataAsync(eventData.Entity);

            await SeedUserSubscriptionNotifiersAsync(eventData.Entity);
        }

        protected virtual async Task SeedChatDataAsync(IUserData user)
        {
            await _chatDataSeeder.SeedAsync(user);
        }

        protected virtual async Task SeedUserSubscriptionNotifiersAsync(IUserData user)
        {
            var userIdentifier = new UserIdentifier(user.Id, user.UserName);

            await _notificationSubscriptionManager
                .SubscribeAsync(
                    user.TenantId,
                    userIdentifier,
                    MessageServiceNotificationNames.IM.FriendValidation);

            await _notificationSubscriptionManager
                .SubscribeAsync(
                    user.TenantId,
                    userIdentifier,
                    MessageServiceNotificationNames.IM.NewFriend);
        }
    }
}
