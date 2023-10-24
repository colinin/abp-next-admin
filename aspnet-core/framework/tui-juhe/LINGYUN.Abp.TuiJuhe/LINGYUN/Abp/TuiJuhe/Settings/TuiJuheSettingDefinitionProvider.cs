using LINGYUN.Abp.TuiJuhe.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.TuiJuhe.Settings;

public class TuiJuheSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new[]
        {
            new SettingDefinition(
                name: TuiJuheSettingNames.Security.Token,
                displayName: L("Settings:Security.Token"),
                description: L("Settings:Security.TokenDesc"),
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
        return LocalizableString.Create<TuiJuheResource>(name);
    }
}
