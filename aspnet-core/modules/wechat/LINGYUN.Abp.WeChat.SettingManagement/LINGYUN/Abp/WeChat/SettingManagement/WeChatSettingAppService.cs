using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.WeChat.Localization;
using LINGYUN.Abp.WeChat.MiniProgram.Settings;
using LINGYUN.Abp.WeChat.Official.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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

        public virtual async Task<ListResultDto<SettingGroupDto>> GetAllForCurrentTenantAsync()
        {
            return await GetAllForProviderAsync(TenantSettingValueProvider.ProviderName, CurrentTenant.GetId().ToString());
        }

        public virtual async Task<ListResultDto<SettingGroupDto>> GetAllForGlobalAsync()
        {
            return await GetAllForProviderAsync(GlobalSettingValueProvider.ProviderName, null);
        }

        protected virtual async Task<ListResultDto<SettingGroupDto>> GetAllForProviderAsync(string providerName, string providerKey)
        {
            var settingGroups = new List<SettingGroupDto>();
            var wechatSettingGroup = new SettingGroupDto(L["DisplayName:WeChat"], L["Description:WeChat"]);

            // 无权限返回空结果,直接报错的话,网关聚合会抛出异常
            if (await PermissionChecker.IsGrantedAsync(WeChatSettingPermissionNames.Official))
            {
                #region 公众号

                var officialSetting = wechatSettingGroup.AddSetting(L["DisplayName:WeChat.Official"], L["Description:WeChat.Official"]);
                officialSetting.AddDetail(
                    SettingDefinitionManager.Get(WeChatOfficialSettingNames.AppId),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.AppId, providerName, providerKey),
                    ValueType.String);
                officialSetting.AddDetail(
                    SettingDefinitionManager.Get(WeChatOfficialSettingNames.AppSecret),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.AppSecret, providerName, providerKey),
                    ValueType.String);
                officialSetting.AddDetail(
                    SettingDefinitionManager.Get(WeChatOfficialSettingNames.Url),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.Url, providerName, providerKey),
                    ValueType.String);
                officialSetting.AddDetail(
                    SettingDefinitionManager.Get(WeChatOfficialSettingNames.Token),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.Token, providerName, providerKey),
                    ValueType.String);
                officialSetting.AddDetail(
                    SettingDefinitionManager.Get(WeChatOfficialSettingNames.EncodingAESKey),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatOfficialSettingNames.EncodingAESKey, providerName, providerKey),
                    ValueType.String);

                #endregion
            }

            if (await PermissionChecker.IsGrantedAsync(WeChatSettingPermissionNames.MiniProgram))
            {
                #region 小程序

                var miniProgramSetting = wechatSettingGroup.AddSetting(L["DisplayName:WeChat.MiniProgram"], L["Description:WeChat.MiniProgram"]);
                miniProgramSetting.AddDetail(
                    SettingDefinitionManager.Get(WeChatMiniProgramSettingNames.AppId),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatMiniProgramSettingNames.AppId, providerName, providerKey),
                    ValueType.String);
                miniProgramSetting.AddDetail(
                    SettingDefinitionManager.Get(WeChatMiniProgramSettingNames.AppSecret),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatMiniProgramSettingNames.AppSecret, providerName, providerKey),
                    ValueType.String);
                miniProgramSetting.AddDetail(
                    SettingDefinitionManager.Get(WeChatMiniProgramSettingNames.Token),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatMiniProgramSettingNames.Token, providerName, providerKey),
                    ValueType.String);
                miniProgramSetting.AddDetail(
                    SettingDefinitionManager.Get(WeChatMiniProgramSettingNames.EncodingAESKey),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(WeChatMiniProgramSettingNames.EncodingAESKey, providerName, providerKey),
                    ValueType.String);

                #endregion
            }

            settingGroups.Add(wechatSettingGroup);
            return new ListResultDto<SettingGroupDto>(settingGroups);
        }
    }
}
