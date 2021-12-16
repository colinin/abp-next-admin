using LINGYUN.Abp.Aliyun.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Aliyun.Settings
{
    public class AliyunSettingProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(CreateAliyunSettings());
        }

        private SettingDefinition[] CreateAliyunSettings()
        {
            return new SettingDefinition[]
            {
                new SettingDefinition(
                    AliyunSettingNames.Authorization.AccessKeyId,
                    displayName: L("DisplayName:AccessKeyId"),
                    description: L("Description:AccessKeyId"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSettingNames.Authorization.AccessKeySecret,
                    displayName: L("DisplayName:AccessKeySecret"),
                    description: L("Description:AccessKeySecret"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSettingNames.Authorization.DurationSeconds,
                    defaultValue: "3600",
                    displayName: L("DisplayName:DurationSeconds"),
                    description: L("Description:DurationSeconds"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSettingNames.Authorization.Policy,
                    displayName: L("DisplayName:Policy"),
                    description: L("Description:Policy"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSettingNames.Authorization.RamRoleArn,
                    displayName: L("DisplayName:RamRoleArn"),
                    description: L("Description:RamRoleArn"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSettingNames.Authorization.RegionId,
                    displayName: L("DisplayName:RegionId"),
                    description: L("Description:RegionId"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSettingNames.Authorization.RoleSessionName,
                    displayName: L("DisplayName:RoleSessionName"),
                    description: L("Description:RoleSessionName"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSettingNames.Authorization.UseSecurityTokenService,
                    defaultValue: true.ToString(),
                    displayName: L("DisplayName:UseSecurityTokenService"),
                    description: L("Description:UseSecurityTokenService"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
            };
        }

        private ILocalizableString L(string name)
        {
            return LocalizableString.Create<AliyunResource>(name);
        }
    }
}
