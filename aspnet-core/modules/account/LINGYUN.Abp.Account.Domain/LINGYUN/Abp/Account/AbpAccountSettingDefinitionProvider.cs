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
                new SettingDefinition(AccountSettingNames.SmsRegisterTemplateCode, 
                    "SMS_190728520", L("DisplayName:SmsRegisterTemplateCode"), L("Description:SmsRegisterTemplateCode")),
                new SettingDefinition(AccountSettingNames.SmsSigninTemplateCode, 
                    "SMS_190728516", L("DisplayName:SmsSigninTemplateCode"), L("Description:SmsSigninTemplateCode")),
                new SettingDefinition(AccountSettingNames.PhoneVerifyCodeExpiration,
                    "3", L("DisplayName:PhoneVerifyCodeExpiration"), L("Description:PhoneVerifyCodeExpiration")),
            };
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<AccountResource>(name);
        }
    }
}
