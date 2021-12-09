using LINGYUN.Abp.WorkflowManagement.Authorization;
using LINGYUN.Abp.WorkflowManagement.Localization;
using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WorkflowManagement.SettingManagement
{
    [Authorize(WorkflowManagementPermissions.ManageSettings)]
    public class SettingAppService : ApplicationService, ISettingAppService
    {
        protected ISettingManager SettingManager { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        public SettingAppService(
            ISettingManager settingManager,
            ISettingDefinitionManager settingDefinitionManager)
        {
            SettingManager = settingManager;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationResource = typeof(WorkflowManagementResource);
        }

        public virtual async Task<ListResultDto<SettingGroupDto>> GetAllForCurrentTenantAsync()
        {
            return await GetAllForProviderAsync(TenantSettingValueProvider.ProviderName, CurrentTenant.GetId().ToString());
        }

        public virtual async Task SetCurrentTenantAsync(UpdateSettingsDto input)
        {
            // 增加特性检查
            await CheckFeatureAsync();

            if (CurrentTenant.IsAvailable)
            {
                foreach (var setting in input.Settings)
                {
                    await SettingManager.SetForTenantAsync(CurrentTenant.GetId(), setting.Name, setting.Value);
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        protected virtual async Task CheckFeatureAsync()
        {
            await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
        }

        protected virtual async Task<ListResultDto<SettingGroupDto>> GetAllForProviderAsync(string providerName, string providerKey)
        {
            var settingGroups = new List<SettingGroupDto>();

            await Task.CompletedTask;

            return new ListResultDto<SettingGroupDto>(settingGroups);
        }

        public Task SetGlobalAsync(UpdateSettingsDto input)
        {
            throw new NotSupportedException();
        }

        public Task<ListResultDto<SettingGroupDto>> GetAllForGlobalAsync()
        {
            throw new NotSupportedException();
        }
    }
}
