using LINGYUN.Abp.Account.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account
{
    public class AbpAccountSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {

            context.Add(GetAccountSettings());
        }

        protected SettingDefinition[] GetAccountSettings()
        {
            return new SettingDefinition[]
            {
                new SettingDefinition(
                    name: AccountSettingNames.SmsRegisterTemplateCode, 
                    defaultValue: "SMS_190728520", 
                    displayName: L("DisplayName:SmsRegisterTemplateCode"), 
                    description: L("Description:SmsRegisterTemplateCode"),
                    isVisibleToClients: true)
                .WithProviders(
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    name: AccountSettingNames.SmsSigninTemplateCode,
                    defaultValue: "SMS_190728516",
                    displayName: L("DisplayName:SmsSigninTemplateCode"),
                    description: L("Description:SmsSigninTemplateCode"),
                    isVisibleToClients: true)
                .WithProviders(
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    name: AccountSettingNames.SmsResetPasswordTemplateCode,
                    defaultValue: "SMS_192530831",
                    displayName: L("DisplayName:SmsResetPasswordTemplateCode"),
                    description: L("Description:SmsResetPasswordTemplateCode"),
                    isVisibleToClients: true)
                .WithProviders(
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    name: AccountSettingNames.PhoneVerifyCodeExpiration,
                    defaultValue: "3", 
                    displayName: L("DisplayName:PhoneVerifyCodeExpiration"), 
                    description: L("Description:PhoneVerifyCodeExpiration"),
                    isVisibleToClients: true)
                .WithProviders(
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
            };
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<AccountResource>(name);
        }
    }
}
