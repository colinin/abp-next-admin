using LINGYUN.Abp.AIManagement.Chats.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AIManagement.Chats;

[Controller]
[RemoteService(Name = AIManagementRemoteServiceConsts.RemoteServiceName)]
[Area(AIManagementRemoteServiceConsts.ModuleName)]
[Route($"api/{AIManagementRemoteServiceConsts.ModuleName}/chats")]
public class ChatController : AbpControllerBase, IChatAppService
{
    private readonly IChatAppService _service;
    public ChatController(IChatAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [ServiceFilter<SseAsyncEnumerableResultFilter>]
    public async virtual IAsyncEnumerable<string> SendMessageAsync(SendTextChatMessageDto input)
    {
        await foreach (var content in _service.SendMessageAsync(input))
        {
            yield return content;
        }
    }
}
