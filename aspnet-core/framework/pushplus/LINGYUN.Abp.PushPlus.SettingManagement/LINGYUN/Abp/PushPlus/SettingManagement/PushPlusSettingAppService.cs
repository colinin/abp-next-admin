using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.PushPlus.Localization;
using LINGYUN.Abp.PushPlus.Settings;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using LINGYUN.Abp.PushPlus.Features;

namespace LINGYUN.Abp.PushPlus.SettingManagement
{
    public class PushPlusSettingAppService : ApplicationService, IPushPlusSettingAppService
    {
        protected ISettingManager SettingManager { get; }
        protected IPermissionChecker PermissionChecker { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        public PushPlusSettingAppService(
            ISettingManager settingManager,
            IPermissionChecker permissionChecker,
            ISettingDefinitionManager settingDefinitionManager)
        {
            SettingManager = settingManager;
            PermissionChecker = permissionChecker;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationResource = typeof(PushPlusResource);
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
            var pushPlusSettingGroup = new SettingGroupDto(L["DisplayName:PushPlus"], L["Description:PushPlus"]);

            if (await FeatureChecker.IsEnabledAsync(PushPlusFeatureNames.Message.Enable) &&
                await PermissionChecker.IsGrantedAsync(PushPlusSettingPermissionNames.ManageSetting))
            {
                var securitySetting = pushPlusSettingGroup.AddSetting(L["Security"], L["Security"]);
                securitySetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(PushPlusSettingNames.Security.Token),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(PushPlusSettingNames.Security.Token, providerName, providerKey),
                    ValueType.String,
                    providerName);
                securitySetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(PushPlusSettingNames.Security.SecretKey),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(PushPlusSettingNames.Security.SecretKey, providerName, providerKey),
                    ValueType.String,
                    providerName);
            }

            settingGroups.AddGroup(pushPlusSettingGroup);

            return settingGroups;
        }
    }
}
