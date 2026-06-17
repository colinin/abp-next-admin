using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.SettingManagement;

[Authorize(AbpSettingManagementPermissions.Settings.Default)]
public class SettingV2AppService : SettingV2AppServiceBase, ISettingV2AppService
{
    public SettingV2AppService(
        IDistributedEventBus eventBus,
        ISettingManager settingManager, 
        ISettingDefinitionManager settingDefinitionManager)
        : base(eventBus, settingManager, settingDefinitionManager)
    {
    }

    public async virtual Task<SettingGroupResult> GetAsync()
    {
        await CheckFeatureAsync();

        var providerName = CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host
            ? GlobalSettingValueProvider.ProviderName
            : TenantSettingValueProvider.ProviderName;
        var providerKey = CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host
            ? null
            : CurrentTenant.GetId().ToString();

        return await GetAllForProviderAsync(providerName, providerKey);
    }

    [Authorize(AbpSettingManagementPermissions.Settings.Manager)]
    public async virtual Task SetAsync(UpdateSettingsDto input)
    {
        await CheckFeatureAsync();

        var multiTenancySides = CurrentTenant.GetMultiTenancySide();
        foreach (var setting in input.Settings)
        {
            if (multiTenancySides == MultiTenancySides.Host)
            {
                await SettingManager.SetGlobalAsync(setting.Name, setting.Value);
            }
            else
            {
                await SettingManager.SetForTenantAsync(CurrentTenant.GetId(), setting.Name, setting.Value);
            }
        }

        CurrentUnitOfWork.OnCompleted(async () =>
        {
            await EventBus.PublishAsync(new CurrentApplicationConfigurationCacheResetEventData());
        });

        await CurrentUnitOfWork.SaveChangesAsync();
    }
}
