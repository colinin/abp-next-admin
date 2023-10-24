using LINGYUN.Abp.PushPlus.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.PushPlus.Settings;

public class PushPlusSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new[]
        {
            new SettingDefinition(
                name: PushPlusSettingNames.Security.Token,
                displayName: L("Settings:Security.Token"),
                description: L("Settings:Security.TokenDesc"),
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName),
            new SettingDefinition(
                name: PushPlusSettingNames.Security.SecretKey,
                displayName: L("Settings:Security.SecretKey"),
                description: L("Settings:Security.SecretKeyDesc"),
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
        return LocalizableString.Create<PushPlusResource>(name);
    }
}
