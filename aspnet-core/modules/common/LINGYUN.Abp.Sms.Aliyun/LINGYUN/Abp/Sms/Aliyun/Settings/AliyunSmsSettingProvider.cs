using LINGYUN.Abp.Aliyun.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Sms.Aliyun.Settings
{
    public class AliyunSmsSettingProvider : SettingDefinitionProvider
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
                    AliyunSmsSettingNames.Sms.ActionName,
                    defaultValue: "SendSms",
                    displayName: L("DisplayName:ActionName"),
                    description: L("Description:ActionName"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSmsSettingNames.Sms.DefaultSignName,
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
                    AliyunSmsSettingNames.Sms.DefaultTemplateCode,
                    displayName: L("DisplayName:DefaultTemplateCode"),
                    description: L("Description:DefaultTemplateCode"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSmsSettingNames.Sms.DefaultPhoneNumber,
                    displayName: L("DisplayName:DefaultPhoneNumber"),
                    description: L("Description:DefaultPhoneNumber"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSmsSettingNames.Sms.Domain,
                    defaultValue: "dysmsapi.aliyuncs.com",
                    displayName: L("DisplayName:Domain"),
                    description: L("Description:Domain"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSmsSettingNames.Sms.Version,
                    defaultValue: "2017-05-25",
                    displayName: L("DisplayName:Version"),
                    description: L("Description:Version"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    AliyunSmsSettingNames.Sms.VisableErrorToClient,
                    defaultValue: false.ToString(),
                    displayName: L("DisplayName:VisableErrorToClient"),
                    description: L("Description:VisableErrorToClient"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
            };
        }

        private ILocalizableString L(string name)
        {
            return LocalizableString.Create<AliyunResource>(name);
        }
    }
}
