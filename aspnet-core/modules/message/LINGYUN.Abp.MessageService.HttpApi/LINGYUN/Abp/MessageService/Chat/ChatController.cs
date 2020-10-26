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
        public virtual async Task ApplyJoinGroupAsync(UserJoinGroupDto input)
        {
            await _chatAppService.ApplyJoinGroupAsync(input);
        }

        [HttpGet]
        [Route("group/messages")]
        public virtual async Task<PagedResultDto<ChatMessage>> GetGroupMessageAsync(GroupMessageGetByPagedDto input)
        {
            return await _chatAppService.GetGroupMessageAsync(input);
        }

        [HttpGet]
        [Route("groups/users")]
        public virtual async Task<PagedResultDto<GroupUserCard>> GetGroupUsersAsync(GroupUserGetByPagedDto input)
        {
            return await _chatAppService.GetGroupUsersAsync(input);
        }

        [HttpGet]
        [Route("my-messages")]
        public virtual async Task<PagedResultDto<ChatMessage>> GetMyChatMessageAsync(UserMessageGetByPagedDto input)
        {
            return await _chatAppService.GetMyChatMessageAsync(input);
        }

        [HttpGet]
        [Route("my-last-messages")]
        public virtual async Task<ListResultDto<LastChatMessage>> GetMyLastChatMessageAsync(GetUserLastMessageDto input)
        {
            return await _chatAppService.GetMyLastChatMessageAsync(input);
        }

        [HttpGet]
        [Route("groups/me")]
        public virtual async Task<ListResultDto<Group>> GetMyGroupsAsync()
        {
            return await _chatAppService.GetMyGroupsAsync();
        }

        [HttpPost]
        [Route("groups/users/accept")]
        public virtual async Task GroupAcceptUserAsync(GroupAcceptUserDto input)
        {
            await _chatAppService.GroupAcceptUserAsync(input);
        }

        [HttpDelete]
        [Route("groups/users/remove")]
        public virtual async Task GroupRemoveUserAsync(GroupRemoveUserDto input)
        {
            await _chatAppService.GroupRemoveUserAsync(input);
        }

        [HttpGet]
        [Route("send-message")]
        public virtual async Task<ChatMessageSendResultDto> SendMessageAsync(ChatMessage input)
        {
            return await _chatAppService.SendMessageAsync(input);
        }
    }
}
