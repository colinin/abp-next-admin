using LINGYUN.Abp.IM.Group;
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
                .GetChatMessageCountAsync(CurrentTenant.Id, CurrentUser.GetId(), input.ReceiveUserId,
                    input.Filter, input.MessageType);

            var chatMessages = await _messageStore
                .GetChatMessageAsync(CurrentTenant.Id, CurrentUser.GetId(), input.ReceiveUserId, 
                    input.Filter, input.Sorting, input.Reverse,
                    input.MessageType, input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<ChatMessage>(chatMessageCount, chatMessages);
        }

        public virtual async Task<ListResultDto<LastChatMessage>> GetMyLastChatMessageAsync(GetUserLastMessageDto input)
        {
            var chatMessages = await _messageStore
                .GetLastChatMessagesAsync(CurrentTenant.Id, CurrentUser.GetId(),
                    input.Sorting, input.Reverse, input.MaxResultCount);

            return new ListResultDto<LastChatMessage>(chatMessages);
        }

        public virtual async Task<PagedResultDto<ChatMessage>> GetMyGroupMessageAsync(GroupMessageGetByPagedDto input)
        {
            if (! await _userGroupStore.MemberHasInGroupAsync(CurrentTenant.Id, input.GroupId, CurrentUser.GetId()))
            {
                throw new BusinessException(MessageServiceErrorCodes.YouHaveNotJoinedGroup);
            }

            var groupMessageCount = await _messageStore
                .GetGroupMessageCountAsync(CurrentTenant.Id, input.GroupId,
                    input.Filter, input.MessageType);

            var groupMessages = await _messageStore
                .GetGroupMessageAsync(CurrentTenant.Id, input.GroupId,
                    input.Filter, input.Sorting, input.Reverse,
                    input.MessageType, input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<ChatMessage>(groupMessageCount, groupMessages);
        }

        //public virtual async Task<PagedResultDto<GroupUserCard>> GetGroupUsersAsync(GroupUserGetByPagedDto input)
        //{
        //    var groupUserCardCount = await _userGroupStore
        //        .GetMembersCountAsync(CurrentTenant.Id, input.GroupId);

        //    var groupUserCards = await _userGroupStore.GetMembersAsync(CurrentTenant.Id,
        //        input.GroupId, input.Sorting, input.Reverse,
        //        input.SkipCount, input.MaxResultCount);

        //    return new PagedResultDto<GroupUserCard>(groupUserCardCount, groupUserCards);
        //}

        //[Authorize]
        //public virtual async Task<ListResultDto<Group>> GetMyGroupsAsync()
        //{
        //    var myGroups = await _userGroupStore.GetUserGroupsAsync(CurrentTenant.Id, CurrentUser.GetId());

        //    return new ListResultDto<Group>(myGroups.ToImmutableList());
        //}

        //public virtual async Task GroupAcceptUserAsync(GroupAcceptUserDto input)
        //{
        //    var myGroupCard = await _userGroupStore
        //        .GetUserGroupCardAsync(CurrentTenant.Id, input.GroupId, CurrentUser.GetId());
        //    if (myGroupCard == null)
        //    {
        //        // 当前登录用户不再用户组
        //        throw new UserFriendlyException("");
        //    }
        //    if (!myGroupCard.IsAdmin)
        //    {
        //        // 当前登录用户没有加人权限
        //        throw new UserFriendlyException("");
        //    }
        //    await _userGroupStore
        //        .AddUserToGroupAsync(CurrentTenant.Id, input.UserId, input.GroupId, CurrentUser.GetId());
        //}

        //public virtual async Task GroupRemoveUserAsync(GroupRemoveUserDto input)
        //{
        //    var myGroupCard = await _userGroupStore
        //        .GetUserGroupCardAsync(CurrentTenant.Id, input.GroupId, CurrentUser.GetId());
        //    if (myGroupCard == null)
        //    {
        //        // 当前登录用户不再用户组
        //        throw new UserFriendlyException("");
        //    }
        //    if (!myGroupCard.IsAdmin)
        //    {
        //        // 当前登录用户没有踢人权限
        //        throw new UserFriendlyException("");
        //    }
        //    await _userGroupStore
        //        .RemoveUserFormGroupAsync(CurrentTenant.Id, input.UserId, input.GroupId);
        //}

        public virtual async Task<ChatMessageSendResultDto> SendMessageAsync(ChatMessage input)
        {
            // TODO：向其他租户发送消息?
            input.TenantId ??= CurrentTenant.Id;

            var messageId = await MessageSender.SendMessageAsync(input);

            return new ChatMessageSendResultDto(messageId);
        }
    }
}
