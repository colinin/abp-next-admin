using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace LINGYUN.Abp.SettingManagement;

[Authorize]
public class UserSettingV2AppService : SettingV2AppServiceBase, IUserSettingV2AppService
{
    public UserSettingV2AppService(
        IDistributedEventBus eventBus,
        ISettingManager settingManager, 
        ISettingDefinitionManager settingDefinitionManager) 
        : base(eventBus, settingManager, settingDefinitionManager)
    {
    }

    public async virtual Task<SettingGroupResult> GetAsync()
    {
        await CheckFeatureAsync();

        return await GetAllForProviderAsync(UserSettingValueProvider.ProviderName, CurrentUser.GetId().ToString());
    }

    public async virtual Task SetAsync(UpdateSettingsDto input)
    {
        await CheckFeatureAsync();

        foreach (var setting in input.Settings)
        {
            await SettingManager.SetForCurrentUserAsync(setting.Name, setting.Value);
        }
    }
}
