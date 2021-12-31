using LINGYUN.Abp.Tencent.Localization;
using LINGYUN.Abp.Tencent.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINYUN.Abp.Tencent.Settings;

public class TencentCloudSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(CreateTencentCloudSettings());
    }

    private SettingDefinition[] CreateTencentCloudSettings()
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
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
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
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
        };
    }

    private ILocalizableString L(string name)
    {
        return LocalizableString.Create<TencentCloudResource>(name);
    }
}
