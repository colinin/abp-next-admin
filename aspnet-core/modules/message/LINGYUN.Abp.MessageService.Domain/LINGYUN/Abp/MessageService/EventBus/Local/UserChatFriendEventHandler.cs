using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.MessageService.EventBus.Local
{
    public class UserChatFriendEventHandler : 
        ILocalEventHandler<EntityCreatedEventData<UserChatFriend>>,
        ILocalEventHandler<EntityDeletedEventData<UserChatFriend>>,
        ILocalEventHandler<EntityUpdatedEventData<UserChatFriend>>,
        ILocalEventHandler<UserChatFriendEto>,
        ITransientDependency
    {
        private ILogger _logger;
        private IMessageSender _messageSender;
        private INotificationDispatcher _dispatcher;
        private IDistributedCache<UserFriendCacheItem> _cache;

        public UserChatFriendEventHandler(
            IMessageSender messageSender,
            INotificationDispatcher dispatcher,
            ILogger<UserChatFriendEventHandler> logger)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _messageSender = messageSender;
        }

        public virtual async Task HandleEventAsync(EntityCreatedEventData<UserChatFriend> eventData)
        {
            switch (eventData.Entity.Status)
            {
                case IM.Contract.UserFriendStatus.Added:
                    await SendFriendAddedMessageAsync(eventData.Entity.UserId, eventData.Entity.FrientId, eventData.Entity.TenantId);
                    break;
                case IM.Contract.UserFriendStatus.NeedValidation:
                    await SendFriendValidationNotiferAsync(eventData.Entity.UserId, eventData.Entity.FrientId, eventData.Entity.TenantId);
                    break;
            }
            await RemoveUserFriendCacheItemAsync(eventData.Entity.UserId);
        }

        public virtual async Task HandleEventAsync(EntityDeletedEventData<UserChatFriend> eventData)
        {
            await RemoveUserFriendCacheItemAsync(eventData.Entity.UserId);
        }

        public virtual async Task HandleEventAsync(EntityUpdatedEventData<UserChatFriend> eventData)
        {
            await RemoveUserFriendCacheItemAsync(eventData.Entity.UserId);
        }

        public virtual async Task HandleEventAsync(UserChatFriendEto eventData)
        {
            if (eventData.Status == IM.Contract.UserFriendStatus.Added)
            {
                await SendFriendAddedMessageAsync(eventData.UserId, eventData.FrientId, eventData.TenantId);
            }
        }

        protected virtual async Task SendFriendAddedMessageAsync(Guid userId, Guid friendId, Guid? tenantId = null)
        {
            // 发送添加好友的第一条消息
        }

        protected virtual async Task SendFriendValidationNotiferAsync(Guid userId, Guid friendId, Guid? tenantId = null)
        {
            // 发送好友验证通知
        }

        protected virtual async Task RemoveUserFriendCacheItemAsync(Guid userId)
        {
            // 移除好友缓存
            await _cache.RemoveAsync(UserFriendCacheItem.CalculateCacheKey(userId.ToString()));
        }
    }
}
