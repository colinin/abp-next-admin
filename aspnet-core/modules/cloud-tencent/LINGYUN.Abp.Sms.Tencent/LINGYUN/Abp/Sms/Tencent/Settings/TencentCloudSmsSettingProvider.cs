using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Sms.Tencent.Settings
{
    public class TencentCloudSmsSettingProvider : SettingDefinitionProvider
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
                    TencentCloudSmsSettingNames.AppId,
                    displayName: L("DisplayName:AppId"),
                    description: L("Description:AppId"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    TencentCloudSmsSettingNames.DefaultSignName,
                    displayName: L("DisplayName:DefaultSignName"),
                    description: L("Description:DefaultSignName"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    TencentCloudSmsSettingNames.DefaultTemplateId,
                    displayName: L("DisplayName:DefaultTemplateId"),
                    description: L("Description:DefaultTemplateId"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
            };
        }

        private ILocalizableString L(string name)
        {
            return LocalizableString.Create<TencentCloudResource>(name);
        }
    }
}
