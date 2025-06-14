using AspNet.Security.OAuth.Bilibili;
using AspNet.Security.OAuth.GitHub;
using AspNet.Security.OAuth.QQ;
using AspNet.Security.OAuth.Weixin;
using AspNet.Security.OAuth.WorkWeixin;
using LINGYUN.Abp.Account.OAuth.Features;
using LINGYUN.Abp.Account.Web.ExternalProviders;
using LINGYUN.Abp.Account.Web.Models;
using LINGYUN.Abp.Account.Web.OAuth.Pages.Account.Components.ExternalProviders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Account.Web.OAuth.ExternalProviders;

public class OAuthExternalProviderService : IExternalProviderService, ITransientDependency
{
    private static readonly Dictionary<string, string> _providerFeaturesMap = new Dictionary<string, string>
    {
        [GitHubAuthenticationDefaults.AuthenticationScheme] = AccountOAuthFeatureNames.GitHub.Enable,
        [QQAuthenticationDefaults.AuthenticationScheme] = AccountOAuthFeatureNames.QQ.Enable,
        [WeixinAuthenticationDefaults.AuthenticationScheme] = AccountOAuthFeatureNames.WeChat.Enable,
        [WorkWeixinAuthenticationDefaults.AuthenticationScheme] = AccountOAuthFeatureNames.WeCom.Enable,
        [BilibiliAuthenticationDefaults.AuthenticationScheme] = AccountOAuthFeatureNames.Bilibili.Enable
    };

    private readonly IFeatureChecker _featureChecker;
    private readonly IStringLocalizer<AccountResource> _stringLocalizer;
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
    public OAuthExternalProviderService(
        IFeatureChecker featureChecker,
        IStringLocalizer<AccountResource> stringLocalizer,
        IAuthenticationSchemeProvider authenticationSchemeProvider)
    {
        _featureChecker = featureChecker;
        _stringLocalizer = stringLocalizer;
        _authenticationSchemeProvider = authenticationSchemeProvider;
    }
    public async virtual Task<List<ExternalLoginProviderModel>> GetAllAsync()
    {
        var models = new List<ExternalLoginProviderModel>();

        var schemas = await _authenticationSchemeProvider.GetAllSchemesAsync();

        foreach (var schema in schemas)
        {
            if (_providerFeaturesMap.TryGetValue(schema.Name, out var schemaFeature))
            {
                if (await _featureChecker.IsEnabledAsync(schemaFeature))
                {
                    models.Add(new ExternalLoginProviderModel
                    {
                        Name = schema.Name,
                        AuthenticationScheme = schema.Name,
                        DisplayName = _stringLocalizer[$"OAuth:{schema.Name}"],
                        ComponentType = typeof(ExternalProviderViewComponent),
                    });
                }
            }
        }

        return models;
    }
}
