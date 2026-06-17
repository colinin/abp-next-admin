using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.SettingManagement;

[RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
[ApiVersion("2")]
[Area("SettingManagement")]
[Route("api/v{version}/setting-management/my-settings")]
public class UserSettingV2Controller(IUserSettingV2AppService _service) : AbpControllerBase, IUserSettingV2AppService
{
    [HttpGet]
    public virtual Task<SettingGroupResult> GetAsync()
    {
        return _service.GetAsync();
    }

    [HttpPut]
    public virtual Task SetAsync(UpdateSettingsDto input)
    {
        return _service.SetAsync(input);
    }
}
