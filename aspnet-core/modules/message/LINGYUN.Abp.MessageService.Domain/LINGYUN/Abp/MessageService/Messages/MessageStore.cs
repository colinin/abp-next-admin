using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Messages
{
    public class MessageStore : DomainService, IMessageStore
    {
        private IObjectMapper _objectMapper;
        protected IObjectMapper ObjectMapper => LazyGetRequiredService(ref _objectMapper);

        private IUnitOfWorkManager _unitOfWorkManager;
        protected IUnitOfWorkManager UnitOfWorkManager => LazyGetRequiredService(ref _unitOfWorkManager);
        protected IUserChatSettingRepository UserChatSettingRepository { get; }
        protected IMessageRepository MessageRepository { get; }
        protected IGroupRepository GroupRepository { get; }
        protected ISnowflakeIdGenerator SnowflakeIdGenerator { get; }
        public MessageStore(
            IGroupRepository groupRepository,
            IMessageRepository messageRepository,
            ISnowflakeIdGenerator snowflakeIdGenerator,
            IUserChatSettingRepository userChatSettingRepository)
        {
            GroupRepository = groupRepository;
            MessageRepository = messageRepository;
            SnowflakeIdGenerator = snowflakeIdGenerator;
            UserChatSettingRepository = userChatSettingRepository;
        }

        [UnitOfWork]
        public async Task StoreMessageAsync(ChatMessage chatMessage)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                using (CurrentTenant.Change(chatMessage.TenantId))
                {
                    if (!chatMessage.GroupId.IsNullOrWhiteSpace())
                    {
                        long groupId = long.Parse(chatMessage.GroupId);
                        await StoreGroupMessageAsync(chatMessage, groupId);
                    }
                    else
                    {
                        await StoreUserMessageAsync(chatMessage);
                    }
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task<List<ChatMessage>> GetGroupMessageAsync(Guid? tenantId, long groupId, string filter = "", MessageType type = MessageType.Text, int skipCount = 1, int maxResultCount = 10)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var groupMessages = await MessageRepository.GetGroupMessagesAsync(groupId, filter, type, skipCount, maxResultCount);
                var chatMessages = ObjectMapper.Map<List<GroupMessage>, List<ChatMessage>>(groupMessages);

                return chatMessages;
            }
        }

        public async Task<List<ChatMessage>> GetChatMessageAsync(Guid? tenantId, Guid sendUserId, Guid receiveUserId, string filter = "", MessageType type = MessageType.Text, int skipCount = 1, int maxResultCount = 10)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var userMessages = await MessageRepository.GetUserMessagesAsync(sendUserId, receiveUserId, filter, type, skipCount, maxResultCount);
                var chatMessages = ObjectMapper.Map<List<UserMessage>, List<ChatMessage>>(userMessages);

                return chatMessages;
            }
        }
    
        protected virtual async Task StoreUserMessageAsync(ChatMessage chatMessage)
        {
            var userHasBlacked = await UserChatSettingRepository
                            .UserHasBlackedAsync(chatMessage.ToUserId.Value, chatMessage.FormUserId);
            if (userHasBlacked)
            {
                // 当前发送的用户已被拉黑
                throw new BusinessException(MessageServiceErrorCodes.UserHasBlack);
            }
            var userChatSetting = await UserChatSettingRepository.GetByUserIdAsync(chatMessage.ToUserId.Value);
            if (!userChatSetting.AllowReceiveMessage)
            {
                // 当前发送的用户不接收消息
                throw new BusinessException(MessageServiceErrorCodes.UserHasRejectAllMessage);
            }
            if (chatMessage.IsAnonymous && !userChatSetting.AllowAnonymous)
            {
                // 当前用户不允许匿名发言
                throw new BusinessException(MessageServiceErrorCodes.UserNotAllowedToSpeakAnonymously);
            }
            var messageId = SnowflakeIdGenerator.Create();
            var message = new UserMessage(messageId, chatMessage.FormUserId, chatMessage.FormUserName, chatMessage.Content, chatMessage.MessageType);
            message.SendToUser(chatMessage.ToUserId.Value);
            await MessageRepository.InsertUserMessageAsync(message);
        }

        protected virtual async Task StoreGroupMessageAsync(ChatMessage chatMessage, long groupId)
        {
            var userHasBlacked = await GroupRepository
                   .UserHasBlackedAsync(groupId, chatMessage.FormUserId);
            if (userHasBlacked)
            {
                // 当前发送的用户已被拉黑
                throw new BusinessException(MessageServiceErrorCodes.GroupUserHasBlack);
            }
            var group = await GroupRepository.GetByIdAsync(groupId);
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
            var messageId = SnowflakeIdGenerator.Create();
            var message = new GroupMessage(messageId, chatMessage.FormUserId, chatMessage.FormUserName, chatMessage.Content, chatMessage.MessageType);
            // TODO: 需要压测 高并发场景下的装箱性能影响
            message.SendToGroup(groupId);
            await MessageRepository.InsertGroupMessageAsync(message);
        }
    }
}
