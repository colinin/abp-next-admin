using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.WxPusher.Features;
using LINGYUN.Abp.WxPusher.Localization;
using LINGYUN.Abp.WxPusher.Settings;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WxPusher.SettingManagement
{
    public class WxPusherSettingAppService : ApplicationService, IWxPusherSettingAppService
    {
        protected ISettingManager SettingManager { get; }
        protected IPermissionChecker PermissionChecker { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        public WxPusherSettingAppService(
            ISettingManager settingManager,
            IPermissionChecker permissionChecker,
            ISettingDefinitionManager settingDefinitionManager)
        {
            SettingManager = settingManager;
            PermissionChecker = permissionChecker;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationResource = typeof(WxPusherResource);
        }

        public async virtual Task<SettingGroupResult> GetAllForCurrentTenantAsync()
        {
            return await GetAllForProviderAsync(TenantSettingValueProvider.ProviderName, CurrentTenant.GetId().ToString());
        }

        public async virtual Task<SettingGroupResult> GetAllForGlobalAsync()
        {
            return await GetAllForProviderAsync(GlobalSettingValueProvider.ProviderName, null);
        }

        protected async virtual Task<SettingGroupResult> GetAllForProviderAsync(string providerName, string providerKey)
        {
            var settingGroups = new SettingGroupResult();
            var wxPusherSettingGroup = new SettingGroupDto(L["DisplayName:WxPusher"], L["Description:WxPusher"]);

            if (await FeatureChecker.IsEnabledAsync(WxPusherFeatureNames.Enable) &&
                await PermissionChecker.IsGrantedAsync(WxPusherSettingPermissionNames.ManageSetting))
            {
                var securitySetting = wxPusherSettingGroup.AddSetting(L["Security"], L["Security"]);
                securitySetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WxPusherSettingNames.Security.AppToken),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WxPusherSettingNames.Security.AppToken, providerName, providerKey),
                    ValueType.String,
                    providerName);
            }

            settingGroups.AddGroup(wxPusherSettingGroup);

            return settingGroups;
        }
    }
}
