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
                defaultValue: "qGE8NZ8rrQYj207kSv9vb5XzG1a+iK6z8yGPICjx3cY5p8bGcmMav5t0DRLjCprA",
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
                defaultValue: "HXGIfCpkUNonrOB8znJzNcDoKvZBKpNZ0tN38tktgrg=",
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
