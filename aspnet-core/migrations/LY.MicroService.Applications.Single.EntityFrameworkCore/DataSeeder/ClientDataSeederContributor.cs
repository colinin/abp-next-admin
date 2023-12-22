using LINGYUN.Abp.IdentityServer.IdentityResources;
using Microsoft.Extensions.Configuration;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.DataSeeder;

public class ClientDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    private readonly IClientRepository _clientRepository;
    private readonly IApiResourceRepository _apiResourceRepository;
    private readonly IApiScopeRepository _apiScopeRepository;
    private readonly ICustomIdentityResourceDataSeeder _customIdentityResourceDataSeeder;
    private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;

    private readonly IGuidGenerator _guidGenerator;
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTenant _currentTenant;

    public ClientDataSeederContributor(
        IOpenIddictApplicationManager applicationManager, 
        IOpenIddictScopeManager scopeManager, 
        IClientRepository clientRepository, 
        IApiResourceRepository apiResourceRepository, 
        IApiScopeRepository apiScopeRepository, 
        ICustomIdentityResourceDataSeeder customIdentityResourceDataSeeder, 
        IIdentityResourceDataSeeder identityResourceDataSeeder, 
        IGuidGenerator guidGenerator,
        IPermissionDataSeeder permissionDataSeeder, 
        IConfiguration configuration, 
        ICurrentTenant currentTenant)
    {
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
        _clientRepository = clientRepository;
        _apiResourceRepository = apiResourceRepository;
        _apiScopeRepository = apiScopeRepository;
        _customIdentityResourceDataSeeder = customIdentityResourceDataSeeder;
        _identityResourceDataSeeder = identityResourceDataSeeder;
        _guidGenerator = guidGenerator;
        _permissionDataSeeder = permissionDataSeeder;
        _configuration = configuration;
        _currentTenant = currentTenant;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context.TenantId))
        {
            if (_configuration.GetValue<bool>("AuthServer:UseOpenIddict"))
            {
                await SeedOpenIddictAsync();
                return;
            }

            await SeedIdentityServerAsync();
        }
    }

    #region OpenIddict

    private async Task SeedOpenIddictAsync()
    {
        await CreateScopeAsync("lingyun-abp-application");
        await CreateApplicationAsync("lingyun-abp-application");
    }

    private async Task CreateScopeAsync(string scope)
    {
        if (await _scopeManager.FindByNameAsync(scope) == null)
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

    private async Task CreateApplicationAsync(string scope)
    {
        var configurationSection = _configuration.GetSection("OpenIddict:Applications");

        var vueClientId = configurationSection["VueAdmin:ClientId"];
        if (!vueClientId.IsNullOrWhiteSpace())
        {
            var vueClientRootUrl = configurationSection["VueAdmin:RootUrl"].EnsureEndsWith('/');

            if (await _applicationManager.FindByClientIdAsync(vueClientId) == null)
            {
                await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = vueClientId,
                    ClientSecret = "1q2w3e*",
                    ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                    ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                    DisplayName = "Abp Vue Admin Client",
                    PostLogoutRedirectUris =
                    {
                        new Uri(vueClientRootUrl + "signout-callback-oidc"),
                        new Uri(vueClientRootUrl)
                    },
                    RedirectUris =
                    {
                        new Uri(vueClientRootUrl + "/signin-oidc"),
                        new Uri(vueClientRootUrl)
                    },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.Device,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.Logout,

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
                        OpenIddictConstants.Permissions.Prefixes.Scope + scope
                    }
                });

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
            if (await _applicationManager.FindByClientIdAsync(internalServiceClientId) == null)
            {
                await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = internalServiceClientId,
                    ClientSecret = "1q2w3e*",
                    ClientType = OpenIddictConstants.ClientTypes.Confidential,
                    ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                    ApplicationType = OpenIddictConstants.ApplicationTypes.Native,
                    DisplayName = "Abp Vue Admin Client",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.Device,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.Logout,

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
                        OpenIddictConstants.Permissions.Prefixes.Scope + scope
                    }
                });

                var internalServicePermissions = new string[2]
            {
                "AbpIdentity.UserLookup","AbpIdentity.Users"
            };
                await _permissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, internalServiceClientId, internalServicePermissions);
            }
        }
    }

    #endregion

    #region IdentityServer

    private async Task SeedIdentityServerAsync()
    {
        await _identityResourceDataSeeder.CreateStandardResourcesAsync();
        await _customIdentityResourceDataSeeder.CreateCustomResourcesAsync();
        await CreateApiResourcesAsync();
        await CreateApiScopesAsync();
        await CreateClientsAsync();
    }

    private async Task CreateApiScopesAsync()
    {
        await CreateApiScopeAsync("lingyun-abp-application");
    }

    private async Task CreateApiResourcesAsync()
    {
        var commonApiUserClaims = new[]
        {
            "email",
            "email_verified",
            "name",
            "phone_number",
            "phone_number_verified",
            "role"
        };

        await CreateApiResourceAsync("lingyun-abp-application", commonApiUserClaims);
    }

    private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims, IEnumerable<string> secrets = null)
    {
        var apiResource = await _apiResourceRepository.FindByNameAsync(name);
        if (apiResource == null)
        {
            apiResource = await _apiResourceRepository.InsertAsync(
                new ApiResource(
                    _guidGenerator.Create(),
                    name,
                    name + " API"
                ),
                autoSave: true
            );
        }

        foreach (var claim in claims)
        {
            if (apiResource.FindClaim(claim) == null)
            {
                apiResource.AddUserClaim(claim);
            }
        }
        if (secrets != null)
        {
            foreach (var secret in secrets)
            {
                if (apiResource.FindSecret(secret) == null)
                {
                    apiResource.AddSecret(secret);
                }
            }
        }

        return await _apiResourceRepository.UpdateAsync(apiResource);
    }

    private async Task<ApiScope> CreateApiScopeAsync(string name)
    {
        var apiScope = await _apiScopeRepository.FindByNameAsync(name);
        if (apiScope == null)
        {
            apiScope = await _apiScopeRepository.InsertAsync(
                new ApiScope(
                    _guidGenerator.Create(),
                    name,
                    name + " API"
                ),
                autoSave: true
            );
        }

        return apiScope;
    }

    private async Task CreateClientsAsync()
    {

        string commonSecret = IdentityServer4.Models.HashExtensions.Sha256("1q2w3e*");

        var commonScopes = new[]
        {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address",
                "offline_access" // 加上刷新,

            };

        var configurationSection = _configuration.GetSection("IdentityServer:Clients");

        var vueClientId = configurationSection["VueAdmin:ClientId"];
        if (!vueClientId.IsNullOrWhiteSpace())
        {
            var vueClientPermissions = new string[1]
            {
                "AbpIdentity.UserLookup"
            };
            var vueClientRootUrl = configurationSection["VueAdmin:RootUrl"].EnsureEndsWith('/');
            await CreateClientAsync(
                vueClientId,
                commonScopes.Union(new[] { "lingyun-abp-application" }),
                new[] { "password", "client_credentials", "implicit", "phone_verify", "wx-mp" },
                commonSecret,
                redirectUri: $"{vueClientRootUrl}signin-oidc",
                postLogoutRedirectUri: $"{vueClientRootUrl}signout-callback-oidc",
                corsOrigins: configurationSection["CorsOrigins"],
                permissions: vueClientPermissions
            );
        }

        // InternalService 内部服务间通讯客户端,必要的话需要在前端指定它拥有所有权限,当前项目仅预置用户查询权限
        var internalServiceClientId = configurationSection["InternalService:ClientId"];
        if (!internalServiceClientId.IsNullOrWhiteSpace())
        {
            var internalServicePermissions = new string[2]
            {
                "AbpIdentity.UserLookup","AbpIdentity.Users"
            };
            await CreateClientAsync(
                internalServiceClientId,
                commonScopes.Union(new[] { "lingyun-abp-application" }),
                new[] { "client_credentials" },
                commonSecret,
                permissions: internalServicePermissions
            );
        }
    }

    private async Task<Client> CreateClientAsync(
        string name,
        IEnumerable<string> scopes,
        IEnumerable<string> grantTypes,
        string secret,
        string redirectUri = null,
        string postLogoutRedirectUri = null,
        IEnumerable<string> permissions = null,
        string corsOrigins = null)
    {
        var client = await _clientRepository.FindByClientIdAsync(name);
        if (client == null)
        {
            client = await _clientRepository.InsertAsync(
                new Client(
                    _guidGenerator.Create(),
                    name
                )
                {
                    ClientName = name,
                    ProtocolType = "oidc",
                    Description = name,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    AbsoluteRefreshTokenLifetime = 10800, //3 hours
                    AccessTokenLifetime = 7200, //2 hours
                    AuthorizationCodeLifetime = 300,
                    IdentityTokenLifetime = 300,
                    RequireConsent = false
                },
                autoSave: true
            );
        }

        foreach (var scope in scopes)
        {
            if (client.FindScope(scope) == null)
            {
                client.AddScope(scope);
            }
        }

        foreach (var grantType in grantTypes)
        {
            if (client.FindGrantType(grantType) == null)
            {
                client.AddGrantType(grantType);
            }
        }

        if (client.FindSecret(secret) == null)
        {
            client.AddSecret(secret);
        }

        if (redirectUri != null)
        {
            if (client.FindRedirectUri(redirectUri) == null)
            {
                client.AddRedirectUri(redirectUri);
            }
        }

        if (postLogoutRedirectUri != null)
        {
            if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
            {
                client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
            }
        }

        if (corsOrigins != null)
        {
            var corsOriginsSplit = corsOrigins.Split(";");
            foreach (var corsOrigin in corsOriginsSplit)
            {
                if (client.FindCorsOrigin(corsOrigin) == null)
                {
                    client.AddCorsOrigin(corsOrigin);
                }
            }
        }

        if (permissions != null)
        {
            await _permissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, name, permissions);
        }

        return await _clientRepository.UpdateAsync(client);
    }

    #endregion
}
