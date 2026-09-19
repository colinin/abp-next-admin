using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Settings;

public class TencentCaptchaSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "TenantCloud";

    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                    TencentCaptchaSettingNames.CaptchaAppId,
                    displayName: L("DisplayName:CaptchaAppId"),
                    description: L("Description:CaptchaAppId"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("Captcha", L("DisplayName:TenantCloud.Captcha"), order: 20)
                .WithOrder(1)
                .WithValueType(ValueType.String),
            new SettingDefinition(
                    TencentCaptchaSettingNames.AppSecretKey,
                    displayName: L("DisplayName:CaptchaSecretKey"),
                    description: L("Description:CaptchaSecretKey"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("Captcha", L("DisplayName:TenantCloud.Captcha"), order: 20)
                .WithOrder(2)
                .WithValueType(ValueType.String),
            new SettingDefinition(
                    TencentCaptchaSettingNames.CaptchaAppIdEncryptedType,
                    displayName: L("DisplayName:CaptchaAppIdEncryptedType"),
                    description: L("Description:CaptchaAppIdEncryptedType"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("Captcha", L("DisplayName:TenantCloud.Captcha"), order: 20)
                .WithOrder(3)
                .WithOptions([
                    new NameValue<string>("cbc","cbc"),
                    new NameValue<string>("gcm","gcm")]),
            new SettingDefinition(
                    TencentCaptchaSettingNames.CaptchaAppIdEncryptedExpireTime,
                    defaultValue: 300.ToString(),
                    displayName: L("DisplayName:CaptchaAppIdEncryptedExpireTime"),
                    description: L("Description:CaptchaAppIdEncryptedExpireTime"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("Captcha", L("DisplayName:TenantCloud.Captcha"), order: 20)
                .WithOrder(4)
                .WithValueType(ValueType.Number));
    }

    private LocalizableString L(string name)
    {
        return LocalizableString.Create<TencentCloudResource>(name);
    }
}
