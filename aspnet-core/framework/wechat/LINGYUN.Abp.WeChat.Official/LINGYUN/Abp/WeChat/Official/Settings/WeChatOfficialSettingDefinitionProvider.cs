using LINGYUN.Abp.WeChat.Localization;
using LINGYUN.Abp.WeChat.Official.Features;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Official.Settings;

public class WeChatOfficialSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "WeChat";
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                WeChatOfficialSettingNames.IsSandBox,
                "false",
                L("DisplayName:WeChat.Official.IsSandBox"),
                L("Description:WeChat.Official.IsSandBox"),
                isVisibleToClients: false,
                isEncrypted: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:WeChat"))
            .WithParent(
                "Official", 
                L("Settings:WeChat.Official"),
                requiredFeatures: [WeChatOfficialFeatures.Enable], order: 2)
            .WithOrder(0)
            .WithValueType(ValueType.Boolean),
            new SettingDefinition(
                WeChatOfficialSettingNames.AppId, "",
                L("DisplayName:WeChat.Official.AppId"),
                L("Description:WeChat.Official.AppId"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:WeChat"))
            .WithParent(
                "Official",
                L("Settings:WeChat.Official"),
                requiredFeatures: [WeChatOfficialFeatures.Enable], order: 2)
            .WithOrder(1),
            new SettingDefinition(
                WeChatOfficialSettingNames.AppSecret, "",
                L("DisplayName:WeChat.Official.AppSecret"),
                L("Description:WeChat.Official.AppSecret"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:WeChat"))
            .WithParent(
                "Official",
                L("Settings:WeChat.Official"),
                requiredFeatures: [WeChatOfficialFeatures.Enable], order: 2)
            .WithOrder(2),
            new SettingDefinition(
                WeChatOfficialSettingNames.Url, "",
                L("DisplayName:WeChat.Official.Url"),
                L("Description:WeChat.Official.Url"),
                isVisibleToClients: false,
                isEncrypted: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:WeChat"))
            .WithParent(
                "Official",
                L("Settings:WeChat.Official"),
                requiredFeatures: [WeChatOfficialFeatures.Enable], order: 2)
            .WithOrder(3),
            new SettingDefinition(
                WeChatOfficialSettingNames.Token, "",
                L("DisplayName:WeChat.Official.Token"),
                L("Description:WeChat.Official.Token"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:WeChat"))
            .WithParent(
                "Official",
                L("Settings:WeChat.Official"),
                requiredFeatures: [WeChatOfficialFeatures.Enable], order: 2)
            .WithOrder(4),
            new SettingDefinition(
                WeChatOfficialSettingNames.EncodingAESKey, "",
                L("DisplayName:WeChat.Official.EncodingAESKey"),
                L("Description:WeChat.Official.EncodingAESKey"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:WeChat"))
            .WithParent(
                "Official",
                L("Settings:WeChat.Official"),
                requiredFeatures: [WeChatOfficialFeatures.Enable], order: 2)
            .WithOrder(5)
        );
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatResource>(name);
    }
}
