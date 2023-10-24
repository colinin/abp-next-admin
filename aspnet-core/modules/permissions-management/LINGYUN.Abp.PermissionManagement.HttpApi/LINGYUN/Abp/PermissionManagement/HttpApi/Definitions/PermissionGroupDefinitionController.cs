using LINGYUN.Abp.PermissionManagement.Definitions;
using LINGYUN.Abp.PermissionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.PermissionManagement.HttpApi.Definitions;

[Controller]
[Authorize(PermissionManagementPermissionNames.GroupDefinition.Default)]
[RemoteService(Name = PermissionManagementRemoteServiceConsts.RemoteServiceName)]
[Area(PermissionManagementRemoteServiceConsts.ModuleName)]
[Route("api/permission-management/definitions/groups")]
public class PermissionGroupDefinitionController : PermissionManagementControllerBase, IPermissionGroupDefinitionAppService
{
    private readonly IPermissionGroupDefinitionAppService _service;

    public PermissionGroupDefinitionController(IPermissionGroupDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Create)]
    public virtual Task<PermissionGroupDefinitionDto> CreateAsync(PermissionGroupDefinitionCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{name}")]
    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Delete)]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }

    [HttpGet]
    [Route("{name}")]
    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Delete)]
    public virtual Task<PermissionGroupDefinitionDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<ListResultDto<PermissionGroupDefinitionDto>> GetListAsync(PermissionGroupDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Update)]
    public virtual Task<PermissionGroupDefinitionDto> UpdateAsync(string name, PermissionGroupDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
