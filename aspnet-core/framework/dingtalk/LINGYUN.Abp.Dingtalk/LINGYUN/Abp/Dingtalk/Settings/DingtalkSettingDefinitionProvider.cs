using LINGYUN.Abp.Dingtalk.Features;
using LINGYUN.Abp.Dingtalk.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Dingtalk.Settings;

public class DingtalkSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "Dingtalk";

    public override void Define(ISettingDefinitionContext context)
    {
        context.Add([
            new SettingDefinition(
                name: DingtalkSettingNames.AppKey,
                displayName: L("DisplayName:AppKey"),
                description: L("Description:AppKey"),
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName,
                L("DisplayName:Dingtalk"),
                requiredFeatures: [DingtalkFeatureNames.Enable])
            .WithParent("AccessToken", L("DisplayName:Dingtalk.AccessToken")),
            new SettingDefinition(
                name: DingtalkSettingNames.AppSecret,
                displayName: L("DisplayName:AppSecret"),
                description: L("Description:AppSecret"),
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName,
                L("DisplayName:Dingtalk"),
                requiredFeatures: [DingtalkFeatureNames.Enable])
            .WithParent("AccessToken", L("DisplayName:Dingtalk.AccessToken")),
        ]);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DingtalkReousrce>(name);
    }
}
