using LINGYUN.Abp.FeatureManagement.Definitions;
using LINGYUN.Abp.FeatureManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.FeatureManagement;

namespace LINGYUN.Abp.FeatureManagement.HttpApi.Definitions;

[Controller]
[Authorize(FeatureManagementPermissionNames.GroupDefinition.Default)]
[RemoteService(Name = FeatureManagementRemoteServiceConsts.RemoteServiceName)]
[Area(FeatureManagementRemoteServiceConsts.ModuleName)]
[Route("api/feature-management/definitions/groups")]
public class FeatureGroupDefinitionController : FeatureManagementControllerBase, IFeatureGroupDefinitionAppService
{
    private readonly IFeatureGroupDefinitionAppService _service;

    public FeatureGroupDefinitionController(IFeatureGroupDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Create)]
    public virtual Task<FeatureGroupDefinitionDto> CreateAsync(FeatureGroupDefinitionCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{name}")]
    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Delete)]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }

    [HttpGet]
    [Route("{name}")]
    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Delete)]
    public virtual Task<FeatureGroupDefinitionDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<FeatureGroupDefinitionDto>> GetListAsync(FeatureGroupDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Update)]
    public virtual Task<FeatureGroupDefinitionDto> UpdateAsync(string name, FeatureGroupDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
