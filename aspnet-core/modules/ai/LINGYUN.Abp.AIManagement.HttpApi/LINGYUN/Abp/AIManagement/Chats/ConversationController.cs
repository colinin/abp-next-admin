using LINGYUN.Abp.AIManagement.Chats.Dtos;
using LINGYUN.Abp.AIManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AIManagement.Chats;

[Controller]
[RemoteService(Name = AIManagementRemoteServiceConsts.RemoteServiceName)]
[Area(AIManagementRemoteServiceConsts.ModuleName)]
[Route($"api/{AIManagementRemoteServiceConsts.ModuleName}/chats/conversations")]
[Authorize(AIManagementPermissionNames.Conversation.Default)]
public class ConversationController : AbpControllerBase, IConversationAppService
{
    private readonly IConversationAppService _service;
    public ConversationController(IConversationAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(AIManagementPermissionNames.Conversation.Create)]
    public virtual Task<ConversationDto> CreateAsync(ConversationCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete("{id}")]
    [Authorize(AIManagementPermissionNames.Conversation.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("{id}")]
    public virtual Task<ConversationDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ConversationDto>> GetListAsync(ConversationGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut("{id}")]
    [Authorize(AIManagementPermissionNames.Conversation.Update)]
    public virtual Task<ConversationDto> UpdateAsync(Guid id, ConversationUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }
}
