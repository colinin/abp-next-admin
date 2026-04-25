using LINGYUN.Abp.AIManagement.Chats.Dtos;
using LINGYUN.Abp.AIManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AIManagement.Chats;

[Controller]
[RemoteService(Name = AIManagementRemoteServiceConsts.RemoteServiceName)]
[Area(AIManagementRemoteServiceConsts.ModuleName)]
[Route($"api/{AIManagementRemoteServiceConsts.ModuleName}/chats")]
[Authorize(AIManagementPermissionNames.Chat.Default)]
public class ChatController : AbpControllerBase, IChatAppService
{
    private readonly IChatAppService _service;
    public ChatController(IChatAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [ServiceFilter<SseAsyncEnumerableResultFilter>]
    [Authorize(AIManagementPermissionNames.Chat.SendMessage)]
    public async virtual IAsyncEnumerable<string> SendMessageAsync(SendTextChatMessageDto input)
    {
        await foreach (var content in _service.SendMessageAsync(input))
        {
            yield return content;
        }
    }

    [HttpGet]
    public virtual Task<PagedResultDto<TextChatMessageDto>> GetListAsync(TextChatMessageGetListInput input)
    {
        return _service.GetListAsync(input);
    }
}
