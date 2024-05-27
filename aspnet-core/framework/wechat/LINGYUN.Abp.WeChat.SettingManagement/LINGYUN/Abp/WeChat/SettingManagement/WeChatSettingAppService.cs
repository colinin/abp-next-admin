using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.WeChat.Localization;
using LINGYUN.Abp.WeChat.MiniProgram.Features;
using LINGYUN.Abp.WeChat.MiniProgram.Settings;
using LINGYUN.Abp.WeChat.Official.Features;
using LINGYUN.Abp.WeChat.Official.Settings;
using LINGYUN.Abp.WeChat.Settings;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Settings;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.SettingManagement
{
    public class WeChatSettingAppService : ApplicationService, IWeChatSettingAppService
    {
        protected ISettingManager SettingManager { get; }
        protected IPermissionChecker PermissionChecker { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        public WeChatSettingAppService(
            ISettingManager settingManager,
            IPermissionChecker permissionChecker,
            ISettingDefinitionManager settingDefinitionManager)
        {
            SettingManager = settingManager;
            PermissionChecker = permissionChecker;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationResource = typeof(WeChatResource);
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
            var wechatSettingGroup = new SettingGroupDto(L["DisplayName:WeChat"], L["Description:WeChat"]);

            var loginSetting = wechatSettingGroup.AddSetting(L["UserLogin"], L["UserLogin"]);
            loginSetting.AddDetail(
                await SettingDefinitionManager.GetAsync(WeChatSettingNames.EnabledQuickLogin),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(WeChatSettingNames.EnabledQuickLogin, providerName, providerKey),
                ValueType.Boolean,
                providerName);

            // 无权限返回空结果,直接报错的话,网关聚合会抛出异常
            if (await FeatureChecker.IsEnabledAsync(WeChatOfficialFeatures.Enable) &&
                await PermissionChecker.IsGrantedAsync(WeChatSettingPermissionNames.Official))
            {
                #region 公众号

                var officialSetting = wechatSettingGroup.AddSetting(L["DisplayName:WeChat.Official"], L["Description:WeChat.Official"]);
                officialSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatOfficialSettingNames.IsSandBox),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.IsSandBox, providerName, providerKey),
                    ValueType.Boolean,
                    providerName);
                officialSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatOfficialSettingNames.AppId),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.AppId, providerName, providerKey),
                    ValueType.String,
                    providerName);
                officialSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatOfficialSettingNames.AppSecret),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.AppSecret, providerName, providerKey),
                    ValueType.String,
                    providerName);
                officialSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatOfficialSettingNames.Url),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.Url, providerName, providerKey),
                    ValueType.String,
                    providerName);
                officialSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatOfficialSettingNames.Token),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.Token, providerName, providerKey),
                    ValueType.String,
                    providerName);
                officialSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatOfficialSettingNames.EncodingAESKey),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.EncodingAESKey, providerName, providerKey),
                    ValueType.String,
                    providerName);

                #endregion
            }

            if (await FeatureChecker.IsEnabledAsync(WeChatMiniProgramFeatures.Enable) &&
                await PermissionChecker.IsGrantedAsync(WeChatSettingPermissionNames.MiniProgram))
            {
                #region 小程序

                var miniProgramSetting = wechatSettingGroup.AddSetting(L["DisplayName:WeChat.MiniProgram"], L["Description:WeChat.MiniProgram"]);
                miniProgramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatMiniProgramSettingNames.AppId),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatMiniProgramSettingNames.AppId, providerName, providerKey),
                    ValueType.String,
                    providerName);
                miniProgramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatMiniProgramSettingNames.AppSecret),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatMiniProgramSettingNames.AppSecret, providerName, providerKey),
                    ValueType.String,
                    providerName);
                miniProgramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatMiniProgramSettingNames.Token),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatMiniProgramSettingNames.Token, providerName, providerKey),
                    ValueType.String,
                    providerName);
                miniProgramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatMiniProgramSettingNames.EncodingAESKey),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatMiniProgramSettingNames.EncodingAESKey, providerName, providerKey),
                    ValueType.String,
                    providerName);

                #endregion
            }

            settingGroups.AddGroup(wechatSettingGroup);

            if (await FeatureChecker.IsEnabledAsync(WeChatWorkFeatureNames.Enable) &&
                await PermissionChecker.IsGrantedAsync(WeChatSettingPermissionNames.Work))
            {
                #region 企业微信
                var wechatWorkSettingGroup = new SettingGroupDto(L["DisplayName:WeChatWork"], L["Description:WeChatWork"]);

                var workLoginSetting = wechatWorkSettingGroup.AddSetting(L["UserLogin"], L["UserLogin"]);
                workLoginSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatWorkSettingNames.EnabledQuickLogin),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatWorkSettingNames.EnabledQuickLogin, providerName, providerKey),
                    ValueType.Boolean,
                    providerName);

                var workConnectionSetting = wechatWorkSettingGroup.AddSetting(L["DisplayName:Connection"], L["Description:Connection"]);

                workConnectionSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(WeChatWorkSettingNames.Connection.CorpId),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId, providerName, providerKey),
                    ValueType.String,
                    providerName);

                settingGroups.AddGroup(wechatWorkSettingGroup);
                #endregion
            }

            return settingGroups;
        }
    }
}
