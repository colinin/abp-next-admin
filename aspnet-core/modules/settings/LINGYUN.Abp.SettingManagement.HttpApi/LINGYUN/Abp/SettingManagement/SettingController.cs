using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.SettingManagement;

[RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
[Area("SettingManagement")]
[Route("api/setting-management/settings")]
public class SettingController : AbpController, ISettingAppService, ISettingTestAppService
{
    private readonly ISettingAppService _settingAppService;
    private readonly ISettingTestAppService _settingTestAppService;
    public SettingController(
        ISettingAppService settingAppService, 
        ISettingTestAppService settingTestAppService)
    {
        _settingAppService = settingAppService;
        _settingTestAppService = settingTestAppService;
    }

    [HttpPut]
    [Route("change-current-tenant")]
    public async virtual Task SetCurrentTenantAsync(UpdateSettingsDto input)
    {
        await _settingAppService.SetCurrentTenantAsync(input);
    }

    [HttpPut]
    [Route("change-global")]
    public async virtual Task SetGlobalAsync(UpdateSettingsDto input)
    {
        await _settingAppService.SetGlobalAsync(input);
    }

    [HttpGet]
    [Route("by-global")]
    public async virtual Task<SettingGroupResult> GetAllForGlobalAsync()
    {
        return await _settingAppService.GetAllForGlobalAsync();
    }

    [HttpGet]
    [Route("by-current-tenant")]
    public async virtual Task<SettingGroupResult> GetAllForCurrentTenantAsync()
    {
        return await _settingAppService.GetAllForCurrentTenantAsync();
    }

    [HttpPost]
    [Authorize]
    [Route("send-test-email")]
    public async virtual Task SendTestEmailAsync(SendTestEmailInput input)
    {
        await _settingTestAppService.SendTestEmailAsync(input);
    }
}
