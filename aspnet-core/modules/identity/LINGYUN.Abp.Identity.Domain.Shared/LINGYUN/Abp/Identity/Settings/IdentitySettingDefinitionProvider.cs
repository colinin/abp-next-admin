using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Identity.Settings
{
    public class IdentitySettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    name: IdentitySettingNames.User.SmsNewUserRegister,
                    defaultValue: "",
                    displayName: L("DisplayName:Abp.Identity.User.SmsNewUserRegister"),
                    description: L("Description:Abp.Identity.User.SmsNewUserRegister"),
                    isVisibleToClients: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    name: IdentitySettingNames.User.SmsUserSignin,
                    defaultValue: "",
                    displayName: L("DisplayName:Abp.Identity.User.SmsUserSignin"),
                    description: L("Description:Abp.Identity.User.SmsUserSignin"),
                    isVisibleToClients: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    name: IdentitySettingNames.User.SmsResetPassword,
                    defaultValue: "",
                    displayName: L("DisplayName:Abp.Identity.User.SmsResetPassword"),
                    description: L("Description:Abp.Identity.User.SmsResetPassword"),
                    isVisibleToClients: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    name: IdentitySettingNames.User.SmsPhoneNumberConfirmed,
                    defaultValue: "",
                    displayName: L("DisplayName:Abp.Identity.User.SmsPhoneNumberConfirmed"),
                    description: L("Description:Abp.Identity.User.SmsPhoneNumberConfirmed"),
                    isVisibleToClients: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    name: IdentitySettingNames.User.SmsRepetInterval,
                    defaultValue: "5",
                    displayName: L("DisplayName:Abp.Identity.User.SmsRepetInterval"),
                    description: L("Description:Abp.Identity.User.SmsRepetInterval"),
                    isVisibleToClients: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<IdentityResource>(name);
        }
    }
}
