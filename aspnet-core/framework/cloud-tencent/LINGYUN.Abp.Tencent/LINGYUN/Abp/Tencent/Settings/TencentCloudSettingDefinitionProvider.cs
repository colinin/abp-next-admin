using LINGYUN.Abp.Tencent.Features;
using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Tencent.Settings;

public class TencentCloudSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "TenantCloud";

    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(GetBasicSettings());
        context.Add(GetConnectionSettings());
        context.Add(GetSmsSettings());
        context.Add(GetCaptchaSettings());
    }

    private SettingDefinition[] GetConnectionSettings()
    {
        return new SettingDefinition[]
        {
            new SettingDefinition(
                    TencentCloudSettingNames.Connection.HttpMethod,
                    // 默认 post
                    defaultValue: "POST",
                    displayName: L("DisplayName:HttpMethod"),
                    description: L("Description:HttpMethod"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("ConnectionSetting", L("Settings:TenantCloud.ConnectionSetting"), order: 0)
                .WithOrder(0),
                new SettingDefinition(
                    TencentCloudSettingNames.Connection.Timeout,
                    // 默认 60秒
                    defaultValue: "60",
                    displayName: L("DisplayName:Timeout"),
                    description: L("Description:Timeout"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("ConnectionSetting", L("Settings:TenantCloud.ConnectionSetting"), order: 0)
                .WithOrder(1)
                .WithValueType(ValueType.Number),
                new SettingDefinition(
                    TencentCloudSettingNames.Connection.WebProxy,
                    displayName: L("DisplayName:WebProxy"),
                    description: L("Description:WebProxy"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("ConnectionSetting", L("Settings:TenantCloud.ConnectionSetting"), order: 0)
                .WithOrder(2),
        };
    }

    private SettingDefinition[] GetSmsSettings()
    {
        return new SettingDefinition[]
            {
                new SettingDefinition(
                    TencentCloudSettingNames.Sms.AppId,
                    displayName: L("DisplayName:AppId"),
                    description: L("Description:AppId"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(
                    GroupName,
                    L("Settings:TenantCloud"),
                    requiredFeatures: [TencentCloudFeatures.Sms.Enable])
                .WithParent("SmsSetting", L("DisplayName:TenantCloud.SmsSetting"), order: 2)
                .WithOrder(0),
                new SettingDefinition(
                    TencentCloudSettingNames.Sms.DefaultSignName,
                    displayName: L("DisplayName:DefaultSignName"),
                    description: L("Description:DefaultSignName"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(
                    GroupName,
                    L("Settings:TenantCloud"),
                    requiredFeatures: [TencentCloudFeatures.Sms.Enable])
                .WithParent("SmsSetting", L("DisplayName:TenantCloud.SmsSetting"), order: 2)
                .WithOrder(1),
                new SettingDefinition(
                    TencentCloudSettingNames.Sms.DefaultTemplateId,
                    displayName: L("DisplayName:DefaultTemplateId"),
                    description: L("Description:DefaultTemplateId"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(
                    GroupName,
                    L("Settings:TenantCloud"),
                    requiredFeatures: [TencentCloudFeatures.Sms.Enable])
                .WithParent("SmsSetting", L("DisplayName:TenantCloud.SmsSetting"), order: 2)
                .WithOrder(2),
            };
    }

    private SettingDefinition[] GetBasicSettings()
    {
        return new SettingDefinition[]
        {
                new SettingDefinition(
                    TencentCloudSettingNames.EndPoint,
                    // 腾讯云默认使用广州区域
                    defaultValue: "ap-guangzhou",
                    displayName: L("DisplayName:EndPoint"),
                    description: L("Description:EndPoint"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("BasicSetting", L("DisplayName:TenantCloud.BasicSetting"), order: 1)
                .WithOrder(0),
                new SettingDefinition(
                    TencentCloudSettingNames.SecretId,
                    displayName: L("DisplayName:SecretId"),
                    description: L("Description:SecretId"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("BasicSetting", L("DisplayName:TenantCloud.BasicSetting"), order: 1)
                .WithOrder(1),
                new SettingDefinition(
                    TencentCloudSettingNames.SecretKey,
                    displayName: L("DisplayName:SecretKey"),
                    description: L("Description:SecretKey"),
                    isVisibleToClients: false,
                    isEncrypted: true
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("BasicSetting", L("DisplayName:TenantCloud.BasicSetting"), order: 1)
                .WithOrder(2),
                new SettingDefinition(
                    TencentCloudSettingNames.DurationSecond,
                    defaultValue: "600",
                    displayName: L("DisplayName:DurationSecond"),
                    description: L("Description:DurationSecond"),
                    isVisibleToClients: false
                )
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
                .WithGroup(GroupName, L("Settings:TenantCloud"))
                .WithParent("BasicSetting", L("DisplayName:TenantCloud.BasicSetting"), order: 1)
                .WithOrder(3)
                .WithValueType(ValueType.Number),
        };
    }

    private SettingDefinition[] GetCaptchaSettings()
    {
        return new SettingDefinition[]
        {
            new SettingDefinition(
                    TencentCloudSettingNames.Captcha.CaptchaAppId,
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
                    TencentCloudSettingNames.Captcha.AppSecretKey,
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
                    TencentCloudSettingNames.Captcha.CaptchaAppIdEncryptedType,
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
                    TencentCloudSettingNames.Captcha.CaptchaAppIdEncryptedExpireTime,
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
                .WithValueType(ValueType.Number)
        };
    }

    private LocalizableString L(string name)
    {
        return LocalizableString.Create<TencentCloudResource>(name);
    }
}
