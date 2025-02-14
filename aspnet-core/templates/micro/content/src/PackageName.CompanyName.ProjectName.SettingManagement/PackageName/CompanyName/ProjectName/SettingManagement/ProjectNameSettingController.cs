using Asp.Versioning;
using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackageName.CompanyName.ProjectName.Permissions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace PackageName.CompanyName.ProjectName.SettingManagement;

[RemoteService(Name = ProjectNameRemoteServiceConsts.RemoteServiceName)]
[ApiVersion("2.0")]
[Area(ProjectNameRemoteServiceConsts.ModuleName)]
[Route("api/ProjectName/settings")]
public class ProjectNameSettingController : AbpController, IProjectNameSettingAppService
{
    private readonly IProjectNameSettingAppService _settingAppService;
    public ProjectNameSettingController(IProjectNameSettingAppService settingAppService)
    {
        _settingAppService = settingAppService;
    }

    [Authorize(ProjectNamePermissions.ManageSettings)]
    [HttpPut]
    [Route("by-current-tenant")]
    public virtual async Task SetCurrentTenantAsync(UpdateSettingsDto input)
    {
        await _settingAppService.SetCurrentTenantAsync(input);
    }

    [HttpGet]
    [Route("by-current-tenant")]
    public virtual async Task<SettingGroupResult> GetAllForCurrentTenantAsync()
    {
        return await _settingAppService.GetAllForCurrentTenantAsync();
    }

    [Authorize]
    [HttpPut]
    [Route("by-current-user")]
    public virtual async Task SetCurrentUserAsync(UpdateSettingsDto input)
    {
        await _settingAppService.SetCurrentTenantAsync(input);
    }

    [Authorize]
    [HttpGet]
    [Route("by-current-user")]
    public virtual async Task<SettingGroupResult> GetAllForCurrentUserAsync()
    {
        return await _settingAppService.GetAllForCurrentTenantAsync();
    }

    [Authorize(ProjectNamePermissions.ManageSettings)]
    [HttpPut]
    [Route("by-global")]
    public virtual async Task SetGlobalAsync(UpdateSettingsDto input)
    {
        await _settingAppService.SetGlobalAsync(input);
    }

    [HttpGet]
    [Route("by-global")]
    public virtual async Task<SettingGroupResult> GetAllForGlobalAsync()
    {
        return await _settingAppService.GetAllForGlobalAsync();
    }
}
