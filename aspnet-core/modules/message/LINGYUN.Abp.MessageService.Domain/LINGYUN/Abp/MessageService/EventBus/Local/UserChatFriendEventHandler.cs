using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.RealTime.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Localization;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.EventBus.Local
{
    public class UserChatFriendEventHandler :
        ILocalEventHandler<EntityCreatedEventData<UserChatFriend>>,
        ILocalEventHandler<EntityDeletedEventData<UserChatFriend>>,
        ILocalEventHandler<EntityUpdatedEventData<UserChatFriend>>,
        ILocalEventHandler<UserChatFriendEto>,
        ITransientDependency
    {
        private IStringLocalizer _stringLocalizer;
        private IMessageSender _messageSender;
        private INotificationSender _notificationSender;
        private IDistributedCache<UserFriendCacheItem> _cache;
        private ICurrentUser _currentUser;

        public UserChatFriendEventHandler(
            ICurrentUser currentUser,
            IMessageSender messageSender,
            INotificationSender notificationSender,
            IDistributedCache<UserFriendCacheItem> cache,
            IStringLocalizer<MessageServiceResource> stringLocalizer)
        {
            _cache = cache;
            _currentUser = currentUser;
            _messageSender = messageSender;
            _stringLocalizer = stringLocalizer;
            _notificationSender = notificationSender;
        }

        public virtual async Task HandleEventAsync(EntityCreatedEventData<UserChatFriend> eventData)
        {
            switch (eventData.Entity.Status)
            {
                case IM.Contract.UserFriendStatus.NeedValidation:
                    await SendFriendValidationNotifierAsync(eventData.Entity);
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

            var addNewFirendMessage = new ChatMessage
            {
                TenantId = tenantId,
                FormUserId = _currentUser.GetId(), // 本地事件中可以获取到当前用户信息
                FormUserName = _currentUser.UserName,
                SendTime = DateTime.Now,
                MessageType = MessageType.Text,
                ToUserId = friendId,
                Content = _stringLocalizer["Messages:NewFriend"]
            };

            await _messageSender.SendMessageAsync(addNewFirendMessage);
        }

        protected virtual async Task SendFriendValidationNotifierAsync(UserChatFriend userChatFriend)
        {
            // 发送好友验证通知
            var userIdentifer = new UserIdentifier(userChatFriend.FrientId, userChatFriend.RemarkName);

            var friendValidationNotifictionData = new NotificationData();
            friendValidationNotifictionData
                .WriteLocalizedData(
                    new LocalizableStringInfo(
                        LocalizationResourceNameAttribute.GetName(typeof(MessageServiceResource)),
                        "Notifications:FriendValidation"),
                    new LocalizableStringInfo(
                        LocalizationResourceNameAttribute.GetName(typeof(MessageServiceResource)),
                        "Notifications:RequestAddNewFriend",
                        new Dictionary<object, object> { { "name", _currentUser.UserName } }),
                    DateTime.Now,
                    _currentUser.UserName,
                    new LocalizableStringInfo(
                        LocalizationResourceNameAttribute.GetName(typeof(MessageServiceResource)),
                        "Notifications:RequestAddNewFriendDetail",
                        new Dictionary<object, object> { { "description", userChatFriend.Description } }));
            friendValidationNotifictionData.TrySetData("userId", userChatFriend.UserId);
            friendValidationNotifictionData.TrySetData("frientId", userChatFriend.FrientId);

            await _notificationSender
                .SendNofiterAsync(
                    MessageServiceNotificationNames.IM.FriendValidation,
                    friendValidationNotifictionData,
                    userIdentifer,
                    userChatFriend.TenantId);
        }

        protected virtual async Task RemoveUserFriendCacheItemAsync(Guid userId)
        {
            // 移除好友缓存
            await _cache.RemoveAsync(UserFriendCacheItem.CalculateCacheKey(userId.ToString()));
        }
    }
}
