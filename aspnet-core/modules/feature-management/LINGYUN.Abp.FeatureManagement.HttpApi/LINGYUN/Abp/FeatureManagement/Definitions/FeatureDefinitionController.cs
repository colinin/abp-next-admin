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
[Authorize(FeatureManagementPermissionNames.Definition.Default)]
[RemoteService(Name = FeatureManagementRemoteServiceConsts.RemoteServiceName)]
[Area(FeatureManagementRemoteServiceConsts.ModuleName)]
[Route("api/feature-management/definitions")]
public class FeatureDefinitionController : FeatureManagementControllerBase, IFeatureDefinitionAppService
{
    private readonly IFeatureDefinitionAppService _service;

    public FeatureDefinitionController(IFeatureDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(FeatureManagementPermissionNames.Definition.Create)]
    public virtual Task<FeatureDefinitionDto> CreateAsync(FeatureDefinitionCreateDto input)
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
    [Authorize(FeatureManagementPermissionNames.Definition.Delete)]
    public virtual Task<FeatureDefinitionDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<ListResultDto<FeatureDefinitionDto>> GetListAsync(FeatureDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(FeatureManagementPermissionNames.Definition.Update)]
    public virtual Task<FeatureDefinitionDto> UpdateAsync(string name, FeatureDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
