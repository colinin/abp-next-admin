using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WorkflowManagement.SettingManagement
{
    [RemoteService(Name = WorkflowManagementRemoteServiceConsts.RemoteServiceName)]
    [ApiVersion("2.0")]
    [Area("WorkflowManagement")]
    [Route("api/WorkflowManagement/settings")]
    public class SettingController : AbpController, ISettingAppService
    {
        private readonly ISettingAppService _settingAppService;
        public SettingController(ISettingAppService settingAppService)
        {
            _settingAppService = settingAppService;
        }

        [HttpPut]
        public virtual async Task SetCurrentTenantAsync(UpdateSettingsDto input)
        {
            await _settingAppService.SetCurrentTenantAsync(input);
        }

        [HttpGet]
        public virtual async Task<ListResultDto<SettingGroupDto>> GetAllForCurrentTenantAsync()
        {
            return await _settingAppService.GetAllForCurrentTenantAsync();
        }

        [HttpPost]
        [Route("by-global")]
        public virtual async Task SetGlobalAsync(UpdateSettingsDto input)
        {
            await _settingAppService.SetGlobalAsync(input);
        }

        [HttpGet]
        [Route("by-global")]
        public virtual async Task<ListResultDto<SettingGroupDto>> GetAllForGlobalAsync()
        {
            return await _settingAppService.GetAllForGlobalAsync();
        }
    }
}
