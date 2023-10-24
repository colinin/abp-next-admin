using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.TuiJuhe.Localization;
using LINGYUN.Abp.TuiJuhe.Settings;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.TuiJuhe.SettingManagement
{
    public class TuiJuheSettingAppService : ApplicationService, ITuiJuheSettingAppService
    {
        protected ISettingManager SettingManager { get; }
        protected IPermissionChecker PermissionChecker { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        public TuiJuheSettingAppService(
            ISettingManager settingManager,
            IPermissionChecker permissionChecker,
            ISettingDefinitionManager settingDefinitionManager)
        {
            SettingManager = settingManager;
            PermissionChecker = permissionChecker;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationResource = typeof(TuiJuheResource);
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
            var wxPusherSettingGroup = new SettingGroupDto(L["DisplayName:TuiJuhe"], L["Description:TuiJuhe"]);

            if (await PermissionChecker.IsGrantedAsync(TuiJuheSettingPermissionNames.ManageSetting))
            {
                var securitySetting = wxPusherSettingGroup.AddSetting(L["Security"], L["Security"]);
                securitySetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(TuiJuheSettingNames.Security.Token),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(TuiJuheSettingNames.Security.Token, providerName, providerKey),
                    ValueType.String,
                    providerName);
            }

            settingGroups.AddGroup(wxPusherSettingGroup);

            return settingGroups;
        }
    }
}
