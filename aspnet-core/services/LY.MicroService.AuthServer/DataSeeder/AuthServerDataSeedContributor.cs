using Microsoft.Extensions.Configuration;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.PermissionManagement;

namespace LY.MicroService.AuthServer.DataSeeder;

public class ServerDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public static HashSet<string> InitializeScopes = new HashSet<string>
    {
        // obsolete! microservice should be allocated separately
        "lingyun-abp-application",
        // admin service
        "ams", 
        // identity service
        "ids", 
        // localization service
        "lts",
        // platform service
        "pts",
        // message service
        "mgs",
        // task service
        "tks",
        // webhook service
        "wks",
        // workflow service
        "wfs",
        // wechat service
        "was"
    };

    private readonly IConfiguration _configuration;
    private readonly ICurrentTenant _currentTenant;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictApplicationRepository _applicationRepository;

    private readonly IPermissionDataSeeder _permissionDataSeeder;

    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly IOpenIddictScopeRepository _scopeRepository;

    public ServerDataSeedContributor(
        IConfiguration configuration,
        ICurrentTenant currentTenant,
        IOpenIddictScopeManager scopeManager,
        IOpenIddictScopeRepository scopeRepository,
        IPermissionDataSeeder permissionDataSeeder,
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictApplicationRepository applicationRepository)
    {
        _configuration = configuration;
        _currentTenant = currentTenant;
        _scopeManager = scopeManager;
        _scopeRepository = scopeRepository;
        _permissionDataSeeder = permissionDataSeeder;
        _applicationManager = applicationManager;
        _applicationRepository = applicationRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context.TenantId))
        {
            await CreateScopeAsync(InitializeScopes);

            await CreateApplicationAsync(InitializeScopes);
        }
    }

    private async Task CreateScopeAsync(IEnumerable<string> scopes)
    {
        foreach (var scope in scopes)
        {
            if (await _scopeRepository.FindByNameAsync(scope) == null)
            {
                await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor()
                {
                    Name = scope,
                    DisplayName = scope + " access",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("zh-Hans")] = "Abp API 应用程序访问",
                        [CultureInfo.GetCultureInfo("en")] = "Abp API Application Access"
                    },
                    Resources =
                    {
                        scope
                    }
                });
            }
        }
    }

    private async Task CreateApplicationAsync(IEnumerable<string> scopes)
    {
        var configurationSection = _configuration.GetSection("OpenIddict:Applications");

        var vueClientId = configurationSection["VueAdmin:ClientId"];
        if (!vueClientId.IsNullOrWhiteSpace())
        {
            var vueClientRootUrl = configurationSection["VueAdmin:RootUrl"].EnsureEndsWith('/');

            if (await _applicationRepository.FindByClientIdAsync(vueClientId) == null)
            {
                var application = new OpenIddictApplicationDescriptor
                {
                    ClientId = vueClientId,
                    ClientSecret = configurationSection["VueAdmin:ClientSecret"],
                    ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                    ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                    DisplayName = "Abp Vue Admin Client",
                    PostLogoutRedirectUris =
                    {
                        new Uri(vueClientRootUrl + "signout-callback"),
                        new Uri(vueClientRootUrl)
                    },
                    RedirectUris =
                    {
                        new Uri(vueClientRootUrl + "signin-callback"),
                        new Uri(vueClientRootUrl)
                    },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.EndSession,

                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.Implicit,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                        OpenIddictConstants.Permissions.ResponseTypes.Code,
                        OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken,
                        OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken,
                        OpenIddictConstants.Permissions.ResponseTypes.CodeToken,
                        OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                        OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
                        OpenIddictConstants.Permissions.ResponseTypes.None,
                        OpenIddictConstants.Permissions.ResponseTypes.Token,

                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Address,
                        OpenIddictConstants.Permissions.Scopes.Phone,
                    }
                };
                foreach (var scope in scopes)
                {
                    application.Permissions.AddIfNotContains(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
                }

                await _applicationManager.CreateAsync(application);

                var vueClientPermissions = new string[1]
                {
                    "AbpIdentity.UserLookup"
                };
                await _permissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, vueClientId, vueClientPermissions);
            }
        }

        var internalServiceClientId = configurationSection["InternalService:ClientId"];
        if (!internalServiceClientId.IsNullOrWhiteSpace())
        {
            if (await _applicationRepository.FindByClientIdAsync(internalServiceClientId) == null)
            {
                var application = new OpenIddictApplicationDescriptor
                {
                    ClientId = internalServiceClientId,
                    ClientSecret = configurationSection["InternalService:ClientSecret"],
                    ClientType = OpenIddictConstants.ClientTypes.Confidential,
                    ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                    ApplicationType = OpenIddictConstants.ApplicationTypes.Native,
                    DisplayName = "Abp Vue Admin Client",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.EndSession,

                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.Implicit,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                        OpenIddictConstants.Permissions.ResponseTypes.Code,
                        OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken,
                        OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken,
                        OpenIddictConstants.Permissions.ResponseTypes.CodeToken,
                        OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                        OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
                        OpenIddictConstants.Permissions.ResponseTypes.None,
                        OpenIddictConstants.Permissions.ResponseTypes.Token,

                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Address,
                        OpenIddictConstants.Permissions.Scopes.Phone,
                    }
                };
                foreach (var scope in scopes)
                {
                    application.Permissions.AddIfNotContains(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
                }

                await _applicationManager.CreateAsync(application);

                var internalServicePermissions = new string[2]
                {
                    "AbpIdentity.UserLookup","AbpIdentity.Users"
                };
                await _permissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, internalServiceClientId, internalServicePermissions);
            }
        }

        var oauthClientId = configurationSection["VueOAuthClient:ClientId"];
        if (!oauthClientId.IsNullOrWhiteSpace())
        {
            var oauthClientRootUrls = configurationSection.GetSection("VueOAuthClient:RootUrls").Get<List<string>>();

            if (await _applicationRepository.FindByClientIdAsync(oauthClientId) == null)
            {
                var application = new OpenIddictApplicationDescriptor
                {
                    ClientId = oauthClientId,
                    ClientSecret = null,
                    ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                    ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                    DisplayName = "OAuth Client",
                    PostLogoutRedirectUris = { },
                    RedirectUris = { },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.EndSession,

                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                        OpenIddictConstants.Permissions.ResponseTypes.Code,
                        OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken,
                        OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken,
                        OpenIddictConstants.Permissions.ResponseTypes.CodeToken,
                        OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                        OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
                        OpenIddictConstants.Permissions.ResponseTypes.None,
                        OpenIddictConstants.Permissions.ResponseTypes.Token,

                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Address,
                        OpenIddictConstants.Permissions.Scopes.Phone,
                    }
                };
                foreach (var scope in scopes)
                {
                    application.Permissions.AddIfNotContains(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
                }

                oauthClientRootUrls.ForEach(url =>
                {
                    application.PostLogoutRedirectUris.AddIfNotContains(new Uri(url.EnsureEndsWith('/')));
                    application.PostLogoutRedirectUris.AddIfNotContains(new Uri(url.EnsureEndsWith('/') + "signout-callback"));

                    application.RedirectUris.AddIfNotContains(new Uri(url));
                    application.RedirectUris.AddIfNotContains(new Uri(url.EnsureEndsWith('/') + "signin-callback"));
                    application.RedirectUris.AddIfNotContains(new Uri(url.EnsureEndsWith('/') + "swagger/oauth2-redirect.html"));
                });

                await _applicationManager.CreateAsync(application);

                var oauthClientPermissions = new string[1]
                {
                    "AbpIdentity.UserLookup"
                };
                await _permissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, oauthClientId, oauthClientPermissions);
            }
        }
    }
}
