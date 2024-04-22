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
[Authorize(PermissionManagementPermissionNames.Definition.Default)]
[RemoteService(Name = PermissionManagementRemoteServiceConsts.RemoteServiceName)]
[Area(PermissionManagementRemoteServiceConsts.ModuleName)]
[Route("api/permission-management/definitions")]
public class PermissionDefinitionController : PermissionManagementControllerBase, IPermissionDefinitionAppService
{
    private readonly IPermissionDefinitionAppService _service;

    public PermissionDefinitionController(IPermissionDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(PermissionManagementPermissionNames.Definition.Create)]
    public virtual Task<PermissionDefinitionDto> CreateAsync(PermissionDefinitionCreateDto input)
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
    [Authorize(PermissionManagementPermissionNames.Definition.Delete)]
    public virtual Task<PermissionDefinitionDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<ListResultDto<PermissionDefinitionDto>> GetListAsync(PermissionDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(PermissionManagementPermissionNames.Definition.Update)]
    public virtual Task<PermissionDefinitionDto> UpdateAsync(string name, PermissionDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
