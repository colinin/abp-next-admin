using AspNet.Security.OAuth.GitHub;
using LINGYUN.Abp.Account.OAuth.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.OAuth.ExternalProviders.GitHub;

public class GitHubAuthHandlerOptionsProvider : OAuthHandlerOptionsProvider<GitHubAuthenticationOptions>
{
    public GitHubAuthHandlerOptionsProvider(ISettingProvider settingProvider) : base(settingProvider)
    {
    }

    public async override Task SetOptionsAsync(GitHubAuthenticationOptions options)
    {
        var clientId = await SettingProvider.GetOrNullAsync(AccountOAuthSettingNames.GitHub.ClientId);
        var clientSecret = await SettingProvider.GetOrNullAsync(AccountOAuthSettingNames.GitHub.ClientSecret);

        if (!clientId.IsNullOrWhiteSpace())
        {
            options.ClientId = clientId;
        }
        if (!clientSecret.IsNullOrWhiteSpace())
        {
            options.ClientSecret = clientSecret;
        }
    }
}
