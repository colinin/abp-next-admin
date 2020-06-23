using LINGYUN.Abp.IM.Group;
using LINGYUN.Abp.IM.Messages;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class ChatAppService : ApplicationService, IChatAppService
    {
        private IMessageSender _messageSender;
        protected IMessageSender MessageSender => LazyGetRequiredService(ref _messageSender);

        private readonly IUserGroupStore _userGroupStore;
        private readonly IMessageStore _messageStore;

        public ChatAppService(
            IMessageStore messageStore,
            IUserGroupStore userGroupStore)
        {
            _messageStore = messageStore;
            _userGroupStore = userGroupStore;
        }

        public virtual async Task<PagedResultDto<ChatMessage>> GetMyChatMessageAsync(UserMessageGetByPagedDto userMessageGetByPaged)
        {
            var chatMessageCount = await _messageStore
                .GetChatMessageCountAsync(CurrentTenant.Id, CurrentUser.GetId(), userMessageGetByPaged.ReceiveUserId,
                    userMessageGetByPaged.Filter, userMessageGetByPaged.MessageType);

            var chatMessages = await _messageStore
                .GetChatMessageAsync(CurrentTenant.Id, CurrentUser.GetId(), userMessageGetByPaged.ReceiveUserId, 
                    userMessageGetByPaged.Filter, userMessageGetByPaged.Sorting, userMessageGetByPaged.MessageType,
                    userMessageGetByPaged.SkipCount, userMessageGetByPaged.MaxResultCount);

            return new PagedResultDto<ChatMessage>(chatMessageCount, chatMessages);
        }

        public virtual async Task<PagedResultDto<ChatMessage>> GetGroupMessageAsync(GroupMessageGetByPagedDto groupMessageGetByPaged)
        {
            var groupMessageCount = await _messageStore
                .GetGroupMessageCountAsync(CurrentTenant.Id, groupMessageGetByPaged.GroupId,
                    groupMessageGetByPaged.Filter, groupMessageGetByPaged.MessageType);

            var groupMessages = await _messageStore
                .GetGroupMessageAsync(CurrentTenant.Id, groupMessageGetByPaged.GroupId,
                    groupMessageGetByPaged.Filter, groupMessageGetByPaged.Sorting, groupMessageGetByPaged.MessageType,
                    groupMessageGetByPaged.SkipCount, groupMessageGetByPaged.MaxResultCount);

            return new PagedResultDto<ChatMessage>(groupMessageCount, groupMessages);
        }

        public virtual async Task<PagedResultDto<UserGroup>> GetGroupUsersAsync(GroupUserGetByPagedDto groupUserGetByPaged)
        {
            var groupUserCount = await _userGroupStore.GetGroupUsersCountAsync(CurrentTenant.Id, 
                groupUserGetByPaged.GroupId, groupUserGetByPaged.Filter);

            var groupUsers = await _userGroupStore.GetGroupUsersAsync(CurrentTenant.Id,
                groupUserGetByPaged.GroupId, groupUserGetByPaged.Filter, groupUserGetByPaged.Sorting,
                groupUserGetByPaged.SkipCount, groupUserGetByPaged.MaxResultCount);

            return new PagedResultDto<UserGroup>(groupUserCount, groupUsers);
        }

        public virtual async Task<ListResultDto<Group>> GetMyGroupsAsync()
        {
            var myGroups = await _userGroupStore.GetUserGroupsAsync(CurrentTenant.Id, CurrentUser.GetId());

            return new ListResultDto<Group>(myGroups.ToImmutableList());
        }

        public virtual async Task GroupAcceptUserAsync(GroupAcceptUserDto groupAcceptUser)
        {
            var myGroupCard = await _userGroupStore
                .GetUserGroupCardAsync(CurrentTenant.Id, groupAcceptUser.GroupId, CurrentUser.GetId());
            if (myGroupCard == null)
            {
                // 当前登录用户不再用户组
                throw new UserFriendlyException("");
            }
            if (!myGroupCard.IsGrant(nameof(ChatGroupAdmin.AllowAddPeople)))
            {
                // 当前登录用户没有加人权限
                throw new UserFriendlyException("");
            }
            await _userGroupStore
                .AddUserToGroupAsync(CurrentTenant.Id, groupAcceptUser.UserId, groupAcceptUser.GroupId, CurrentUser.GetId());
        }

        public virtual async Task GroupRemoveUserAsync(GroupRemoveUserDto groupRemoveUser)
        {
            var myGroupCard = await _userGroupStore
                .GetUserGroupCardAsync(CurrentTenant.Id, groupRemoveUser.GroupId, CurrentUser.GetId());
            if (myGroupCard == null)
            {
                // 当前登录用户不再用户组
                throw new UserFriendlyException("");
            }
            if (!myGroupCard.IsGrant(nameof(ChatGroupAdmin.AllowKickPeople)))
            {
                // 当前登录用户没有踢人权限
                throw new UserFriendlyException("");
            }
            await _userGroupStore
                .RemoveUserFormGroupAsync(CurrentTenant.Id, groupRemoveUser.UserId, groupRemoveUser.GroupId);
        }

        public virtual async Task<ChatMessageSendResultDto> SendMessageAsync(ChatMessage chatMessage)
        {
            // TODO：向其他租户发送消息?
            chatMessage.TenantId = chatMessage.TenantId ?? CurrentTenant.Id;

            await MessageSender.SendMessageAsync(chatMessage);

            return new ChatMessageSendResultDto(chatMessage.MessageId);
        }

        public virtual Task ApplyJoinGroupAsync(UserJoinGroupDto userJoinGroup)
        {
            // TOTO 发送通知? 
            return Task.CompletedTask;
        }
    }
}
