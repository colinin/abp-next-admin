using LINGYUN.Abp.AIManagement.Tools.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AIManagement.Tools;

[Controller]
[RemoteService(Name = AIManagementRemoteServiceConsts.RemoteServiceName)]
[Area(AIManagementRemoteServiceConsts.ModuleName)]
[Route($"api/{AIManagementRemoteServiceConsts.ModuleName}/tools")]
public class AIToolDefinitionController : AbpControllerBase, IAIToolDefinitionAppService
{
    private readonly IAIToolDefinitionAppService _service;
    public AIToolDefinitionController(IAIToolDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    public virtual Task<AIToolDefinitionRecordDto> CreateAsync(AIToolDefinitionRecordCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("{id}")]
    public virtual Task<AIToolDefinitionRecordDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet("available-providers")]
    public virtual Task<ListResultDto<AIToolProviderDto>> GetAvailableProvidersAsync()
    {
        return _service.GetAvailableProvidersAsync();
    }

    [HttpGet]
    public virtual Task<PagedResultDto<AIToolDefinitionRecordDto>> GetListAsync(AIToolDefinitionRecordGetPagedListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<AIToolDefinitionRecordDto> UpdateAsync(Guid id, AIToolDefinitionRecordUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }
}
