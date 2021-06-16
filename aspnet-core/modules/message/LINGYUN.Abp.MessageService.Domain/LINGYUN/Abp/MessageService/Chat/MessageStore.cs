using LINGYUN.Abp.IM.Contract;
using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Group;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class MessageStore : IMessageStore, ITransientDependency
    {
        private readonly IFriendStore _friendStore;

        private readonly IObjectMapper _objectMapper;

        private readonly ICurrentTenant _currentTenant;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IGroupRepository _groupRepository;

        private readonly IMessageRepository _messageRepository;

        private readonly IUserChatSettingRepository _userChatSettingRepository;
        public MessageStore(
            IFriendStore friendStore,
            IObjectMapper objectMapper,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IGroupRepository groupRepository,
            IMessageRepository messageRepository,
            IUserChatSettingRepository userChatSettingRepository)
        {
            _friendStore = friendStore;
            _objectMapper = objectMapper;
            _currentTenant = currentTenant;
            _unitOfWorkManager = unitOfWorkManager;
            _groupRepository = groupRepository;
            _messageRepository = messageRepository;
            _userChatSettingRepository = userChatSettingRepository;
        }

        public virtual async Task StoreMessageAsync(
            ChatMessage chatMessage,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                using (_currentTenant.Change(chatMessage.TenantId))
                {
                    if (!chatMessage.GroupId.IsNullOrWhiteSpace())
                    {
                        long groupId = long.Parse(chatMessage.GroupId);
                        await StoreGroupMessageAsync(chatMessage, groupId, cancellationToken);
                    }
                    else
                    {
                        await StoreUserMessageAsync(chatMessage, cancellationToken);
                    }
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public virtual async Task<List<ChatMessage>> GetGroupMessageAsync(
            Guid? tenantId, 
            long groupId,
            string filter = "",
            string sorting = nameof(ChatMessage.MessageId),
            bool reverse = true, 
            MessageType? type = null, 
            int skipCount = 0, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var groupMessages = await _messageRepository
                    .GetGroupMessagesAsync(groupId, filter, sorting, reverse, type, skipCount, maxResultCount, cancellationToken);

                var chatMessages = _objectMapper.Map<List<GroupMessage>, List<ChatMessage>>(groupMessages);

                return chatMessages;
            }
        }

        public virtual async Task<List<ChatMessage>> GetChatMessageAsync(
            Guid? tenantId, 
            Guid sendUserId, 
            Guid receiveUserId, 
            string filter = "",
            string sorting = nameof(ChatMessage.MessageId),
            bool reverse = true, 
            MessageType? type = null, 
            int skipCount = 0, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var userMessages = await _messageRepository
                    .GetUserMessagesAsync(sendUserId, receiveUserId, filter, sorting, reverse, type, skipCount, maxResultCount, cancellationToken);

                var chatMessages = _objectMapper.Map<List<UserMessage>, List<ChatMessage>>(userMessages);

                return chatMessages;
            }
        }

        public virtual async Task<List<LastChatMessage>> GetLastChatMessagesAsync(
            Guid? tenantId,
            Guid userId,
            string sorting = nameof(LastChatMessage.SendTime),
            bool reverse = true,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
            )
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _messageRepository
                    .GetLastMessagesByOneFriendAsync(userId, sorting, reverse, maxResultCount, cancellationToken);
            }
        }

        public virtual async Task<long> GetGroupMessageCountAsync(
            Guid? tenantId, 
            long groupId, 
            string filter = "",
            MessageType? type = null,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _messageRepository.GetCountAsync(groupId, filter, type, cancellationToken);
            }
        }

        public virtual async Task<long> GetChatMessageCountAsync(
            Guid? tenantId,
            Guid sendUserId, 
            Guid receiveUserId, 
            string filter = "", 
            MessageType? type = null,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _messageRepository.GetCountAsync(sendUserId, receiveUserId, filter, type, cancellationToken);
            }
        }

        protected virtual async Task StoreUserMessageAsync(
            ChatMessage chatMessage,
            CancellationToken cancellationToken = default)
        {
            // 检查接收用户
            if (!chatMessage.ToUserId.HasValue)
            {
                throw new BusinessException(MessageServiceErrorCodes.UseNotFount);
            }

            var myFriend = await _friendStore
                .GetMemberAsync(chatMessage.TenantId, chatMessage.ToUserId.Value, chatMessage.FormUserId, cancellationToken);

            var userChatSetting = await _userChatSettingRepository
                .FindByUserIdAsync(chatMessage.ToUserId.Value, cancellationToken);

            if (userChatSetting != null)
            {
                if (!userChatSetting.AllowReceiveMessage)
                {
                    // 当前发送的用户不接收消息
                    throw new BusinessException(MessageServiceErrorCodes.UserHasRejectAllMessage);
                }

                if (myFriend == null && !chatMessage.IsAnonymous)
                {
                    throw new BusinessException(MessageServiceErrorCodes.UserHasRejectNotFriendMessage);
                }

                if (chatMessage.IsAnonymous && !userChatSetting.AllowAnonymous)
                {
                    // 当前用户不允许匿名发言
                    throw new BusinessException(MessageServiceErrorCodes.UserNotAllowedToSpeakAnonymously);
                }
            }
            else
            {
                if (myFriend == null)
                {
                    throw new BusinessException(MessageServiceErrorCodes.UserHasRejectNotFriendMessage);
                }
            }
            if (myFriend?.Black == true)
            {
                throw new BusinessException(MessageServiceErrorCodes.UserHasBlack);
            }

            var message = new UserMessage(
                long.Parse(chatMessage.MessageId), 
                chatMessage.FormUserId, 
                chatMessage.FormUserName, 
                chatMessage.Content, 
                chatMessage.MessageType);

            message.SendToUser(chatMessage.ToUserId.Value);
            message.SetProperty(nameof(ChatMessage.IsAnonymous), chatMessage.IsAnonymous);

            await _messageRepository.InsertUserMessageAsync(message, cancellationToken);
        }

        protected virtual async Task StoreGroupMessageAsync(
            ChatMessage chatMessage, 
            long groupId,
            CancellationToken cancellationToken = default)
        {
            var userHasBlacked = await _groupRepository
                   .UserHasBlackedAsync(groupId, chatMessage.FormUserId, cancellationToken);
            if (userHasBlacked)
            {
                // 当前发送的用户已被拉黑
                throw new BusinessException(MessageServiceErrorCodes.GroupUserHasBlack);
            }
            var group = await _groupRepository.GetByIdAsync(groupId, cancellationToken);
            if (!group.AllowSendMessage)
            {
                // 当前群组不允许发言
                throw new BusinessException(MessageServiceErrorCodes.GroupNotAllowedToSpeak);
            }
            if (chatMessage.IsAnonymous && !group.AllowAnonymous)
            {
                // 当前群组不允许匿名发言
                throw new BusinessException(MessageServiceErrorCodes.GroupNotAllowedToSpeakAnonymously);
            }

            var message = new GroupMessage(
                long.Parse(chatMessage.MessageId), 
                chatMessage.FormUserId, 
                chatMessage.FormUserName, 
                chatMessage.Content, 
                chatMessage.MessageType);

            message.SendToGroup(groupId);
            message.SetProperty(nameof(ChatMessage.IsAnonymous), chatMessage.IsAnonymous);

            await _messageRepository.InsertGroupMessageAsync(message, cancellationToken);
        }
    }
}
