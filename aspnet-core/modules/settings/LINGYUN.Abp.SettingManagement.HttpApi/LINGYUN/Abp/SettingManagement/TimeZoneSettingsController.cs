using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.SettingManagement;

[RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
[Area(AbpSettingManagementRemoteServiceConsts.ModuleName)]
[Route("api/setting-management/timezone")]
[Authorize]
public class TimeZoneSettingsController : AbpControllerBase, ITimeZoneSettingsAppService
{
    private readonly ITimeZoneSettingsAppService _service;

    public TimeZoneSettingsController(ITimeZoneSettingsAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Volo.Abp.SettingManagement.SettingManagementPermissions.TimeZone)]
    public virtual Task<string> GetAsync()
    {
        return _service.GetAsync();
    }

    [HttpGet]
    [Route("my-timezone")]
    public virtual Task<string> GetMyTimezoneAsync()
    {
        return _service.GetMyTimezoneAsync();
    }

    [HttpGet]
    [Route("timezones")]
    public virtual Task<List<NameValue>> GetTimezonesAsync()
    {
        return _service.GetTimezonesAsync();
    }

    [HttpPost]
    [Route("my-timezone")]
    public virtual Task SetMyTimezoneAsync(string timezone)
    {
        return _service.SetMyTimezoneAsync(timezone);
    }

    [HttpPost]
    [Authorize(Volo.Abp.SettingManagement.SettingManagementPermissions.TimeZone)]
    public virtual Task UpdateAsync(string timezone)
    {
        return _service.UpdateAsync(timezone);
    }
}
