using LINGYUN.Abp.WeChat.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Settings;

public class WeChatSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "WeChat";

    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                WeChatSettingNames.EnabledQuickLogin, 
                // 默认启用
                true.ToString(),
                L("DisplayName:WeChat.EnabledQuickLogin"),
                L("Description:WeChat.EnabledQuickLogin"),
                isVisibleToClients: true,
                isEncrypted: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:WeChat"))
            .WithParent("UserLogin", L("Settings:WeChat.UserLogin"), order: 0)
            .WithOrder(0)
            .WithValueType(ValueType.Boolean)
        );
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatResource>(name);
    }
}
