using LINGYUN.Abp.Account.OAuth.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.OAuth.Settings;

public class AccountOAuthSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(GetGitHubSettings());
        context.Add(GetBilibiliSettings());
    }

    private SettingDefinition[] GetGitHubSettings()
    {
        return new SettingDefinition[]
        {
            new SettingDefinition(
                AccountOAuthSettingNames.GitHub.ClientId,
                displayName: L("Settings:GitHubClientId"),
                description: L("Settings:GitHubClientIdDesc"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName),
            new SettingDefinition(
                AccountOAuthSettingNames.GitHub.ClientSecret,
                displayName: L("Settings:GitHubClientSecret"),
                description: L("Settings:GitHubClientSecretDesc"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName),
        };
    }

    private SettingDefinition[] GetBilibiliSettings()
    {
        return new SettingDefinition[]
        {
            new SettingDefinition(
                AccountOAuthSettingNames.Bilibili.ClientId,
                displayName: L("Settings:BilibiliClientId"),
                description: L("Settings:BilibiliClientIdDesc"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName),
            new SettingDefinition(
                AccountOAuthSettingNames.Bilibili.ClientSecret,
                displayName: L("Settings:BilibiliClientSecret"),
                description: L("Settings:BilibiliClientSecretDesc"),
                isVisibleToClients: false,
                isEncrypted: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName),
        };
    }

    protected ILocalizableString L(string name)
    {
        return LocalizableString.Create<AccountOAuthResource>(name);
    }
}
