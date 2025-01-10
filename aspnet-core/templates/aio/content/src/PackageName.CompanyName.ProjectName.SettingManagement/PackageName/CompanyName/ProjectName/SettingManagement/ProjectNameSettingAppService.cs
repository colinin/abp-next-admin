using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Authorization;
using PackageName.CompanyName.ProjectName.Permissions;
using PackageName.CompanyName.ProjectName.Localization;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace PackageName.CompanyName.ProjectName.SettingManagement;

public class ProjectNameSettingAppService : ApplicationService, IProjectNameSettingAppService
{
    protected ISettingManager SettingManager { get; }
    protected ISettingDefinitionManager SettingDefinitionManager { get; }

    public ProjectNameSettingAppService(
        ISettingManager settingManager,
        ISettingDefinitionManager settingDefinitionManager)
    {
        SettingManager = settingManager;
        SettingDefinitionManager = settingDefinitionManager;
        LocalizationResource = typeof(ProjectNameResource);
    }

    public virtual async Task<SettingGroupResult> GetAllForCurrentTenantAsync()
    {
        return await GetAllForProviderAsync(TenantSettingValueProvider.ProviderName, CurrentTenant.GetId().ToString());
    }

    [Authorize]
    public virtual async Task<SettingGroupResult> GetAllForCurrentUserAsync()
    {
        return await GetAllForProviderAsync(UserSettingValueProvider.ProviderName, CurrentUser.GetId().ToString());
    }

    public virtual async Task<SettingGroupResult> GetAllForGlobalAsync()
    {
        return await GetAllForProviderAsync(GlobalSettingValueProvider.ProviderName, null);
    }

    [Authorize(ProjectNamePermissions.ManageSettings)]
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

    [Authorize]
    public virtual async Task SetCurrentUserAsync(UpdateSettingsDto input)
    {
        // 增加特性检查
        await CheckFeatureAsync();

        foreach (var setting in input.Settings)
        {
            await SettingManager.SetForCurrentUserAsync(setting.Name, setting.Value);
        }

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(ProjectNamePermissions.ManageSettings)]
    public virtual async Task SetGlobalAsync(UpdateSettingsDto input)
    {
        // 增加特性检查
        await CheckFeatureAsync();

        foreach (var setting in input.Settings)
        {
            await SettingManager.SetGlobalAsync(setting.Name, setting.Value);
        }

        await CurrentUnitOfWork.SaveChangesAsync();
    }


    protected virtual async Task CheckFeatureAsync()
    {
        await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
    }

    protected virtual async Task<SettingGroupResult> GetAllForProviderAsync(string providerName, string providerKey)
    {
        var settingGroups = new SettingGroupResult();

        //TODO: 当前项目所有配置项在此定义返回

        await Task.CompletedTask;

        return settingGroups;
    }
}
