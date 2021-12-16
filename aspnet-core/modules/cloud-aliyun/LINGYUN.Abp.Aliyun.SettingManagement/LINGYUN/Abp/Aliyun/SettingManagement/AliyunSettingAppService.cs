using LINGYUN.Abp.Aliyun.Localization;
using LINGYUN.Abp.Aliyun.Settings;
using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.Sms.Aliyun.Settings;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using ValueType = LINGYUN.Abp.SettingManagement.ValueType;

namespace LINGYUN.Abp.Aliyun.SettingManagement
{
    public class AliyunSettingAppService : ApplicationService, IAliyunSettingAppService
    {
        protected ISettingManager SettingManager { get; }
        protected IPermissionChecker PermissionChecker { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        public AliyunSettingAppService(
            ISettingManager settingManager,
            IPermissionChecker permissionChecker,
            ISettingDefinitionManager settingDefinitionManager)
        {
            SettingManager = settingManager;
            PermissionChecker = permissionChecker;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationResource = typeof(AliyunResource);
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
            if (await PermissionChecker.IsGrantedAsync(AliyunSettingPermissionNames.Settings))
            {
                var aliyunSettingGroup = new SettingGroupDto(L["DisplayName:Aliyun"], L["Description:Aliyun"]);
                #region 访问控制

                var ramSetting = aliyunSettingGroup.AddSetting(L["DisplayName:Aliyun.RAM"], L["Description:Aliyun.RAM"]);

                ramSetting.AddDetail(
                    SettingDefinitionManager.Get(AliyunSettingNames.Authorization.RegionId),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.RegionId, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    SettingDefinitionManager.Get(AliyunSettingNames.Authorization.AccessKeyId),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.AccessKeyId, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    SettingDefinitionManager.Get(AliyunSettingNames.Authorization.AccessKeySecret),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.AccessKeySecret, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    SettingDefinitionManager.Get(AliyunSettingNames.Authorization.RamRoleArn),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.RamRoleArn, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    SettingDefinitionManager.Get(AliyunSettingNames.Authorization.RoleSessionName),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.RoleSessionName, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    SettingDefinitionManager.Get(AliyunSettingNames.Authorization.Policy),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.Policy, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    SettingDefinitionManager.Get(AliyunSettingNames.Authorization.UseSecurityTokenService),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.UseSecurityTokenService, providerName, providerKey),
                    ValueType.Boolean,
                    providerName);
                ramSetting.AddDetail(
                    SettingDefinitionManager.Get(AliyunSettingNames.Authorization.DurationSeconds),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.DurationSeconds, providerName, providerKey),
                    ValueType.Number,
                    providerName);

                #endregion

                #region 短信

                var smsSetting = aliyunSettingGroup.AddSetting(L["DisplayName:Aliyun.Sms"], L["Description:Aliyun.Sms"]);
                smsSetting.AddDetail(
                   SettingDefinitionManager.Get(AliyunSmsSettingNames.Sms.Domain),
                   StringLocalizerFactory,
                   await SettingManager.GetOrNullAsync(AliyunSmsSettingNames.Sms.Domain, providerName, providerKey),
                   ValueType.String,
                    providerName);
                smsSetting.AddDetail(
                   SettingDefinitionManager.Get(AliyunSmsSettingNames.Sms.Version),
                   StringLocalizerFactory,
                   await SettingManager.GetOrNullAsync(AliyunSmsSettingNames.Sms.Version, providerName, providerKey),
                   ValueType.String,
                    providerName);
                smsSetting.AddDetail(
                   SettingDefinitionManager.Get(AliyunSmsSettingNames.Sms.ActionName),
                   StringLocalizerFactory,
                   await SettingManager.GetOrNullAsync(AliyunSmsSettingNames.Sms.ActionName, providerName, providerKey),
                   ValueType.String,
                    providerName);
                smsSetting.AddDetail(
                   SettingDefinitionManager.Get(AliyunSmsSettingNames.Sms.DefaultPhoneNumber),
                   StringLocalizerFactory,
                   await SettingManager.GetOrNullAsync(AliyunSmsSettingNames.Sms.DefaultPhoneNumber, providerName, providerKey),
                   ValueType.String,
                    providerName);
                smsSetting.AddDetail(
                   SettingDefinitionManager.Get(AliyunSmsSettingNames.Sms.DefaultSignName),
                   StringLocalizerFactory,
                   await SettingManager.GetOrNullAsync(AliyunSmsSettingNames.Sms.DefaultSignName, providerName, providerKey),
                   ValueType.String,
                    providerName);
                smsSetting.AddDetail(
                   SettingDefinitionManager.Get(AliyunSmsSettingNames.Sms.DefaultTemplateCode),
                   StringLocalizerFactory,
                   await SettingManager.GetOrNullAsync(AliyunSmsSettingNames.Sms.DefaultTemplateCode, providerName, providerKey),
                   ValueType.String,
                    providerName);
                smsSetting.AddDetail(
                   SettingDefinitionManager.Get(AliyunSmsSettingNames.Sms.VisableErrorToClient),
                   StringLocalizerFactory,
                   await SettingManager.GetOrNullAsync(AliyunSmsSettingNames.Sms.VisableErrorToClient, providerName, providerKey),
                   ValueType.Boolean,
                    providerName);

                #endregion

                settingGroups.AddGroup(aliyunSettingGroup);
            }

            return settingGroups;
        }
    }
}
