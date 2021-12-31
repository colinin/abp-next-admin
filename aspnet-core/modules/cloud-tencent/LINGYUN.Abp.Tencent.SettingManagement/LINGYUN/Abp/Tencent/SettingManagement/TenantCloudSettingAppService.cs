using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.Sms.Tencent.Settings;
using LINGYUN.Abp.Tencent.Localization;
using LINGYUN.Abp.Tencent.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using ValueType = LINGYUN.Abp.SettingManagement.ValueType;

namespace LINGYUN.Abp.Tencent.SettingManagement;

public class TenantCloudSettingAppService : ApplicationService, ITenantCloudSettingAppService
{
    protected ISettingManager SettingManager { get; }
    protected IPermissionChecker PermissionChecker { get; }
    protected ISettingDefinitionManager SettingDefinitionManager { get; }

    public TenantCloudSettingAppService(
        ISettingManager settingManager,
        IPermissionChecker permissionChecker,
        ISettingDefinitionManager settingDefinitionManager)
    {
        SettingManager = settingManager;
        PermissionChecker = permissionChecker;
        SettingDefinitionManager = settingDefinitionManager;

        LocalizationResource = typeof(TencentCloudResource);
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
        if (await PermissionChecker.IsGrantedAsync(TenantCloudSettingPermissionNames.Settings))
        {
            var settingGroup = new SettingGroupDto(L["DisplayName:TenantCloud"], L["DisplayName:TenantCloud"]);

            #region 基本设置

            var basicSetting = settingGroup.AddSetting(L["DisplayName:TenantCloud.BasicSetting"], L["Description:TenantCloud.BasicSetting"]);

            basicSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSettingNames.EndPoint),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSettingNames.EndPoint, providerName, providerKey),
                ValueType.Option,
                providerName)
                .AddOptions(GetAvailableRegionOptions());
            basicSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSettingNames.SecretId),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSettingNames.SecretId, providerName, providerKey),
                ValueType.String,
                providerName);
            basicSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSettingNames.SecretKey),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSettingNames.SecretKey, providerName, providerKey),
                ValueType.String,
                providerName);
            basicSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSettingNames.DurationSecond),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSettingNames.SecretKey, providerName, providerKey),
                ValueType.Number,
                providerName);

            #endregion

            #region 连接信息

            var connectionSetting = settingGroup.AddSetting(
                L["DisplayName:TenantCloud.ConnectionSetting"], L["Description:TenantCloud.ConnectionSetting"]);

            connectionSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSettingNames.Connection.HttpMethod),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSettingNames.Connection.HttpMethod, providerName, providerKey),
                ValueType.Option,
                providerName)
                .AddOption("POST", "POST")
                .AddOption("GET", "GET");
            connectionSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSettingNames.Connection.Timeout),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSettingNames.Connection.Timeout, providerName, providerKey),
                ValueType.Number,
                providerName);
            connectionSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSettingNames.Connection.WebProxy),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSettingNames.Connection.WebProxy, providerName, providerKey),
                ValueType.Number,
                providerName);

            #endregion

            #region 短信设置

            var smsSetting = settingGroup.AddSetting(
                L["DisplayName:TenantCloud.SmsSetting"], L["Description:TenantCloud.SmsSetting"]);

            smsSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSmsSettingNames.AppId),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSmsSettingNames.AppId, providerName, providerKey),
                ValueType.String,
                providerName);
            smsSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSmsSettingNames.DefaultTemplateId),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSmsSettingNames.DefaultTemplateId, providerName, providerKey),
                ValueType.String,
                providerName);
            smsSetting.AddDetail(
                SettingDefinitionManager.Get(TencentCloudSmsSettingNames.DefaultSignName),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(TencentCloudSmsSettingNames.DefaultSignName, providerName, providerKey),
                ValueType.String,
                providerName);

            #endregion

            settingGroups.AddGroup(settingGroup);
        }

        return settingGroups;
    }

    protected virtual IEnumerable<OptionDto> GetAvailableRegionOptions()
    {
        return new OptionDto[]
        {
            new OptionDto(L["Region:Beijing"], "ap-beijing"),
            new OptionDto(L["Region:Chengdu"], "ap-chengdu"),
            new OptionDto(L["Region:Chongqing"], "ap-chongqing"),
            new OptionDto(L["Region:Guangzhou"], "ap-guangzhou"),
            new OptionDto(L["Region:Hongkong"], "ap-hongkong"),
            new OptionDto(L["Region:Nanjing"], "ap-nanjing"),
            new OptionDto(L["Region:Shanghai"], "ap-shanghai"),
            new OptionDto(L["Region:ShanghaiFsi"], "ap-shanghai-fsi"),
            new OptionDto(L["Region:ShenzhenFsi"], "ap-shenzhen-fsi"),
            new OptionDto(L["Region:Bangkok"], "ap-bangkok"),
            new OptionDto(L["Region:Jakarta"], "ap-jakarta"),
            new OptionDto(L["Region:Mumbai"], "ap-mumbai"),
            new OptionDto(L["Region:Seoul"], "ap-seoul"),
            new OptionDto(L["Region:Singapore"], "ap-singapore"),
            new OptionDto(L["Region:Tokyo"], "ap-tokyo"),
            new OptionDto(L["Region:Frankfurt"], "eu-frankfurt"),
            new OptionDto(L["Region:Moscow"], "eu-moscow"),
            new OptionDto(L["Region:Virginia"], "na-ashburn"),
            new OptionDto(L["Region:SiliconValley"], "na-siliconvalley"),
            new OptionDto(L["Region:Toronto"], "na-toronto"),
        };
    }
}
