using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Tencent.QQ.Settings;

public class TencentQQSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "TenantCloud";

    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(GetQQConnectSettings());
    }

    private SettingDefinition[] GetQQConnectSettings()
    {
        return new SettingDefinition[]
        {
            new SettingDefinition(
                TencentQQSettingNames.QQConnect.AppId,
                displayName: L("DisplayName:QQConnect.AppId"),
                description: L("Description:QQConnect.AppId"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:TenantCloud"))
            .WithParent("QQConnect", L("Settings:TenantCloud.QQConnect"), order: 10)
            .WithOrder(0),
            new SettingDefinition(
                TencentQQSettingNames.QQConnect.AppKey,
                displayName: L("DisplayName:QQConnect.AppKey"),
                description: L("Description:QQConnect.AppKey"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:TenantCloud"))
            .WithParent("QQConnect", L("Settings:TenantCloud.QQConnect"), order: 10)
            .WithOrder(1),
            new SettingDefinition(
                TencentQQSettingNames.QQConnect.IsMobile,
                "false",
                L("DisplayName:QQConnect.IsMobile"),
                L("Description:QQConnect.IsMobile"),
                isVisibleToClients: false,
                isEncrypted: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:TenantCloud"))
            .WithParent("QQConnect", L("Settings:TenantCloud.QQConnect"), order: 10)
            .WithOrder(2)
            .WithValueType(ValueType.Boolean)
        };
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<TencentCloudResource>(name);
    }
}
