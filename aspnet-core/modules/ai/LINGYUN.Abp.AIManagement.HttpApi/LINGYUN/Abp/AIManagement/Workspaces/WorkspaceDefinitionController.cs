using LINGYUN.Abp.AIManagement.Workspaces.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AIManagement.Workspaces;

[Controller]
[RemoteService(Name = AIManagementRemoteServiceConsts.RemoteServiceName)]
[Area(AIManagementRemoteServiceConsts.ModuleName)]
[Route($"api/{AIManagementRemoteServiceConsts.ModuleName}/workspaces")]
public class WorkspaceDefinitionController : AbpControllerBase, IWorkspaceDefinitionAppService
{
    private readonly IWorkspaceDefinitionAppService _service;
    public WorkspaceDefinitionController(IWorkspaceDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    public virtual Task<WorkspaceDefinitionRecordDto> CreateAsync(WorkspaceDefinitionRecordCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("{id}")]
    public virtual Task<WorkspaceDefinitionRecordDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<WorkspaceDefinitionRecordDto>> GetListAsync(WorkspaceDefinitionRecordGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<WorkspaceDefinitionRecordDto> UpdateAsync(Guid id, WorkspaceDefinitionRecordUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }
}
