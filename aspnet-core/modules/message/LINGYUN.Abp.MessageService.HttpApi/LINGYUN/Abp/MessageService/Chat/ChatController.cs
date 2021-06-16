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

        [HttpGet]
        [Route("group/messages")]
        public virtual async Task<PagedResultDto<ChatMessage>> GetMyGroupMessageAsync(GroupMessageGetByPagedDto input)
        {
            return await _chatAppService.GetMyGroupMessageAsync(input);
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

        [HttpPost]
        [Route("send-message")]
        public virtual async Task<ChatMessageSendResultDto> SendMessageAsync(ChatMessage input)
        {
            return await _chatAppService.SendMessageAsync(input);
        }
    }
}
