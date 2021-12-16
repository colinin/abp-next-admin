using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.SettingManagement
{
    [RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("settingManagement")]
    [Route("api/setting-management/settings")]
    public class UserSettingController : AbpController, IUserSettingAppService
    {
        private readonly IUserSettingAppService _service;

        public UserSettingController(
            IUserSettingAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("by-current-user")]
        public virtual async Task<SettingGroupResult> GetAllForCurrentUserAsync()
        {
            return await _service.GetAllForCurrentUserAsync();
        }

        [HttpPut]
        [Route("change-current-user")]
        public virtual async Task SetCurrentUserAsync(UpdateSettingsDto input)
        {
            await _service.SetCurrentUserAsync(input);
        }
    }
}
