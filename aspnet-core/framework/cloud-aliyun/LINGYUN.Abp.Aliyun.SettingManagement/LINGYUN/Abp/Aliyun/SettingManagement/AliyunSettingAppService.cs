using LINGYUN.Abp.Aliyun.Features;
using LINGYUN.Abp.Aliyun.Localization;
using LINGYUN.Abp.Aliyun.Settings;
using LINGYUN.Abp.SettingManagement;
using System.Collections.Generic;
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

            // 无权限返回空结果,直接报错的话,网关聚合会抛出异常
            if (await FeatureChecker.IsEnabledAsync(AliyunFeatureNames.Enable) && 
                await PermissionChecker.IsGrantedAsync(AliyunSettingPermissionNames.Settings))
            {
                var aliyunSettingGroup = new SettingGroupDto(L["DisplayName:Aliyun"], L["Description:Aliyun"]);
                #region 访问控制

                var ramSetting = aliyunSettingGroup.AddSetting(L["DisplayName:Aliyun.RAM"], L["Description:Aliyun.RAM"]);

                ramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(AliyunSettingNames.Authorization.RegionId),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.RegionId, providerName, providerKey),
                    ValueType.Option,
                    providerName)
                    .AddOptions(GetAvailableRegionOptions());
                ramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(AliyunSettingNames.Authorization.AccessKeyId),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.AccessKeyId, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(AliyunSettingNames.Authorization.AccessKeySecret),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.AccessKeySecret, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(AliyunSettingNames.Authorization.RamRoleArn),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.RamRoleArn, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(AliyunSettingNames.Authorization.RoleSessionName),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.RoleSessionName, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(AliyunSettingNames.Authorization.Policy),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.Policy, providerName, providerKey),
                    ValueType.String,
                    providerName);
                ramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(AliyunSettingNames.Authorization.UseSecurityTokenService),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.UseSecurityTokenService, providerName, providerKey),
                    ValueType.Boolean,
                    providerName);
                ramSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(AliyunSettingNames.Authorization.DurationSeconds),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(AliyunSettingNames.Authorization.DurationSeconds, providerName, providerKey),
                    ValueType.Number,
                    providerName);

                #endregion

                #region 短信

                if (await FeatureChecker.IsEnabledAsync(AliyunFeatureNames.Sms.Enable))
                {
                    var smsSetting = aliyunSettingGroup.AddSetting(L["DisplayName:Aliyun.Sms"], L["Description:Aliyun.Sms"]);
                    smsSetting.AddDetail(
                       await SettingDefinitionManager.GetAsync(AliyunSettingNames.Sms.Domain),
                       StringLocalizerFactory,
                       await SettingManager.GetOrNullAsync(AliyunSettingNames.Sms.Domain, providerName, providerKey),
                       ValueType.String,
                        providerName);
                    smsSetting.AddDetail(
                       await SettingDefinitionManager.GetAsync(AliyunSettingNames.Sms.Version),
                       StringLocalizerFactory,
                       await SettingManager.GetOrNullAsync(AliyunSettingNames.Sms.Version, providerName, providerKey),
                       ValueType.String,
                        providerName);
                    smsSetting.AddDetail(
                       await SettingDefinitionManager.GetAsync(AliyunSettingNames.Sms.ActionName),
                       StringLocalizerFactory,
                       await SettingManager.GetOrNullAsync(AliyunSettingNames.Sms.ActionName, providerName, providerKey),
                       ValueType.String,
                        providerName);
                    smsSetting.AddDetail(
                       await SettingDefinitionManager.GetAsync(AliyunSettingNames.Sms.DefaultPhoneNumber),
                       StringLocalizerFactory,
                       await SettingManager.GetOrNullAsync(AliyunSettingNames.Sms.DefaultPhoneNumber, providerName, providerKey),
                       ValueType.String,
                        providerName);
                    smsSetting.AddDetail(
                       await SettingDefinitionManager.GetAsync(AliyunSettingNames.Sms.DefaultSignName),
                       StringLocalizerFactory,
                       await SettingManager.GetOrNullAsync(AliyunSettingNames.Sms.DefaultSignName, providerName, providerKey),
                       ValueType.String,
                        providerName);
                    smsSetting.AddDetail(
                       await SettingDefinitionManager.GetAsync(AliyunSettingNames.Sms.DefaultTemplateCode),
                       StringLocalizerFactory,
                       await SettingManager.GetOrNullAsync(AliyunSettingNames.Sms.DefaultTemplateCode, providerName, providerKey),
                       ValueType.String,
                        providerName);
                    smsSetting.AddDetail(
                       await SettingDefinitionManager.GetAsync(AliyunSettingNames.Sms.VisableErrorToClient),
                       StringLocalizerFactory,
                       await SettingManager.GetOrNullAsync(AliyunSettingNames.Sms.VisableErrorToClient, providerName, providerKey),
                       ValueType.Boolean,
                        providerName);
                }

                #endregion

                settingGroups.AddGroup(aliyunSettingGroup);
            }

            return settingGroups;
        }

        protected virtual IEnumerable<OptionDto> GetAvailableRegionOptions()
        {
            return new OptionDto[]
            {
                new OptionDto(L["Region:HangZhou"], "oss-cn-hangzhou"),
                new OptionDto(L["Region:ShangHai"], "oss-cn-shanghai"),
                new OptionDto(L["Region:NanJing"], "oss-cn-nanjing"),
                new OptionDto(L["Region:FuZhou"], "oss-cn-fuzhou"),
                new OptionDto(L["Region:WuHan"], "oss-cn-wuhan"),
                new OptionDto(L["Region:QingDao"], "oss-cn-qingdao"),
                new OptionDto(L["Region:BeiJing"], "oss-cn-beijing"),
                new OptionDto(L["Region:ZhangJiaKou"], "oss-cn-zhangjiakou"),
                new OptionDto(L["Region:HuHeHaoTe"], "oss-cn-huhehaote"),
                new OptionDto(L["Region:WuLanChaBu"], "oss-cn-wulanchabu"),
                new OptionDto(L["Region:ShenZhen"], "oss-cn-shenzhen"),
                new OptionDto(L["Region:HeYuan"], "oss-cn-heyuan"),
                new OptionDto(L["Region:GuangZhou"], "oss-cn-guangzhou"),
                new OptionDto(L["Region:ChengDu"], "oss-cn-chengdu"),
                new OptionDto(L["Region:HongKong"], "oss-cn-hongkong"),
                new OptionDto(L["Region:SiliconValley"], "oss-us-west-1"),
                new OptionDto(L["Region:Virginia"], "oss-us-east-1"),
                new OptionDto(L["Region:Tokoyo"], "oss-ap-northeast-1"),
                new OptionDto(L["Region:Seoul"], "oss-ap-northeast-2"),
                new OptionDto(L["Region:Singapore"], "oss-ap-southeast-1"),
                new OptionDto(L["Region:Sydney"], "oss-ap-southeast-2"),
                new OptionDto(L["Region:KualaLumpur"], "oss-ap-southeast-3"),
                new OptionDto(L["Region:Jakarta"], "oss-ap-southeast-5"),
                new OptionDto(L["Region:Manila"], "oss-ap-southeast-6"),
                new OptionDto(L["Region:Bangkok"], "oss-ap-southeast-7"),
                new OptionDto(L["Region:Bombay"], "oss-ap-south-1"),
                new OptionDto(L["Region:Frankfurt"], "oss-eu-central-1"),
                new OptionDto(L["Region:London"], "oss-eu-west-1"),
                new OptionDto(L["Region:Dubai"], "oss-me-east-1"),
                new OptionDto(L["Region:MainLand"], "oss-rg-china-mainland"),
            };
        }
    }
}
