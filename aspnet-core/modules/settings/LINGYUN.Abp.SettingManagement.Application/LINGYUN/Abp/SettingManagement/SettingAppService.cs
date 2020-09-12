using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace LINGYUN.Abp.SettingManagement
{
    [Authorize(AbpSettingManagementPermissions.Settings.Default)]
    public class SettingAppService : ApplicationService, ISettingAppService
    {
        protected ISettingManager SettingManager { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        protected IDistributedCache<SettingCacheItem> Cache { get; }
        public SettingAppService(
            ISettingManager settingManager,
            IDistributedCache<SettingCacheItem> cache,
            ISettingDefinitionManager settingDefinitionManager)
        {
            Cache = cache;
            SettingManager = settingManager;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationResource = typeof(AbpSettingManagementResource);
        }

        [Authorize(AbpSettingManagementPermissions.Settings.Manager)]
        public virtual async Task SetGlobalAsync(UpdateSettingsDto input)
        {
            foreach (var setting in input.Settings)
            {
                await SettingManager.SetGlobalAsync(setting.Name, setting.Value);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(AbpSettingManagementPermissions.Settings.Manager)]
        public virtual async Task SetCurrentTenantAsync(UpdateSettingsDto input)
        {
            if (CurrentTenant.IsAvailable)
            {
                foreach (var setting in input.Settings)
                {
                    await SettingManager.SetForTenantAsync(CurrentTenant.GetId(), setting.Name, setting.Value);
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        [Authorize(AbpSettingManagementPermissions.Settings.Manager)]
        public virtual async Task SetForUserAsync([Required] Guid userId, UpdateSettingsDto input)
        {
            foreach (var setting in input.Settings)
            {
                await SettingManager.SetForUserAsync(userId, setting.Name, setting.Value);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize]
        public virtual async Task SetCurrentUserAsync(UpdateSettingsDto input)
        {
            foreach (var setting in input.Settings)
            {
                await SettingManager.SetForUserAsync(CurrentUser.GetId(), setting.Name, setting.Value);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AllowAnonymous]
        public virtual async Task<ListResultDto<SettingDto>> GetAllGlobalAsync()
        {
            // return GetAllSetting(await SettingManager.GetAllGlobalAsync());

            return await GetAllSettingAsync(GlobalSettingValueProvider.ProviderName, null);
        }

        public virtual async Task<ListResultDto<SettingDto>> GetAllForCurrentTenantAsync()
        {
            if (CurrentTenant.IsAvailable)
            {
                // return GetAllSetting(await SettingManager.GetAllForTenantAsync(CurrentTenant.GetId(), false));

                return await GetAllSettingAsync(TenantSettingValueProvider.ProviderName, CurrentTenant.GetId().ToString());
            }
            return new ListResultDto<SettingDto>();

        }

        public virtual async Task<ListResultDto<SettingDto>> GetAllForUserAsync([Required] Guid userId)
        {
            // return GetAllSetting(await SettingManager.GetAllForUserAsync(userId));

            return await GetAllSettingAsync(UserSettingValueProvider.ProviderName, userId.ToString());
        }

        [Authorize]
        public virtual async Task<ListResultDto<SettingDto>> GetAllForCurrentUserAsync()
        {
            // return GetAllSetting(await SettingManager.GetAllForUserAsync(CurrentUser.GetId()));

            return await GetAllSettingAsync(UserSettingValueProvider.ProviderName, CurrentUser.GetId().ToString());
        }

        protected virtual ListResultDto<SettingDto> GetAllSetting(List<SettingValue> settings)
        {
            var settingsDto = new List<SettingDto>();
            foreach (var setting in settings)
            {
                var settingDefinition = SettingDefinitionManager.Get(setting.Name);
                var settingInfo = new SettingDto
                {
                    Name = setting.Name,
                    Value = setting.Value ?? settingDefinition.DefaultValue,
                    DefaultValue = settingDefinition.DefaultValue,
                    Description = settingDefinition.Description.Localize(StringLocalizerFactory),
                    DisplayName = settingDefinition.DisplayName.Localize(StringLocalizerFactory)
                };
                settingsDto.Add(settingInfo);
            }

            return new ListResultDto<SettingDto>(settingsDto);
        }

        protected virtual async Task<ListResultDto<SettingDto>> GetAllSettingAsync(string providerName, string providerKey)
        {
            var settingsDto = new List<SettingDto>();

            var settings = await SettingManager.GetAllAsync(providerName, providerKey);
            foreach (var setting in settings)
            {
                var settingDefinition = SettingDefinitionManager.Get(setting.Name);
                if (settingDefinition.Providers.Count > 0 && 
                    !settingDefinition.Providers.Any(p => p.Equals(providerName)))
                {
                    continue;
                }
                    var settingInfo = new SettingDto
                {
                    Name = setting.Name,
                    Value = setting.Value ?? settingDefinition.DefaultValue,
                    DefaultValue = settingDefinition.DefaultValue,
                    Description = settingDefinition.Description.Localize(StringLocalizerFactory),
                    DisplayName = settingDefinition.DisplayName.Localize(StringLocalizerFactory)
                };
                settingsDto.Add(settingInfo);
            }

            return new ListResultDto<SettingDto>(settingsDto);
        }
    }
}
