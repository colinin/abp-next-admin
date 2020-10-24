using LINGYUN.Abp.IM.Group;
using LINGYUN.Abp.IM.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Chat
{
    [RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
    [Route("api/im/chat")]
    public class ChatController : AbpController, IChatAppService
    {
        private readonly IChatAppService _chatAppService;

        public ChatController(IChatAppService chatAppService)
        {
            _chatAppService = chatAppService;
        }

        [HttpPost]
        [Route("groups/join")]
        public virtual async Task ApplyJoinGroupAsync(UserJoinGroupDto userJoinGroup)
        {
            await _chatAppService.ApplyJoinGroupAsync(userJoinGroup);
        }

        [HttpGet]
        [Route("messages/group")]
        public virtual async Task<PagedResultDto<ChatMessage>> GetGroupMessageAsync(GroupMessageGetByPagedDto groupMessageGetByPaged)
        {
            return await _chatAppService.GetGroupMessageAsync(groupMessageGetByPaged);
        }

        [HttpGet]
        [Route("groups/users")]
        public virtual async Task<PagedResultDto<GroupUserCard>> GetGroupUsersAsync(GroupUserGetByPagedDto groupUserGetByPaged)
        {
            return await _chatAppService.GetGroupUsersAsync(groupUserGetByPaged);
        }

        [HttpGet]
        [Route("messages/me")]
        public virtual async Task<PagedResultDto<ChatMessage>> GetMyChatMessageAsync(UserMessageGetByPagedDto userMessageGetByPaged)
        {
            return await _chatAppService.GetMyChatMessageAsync(userMessageGetByPaged);
        }

        [HttpGet]
        [Route("groups/me")]
        public virtual async Task<ListResultDto<Group>> GetMyGroupsAsync()
        {
            return await _chatAppService.GetMyGroupsAsync();
        }

        [HttpPost]
        [Route("groups/users/accept")]
        public virtual async Task GroupAcceptUserAsync(GroupAcceptUserDto groupAcceptUser)
        {
            await _chatAppService.GroupAcceptUserAsync(groupAcceptUser);
        }

        [HttpDelete]
        [Route("groups/users/remove")]
        public virtual async Task GroupRemoveUserAsync(GroupRemoveUserDto groupRemoveUser)
        {
            await _chatAppService.GroupRemoveUserAsync(groupRemoveUser);
        }

        [HttpGet]
        [Route("messages/send")]
        public virtual async Task<ChatMessageSendResultDto> SendMessageAsync(ChatMessage chatMessage)
        {
            return await _chatAppService.SendMessageAsync(chatMessage);
        }
    }
}
