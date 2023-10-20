using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.SettingManagement;

[RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
[Area("SettingManagement")]
[Route("api/setting-management/settings/definitions")]
[Authorize(SettingManagementPermissions.Definition.Default)]
public class SettingDefinitionController : AbpControllerBase, ISettingDefinitionAppService
{
    private readonly ISettingDefinitionAppService _service;

    public SettingDefinitionController(ISettingDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(SettingManagementPermissions.Definition.Create)]
    public virtual Task<SettingDefinitionDto> CreateAsync(SettingDefinitionCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{name}")]
    [Authorize(SettingManagementPermissions.Definition.DeleteOrRestore)]
    public virtual Task DeleteOrRestoreAsync(string name)
    {
        return _service.DeleteOrRestoreAsync(name);
    }

    [HttpGet]
    [Route("{name}")]
    public virtual Task<SettingDefinitionDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<ListResultDto<SettingDefinitionDto>> GetListAsync(SettingDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(SettingManagementPermissions.Definition.Update)]
    public virtual Task<SettingDefinitionDto> UpdateAsync(string name, SettingDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
