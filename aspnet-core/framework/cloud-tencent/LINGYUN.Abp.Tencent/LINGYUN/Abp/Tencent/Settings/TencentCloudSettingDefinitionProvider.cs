using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Tencent.Settings;

public class TencentCloudSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(GetBasicSettings());
        context.Add(GetConnectionSettings());
        context.Add(GetSmsSettings());
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
                    TenantSettingValueProvider.ProviderName),
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
                    TenantSettingValueProvider.ProviderName),
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
                    TenantSettingValueProvider.ProviderName),
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
                    TenantSettingValueProvider.ProviderName),
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
                    TenantSettingValueProvider.ProviderName),
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
                    TenantSettingValueProvider.ProviderName),
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
                    TenantSettingValueProvider.ProviderName),
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
                    TenantSettingValueProvider.ProviderName),
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
                    TenantSettingValueProvider.ProviderName),
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
                    TenantSettingValueProvider.ProviderName),
        };
    }

    private ILocalizableString L(string name)
    {
        return LocalizableString.Create<TencentCloudResource>(name);
    }
}
