using LINGYUN.Abp.IM.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Chat;

[RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
[Route("api/im/chat")]
public class ChatController : AbpControllerBase, IChatAppService
{
    private readonly IChatAppService _chatAppService;

    public ChatController(IChatAppService chatAppService)
    {
        _chatAppService = chatAppService;
    }

    [HttpGet]
    [Route("group/messages")]
    public async virtual Task<PagedResultDto<ChatMessage>> GetMyGroupMessageAsync(GroupMessageGetByPagedDto input)
    {
        return await _chatAppService.GetMyGroupMessageAsync(input);
    }

    [HttpGet]
    [Route("my-messages")]
    public async virtual Task<PagedResultDto<ChatMessage>> GetMyChatMessageAsync(UserMessageGetByPagedDto input)
    {
        return await _chatAppService.GetMyChatMessageAsync(input);
    }

    [HttpGet]
    [Route("my-last-messages")]
    public async virtual Task<ListResultDto<LastChatMessage>> GetMyLastChatMessageAsync(GetUserLastMessageDto input)
    {
        return await _chatAppService.GetMyLastChatMessageAsync(input);
    }

    [HttpPost]
    [Route("send-message")]
    public async virtual Task<ChatMessageSendResultDto> SendMessageAsync(ChatMessage input)
    {
        return await _chatAppService.SendMessageAsync(input);
    }
}
