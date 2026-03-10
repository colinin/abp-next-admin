using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.SettingManagement;

namespace LINGYUN.Abp.SettingManagement;

[RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
[Area(AbpSettingManagementRemoteServiceConsts.ModuleName)]
[Route("api/setting-management/timezone")]
[Authorize(Volo.Abp.SettingManagement.SettingManagementPermissions.TimeZone)]
public class TimeZoneSettingsController : AbpControllerBase, ITimeZoneSettingsAppService
{
    private readonly ITimeZoneSettingsAppService _service;

    public TimeZoneSettingsController(ITimeZoneSettingsAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<string> GetAsync()
    {
        return _service.GetAsync();
    }

    [HttpGet]
    [Route("timezones")]
    public virtual Task<List<NameValue>> GetTimezonesAsync()
    {
        return _service.GetTimezonesAsync();
    }

    [HttpPost]
    public virtual Task UpdateAsync(string timezone)
    {
        return _service.UpdateAsync(timezone);
    }
}
