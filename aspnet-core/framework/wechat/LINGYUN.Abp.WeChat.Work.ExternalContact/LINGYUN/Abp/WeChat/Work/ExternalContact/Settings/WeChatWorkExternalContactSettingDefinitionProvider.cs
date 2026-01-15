using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Settings;

public class WeChatWorkExternalContactSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                WeChatWorkExternalContactSettingNames.Secret,
                displayName: L("DisplayName:WeChatWorkExternalContact.Secret"),
                description: L("Description:WeChatWorkExternalContact.Secret"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
        );
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatWorkResource>(name);
    }
}
