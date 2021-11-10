using LINGYUN.Abp.IM.Groups;
using LINGYUN.Abp.IM.Messages;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Chat
{
    [Authorize]
    public class ChatAppService : ApplicationService, IChatAppService
    {
        protected IMessageSender MessageSender => LazyServiceProvider.LazyGetRequiredService<IMessageSender>();

        private readonly IUserGroupStore _userGroupStore;
        private readonly IMessageStore _messageStore;

        public ChatAppService(
            IMessageStore messageStore,
            IUserGroupStore userGroupStore)
        {
            _messageStore = messageStore;
            _userGroupStore = userGroupStore;
        }

        public virtual async Task<PagedResultDto<ChatMessage>> GetMyChatMessageAsync(UserMessageGetByPagedDto input)
        {
            var chatMessageCount = await _messageStore
                .GetChatMessageCountAsync(
                    CurrentTenant.Id,
                    CurrentUser.GetId(),
                    input.ReceiveUserId,
                    input.MessageType,
                    input.Filter);

            var chatMessages = await _messageStore
                .GetChatMessageAsync(
                    CurrentTenant.Id,
                    CurrentUser.GetId(),
                    input.ReceiveUserId,
                    input.MessageType,
                    input.Filter,
                    input.Sorting,
                    input.SkipCount,
                    input.MaxResultCount);

            return new PagedResultDto<ChatMessage>(chatMessageCount, chatMessages);
        }

        public virtual async Task<ListResultDto<LastChatMessage>> GetMyLastChatMessageAsync(GetUserLastMessageDto input)
        {
            var chatMessages = await _messageStore
                .GetLastChatMessagesAsync(
                    CurrentTenant.Id,
                    CurrentUser.GetId(),
                    input.State,
                    input.Sorting,
                    input.MaxResultCount);

            return new ListResultDto<LastChatMessage>(chatMessages);
        }

        public virtual async Task<PagedResultDto<ChatMessage>> GetMyGroupMessageAsync(GroupMessageGetByPagedDto input)
        {
            if (!await _userGroupStore.MemberHasInGroupAsync(CurrentTenant.Id, input.GroupId, CurrentUser.GetId()))
            {
                throw new BusinessException(MessageServiceErrorCodes.YouHaveNotJoinedGroup);
            }

            var groupMessageCount = await _messageStore
                .GetGroupMessageCountAsync(
                    CurrentTenant.Id,
                    input.GroupId,
                    input.MessageType,
                    input.Filter);

            var groupMessages = await _messageStore
                .GetGroupMessageAsync(
                    CurrentTenant.Id,
                    input.GroupId,
                    input.MessageType,
                    input.Filter,
                    input.Sorting,
                    input.SkipCount,
                    input.MaxResultCount);

            return new PagedResultDto<ChatMessage>(groupMessageCount, groupMessages);
        }


        public virtual async Task<ChatMessageSendResultDto> SendMessageAsync(ChatMessage input)
        {
            // TODO：向其他租户发送消息?
            input.TenantId ??= CurrentTenant.Id;

            var messageId = await MessageSender.SendMessageAsync(input);

            return new ChatMessageSendResultDto(messageId);
        }
    }
}
