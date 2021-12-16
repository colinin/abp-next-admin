using LINGYUN.Abp.OssManagement.Localization;
using LINGYUN.Abp.OssManagement.Permissions;
using LINGYUN.Abp.OssManagement.Settings;
using LINGYUN.Abp.SettingManagement;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using ValueType = LINGYUN.Abp.SettingManagement.ValueType;

namespace LINGYUN.Abp.OssManagement.SettingManagement
{
    public class OssManagementSettingAppService : ApplicationService, IOssManagementSettingAppService
    {
        protected ISettingManager SettingManager { get; }
        protected IPermissionChecker PermissionChecker { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        public OssManagementSettingAppService(
            ISettingManager settingManager,
            IPermissionChecker permissionChecker,
            ISettingDefinitionManager settingDefinitionManager)
        {
            SettingManager = settingManager;
            PermissionChecker = permissionChecker;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationResource = typeof(AbpOssManagementResource);
        }

        public virtual async Task<SettingGroupResult> GetAllForCurrentTenantAsync()
        {
            return await GetAllForProviderAsync(TenantSettingValueProvider.ProviderName, CurrentTenant.GetId().ToString());
        }

        public virtual async Task<SettingGroupResult> GetAllForGlobalAsync()
        {
            return await GetAllForProviderAsync(GlobalSettingValueProvider.ProviderName, null);
        }

        protected virtual async Task<SettingGroupResult> GetAllForProviderAsync(string providerName, string providerKey)
        {
            var settingGroups = new SettingGroupResult();

            // 无权限返回空结果,直接报错的话,网关聚合会抛出异常
            if (await PermissionChecker.IsGrantedAsync(AbpOssManagementPermissions.OssObject.Default))
            {
                var ossSettingGroup = new SettingGroupDto(L["DisplayName:OssManagement"], L["Description:OssManagement"]);

                var ossObjectSetting = ossSettingGroup.AddSetting(L["DisplayName:OssObject"], L["Description:OssObject"]);

                ossObjectSetting.AddDetail(
                    SettingDefinitionManager.Get(AbpOssManagementSettingNames.FileLimitLength),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AbpOssManagementSettingNames.FileLimitLength, providerName, providerKey),
                    ValueType.Number);
                ossObjectSetting.AddDetail(
                    SettingDefinitionManager.Get(AbpOssManagementSettingNames.AllowFileExtensions),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AbpOssManagementSettingNames.AllowFileExtensions, providerName, providerKey),
                    ValueType.String);

                settingGroups.AddGroup(ossSettingGroup);
            }

            return settingGroups;
        }
    }
}
