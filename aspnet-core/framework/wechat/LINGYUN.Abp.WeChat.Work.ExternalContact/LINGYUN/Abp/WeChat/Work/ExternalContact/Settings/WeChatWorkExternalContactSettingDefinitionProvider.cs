using LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Settings;

public class WeChatWorkExternalContactSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "WeChatWork";
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
            .WithGroup(
                GroupName,
                L("Settings:WeChatWork"),
                requiredFeatures: [WeChatWorkFeatureNames.Enable])
            .WithParent(
                "WeChatWorkExternalContact",
                L("Settings:WeChatWork.WeChatWorkExternalContact"),
                requiredFeatures: [WeChatWorkExternalContactFeatureNames.Enable], order: 4)
            .WithOrder(0)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatWorkResource>(name);
    }
}
