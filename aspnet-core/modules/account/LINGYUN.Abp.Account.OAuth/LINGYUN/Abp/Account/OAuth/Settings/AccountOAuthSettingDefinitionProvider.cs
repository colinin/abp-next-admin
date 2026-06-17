using LINGYUN.Abp.Account.OAuth.Features;
using LINGYUN.Abp.Account.OAuth.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.OAuth.Settings;

public class AccountOAuthSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "ExternalOAuthLogin";

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
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:ExternalOAuthLogin"))
            .WithParent("GitHubAuth", L("Settings:GitHubAuth"))
            .RequiredFeatures([AccountOAuthFeatureNames.GitHub.Enable]),
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
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:ExternalOAuthLogin"))
            .WithParent("GitHubAuth", L("Settings:GitHubAuth"))
            .RequiredFeatures([AccountOAuthFeatureNames.GitHub.Enable]),
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
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:ExternalOAuthLogin"))
            .WithParent("Bilibili", L("Settings:BilibiliAuth"))
            .RequiredFeatures([AccountOAuthFeatureNames.Bilibili.Enable]),
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
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:ExternalOAuthLogin"))
            .WithParent("Bilibili", L("Settings:BilibiliAuth"))
            .RequiredFeatures([AccountOAuthFeatureNames.Bilibili.Enable]),
        };
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountOAuthResource>(name);
    }
}
