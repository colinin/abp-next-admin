using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.SettingManagement
{
    [RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("settingManagement")]
    [Route("api/setting-management/settings")]
    public class SettingController : AbpController, ISettingAppService
    {
        private readonly ISettingAppService _settingAppService;
        public SettingController(ISettingAppService settingAppService)
        {
            _settingAppService = settingAppService;
        }

        [HttpGet]
        [Route("by-current-user")]
        public virtual async Task<ListResultDto<SettingDto>> GetAllForCurrentUserAsync()
        {
            return await _settingAppService.GetAllForCurrentUserAsync();
        }

        [HttpGet]
        [Route("by-current-tenant")]
        public virtual async Task<ListResultDto<SettingDto>> GetAllForCurrentTenantAsync()
        {
            return await _settingAppService.GetAllForCurrentTenantAsync();
        }

        [HttpGet]
        [Route("by-user/{userId}")]
        public virtual async Task<ListResultDto<SettingDto>> GetAllForUserAsync([Required] Guid userId)
        {
            return await _settingAppService.GetAllForUserAsync(userId);
        }

        [HttpGet]
        [Route("by-global")]
        public virtual async Task<ListResultDto<SettingDto>> GetAllGlobalAsync()
        {
            return await _settingAppService.GetAllGlobalAsync();
        }

        [HttpPut]
        [Route("by-current-user")]
        public virtual async Task SetCurrentUserAsync(UpdateSettingsDto input)
        {
            await _settingAppService.SetCurrentUserAsync(input);
        }

        [HttpPut]
        [Route("by-current-tenant")]
        public virtual async Task SetCurrentTenantAsync(UpdateSettingsDto input)
        {
            await _settingAppService.SetCurrentTenantAsync(input);
        }

        [HttpPut]
        [Route("by-user/{userId}")]
        public virtual async Task SetForUserAsync([Required] Guid userId, UpdateSettingsDto input)
        {
            await _settingAppService.SetForUserAsync(userId, input);
        }

        [HttpPut]
        [Route("by-global")]
        public virtual async Task SetGlobalAsync(UpdateSettingsDto input)
        {
            await _settingAppService.SetGlobalAsync(input);
        }
    }
}
