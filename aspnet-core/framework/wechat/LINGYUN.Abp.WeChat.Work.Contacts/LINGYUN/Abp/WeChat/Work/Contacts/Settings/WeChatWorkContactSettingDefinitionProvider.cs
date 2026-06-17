using LINGYUN.Abp.WeChat.Work.Contacts.Features;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Settings;

public class WeChatWorkContactSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "WeChatWork";
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                WeChatWorkContactSettingNames.Secret,
                displayName: L("DisplayName:WeChatWorkContact.Secret"),
                description: L("Description:WeChatWorkContact.Secret"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName,
                L("Settings:WeChatWork"),
                requiredFeatures: [WeChatWorkFeatureNames.Enable])
            .WithParent(
                "WeChatWorkContact",
                L("Settings:WeChatWork.WeChatWorkContact"),
                requiredFeatures: [WeChatWorkContactsFeatureNames.Enable], order: 3)
            .WithOrder(0)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatWorkResource>(name);
    }
}
