using LINGYUN.Abp.WxPusher.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WxPusher.Settings;

public class WxPusherSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new[]
        {
            new SettingDefinition(
                name: WxPusherSettingNames.Security.AppToken,
                displayName: L("Settings:Security.AppToken"),
                description: L("Settings:Security.AppTokenDesc"),
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName),
        });
    }

    public static ILocalizableString L(string name)
    {
        return LocalizableString.Create<WxPusherResource>(name);
    }
}
