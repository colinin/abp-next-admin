using JetBrains.Annotations;
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
    [Area("settings")]
    [Route("api/settings")]
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
        [Route("by-tenant")]
        public virtual async Task<ListResultDto<SettingDto>> GetAllForTenantAsync()
        {
            return await _settingAppService.GetAllForTenantAsync();
        }

        [HttpGet]
        [Route("by-user")]
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

        [HttpGet]
        public virtual async Task<ListResultDto<SettingDto>> GetAsync([NotNull] string providerName, [NotNull] string providerKey)
        {
            return await _settingAppService.GetAsync(providerName, providerKey);
        }

        [HttpPut]
        public virtual async Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdateSettingsDto input)
        {
            await _settingAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}
