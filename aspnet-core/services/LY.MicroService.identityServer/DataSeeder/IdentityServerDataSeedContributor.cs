using LINGYUN.Abp.IdentityServer;
using LINGYUN.Abp.IdentityServer.IdentityResources;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
using Volo.Abp.Uow;

namespace LY.MicroService.IdentityServer.DataSeeder;

public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IApiResourceRepository _apiResourceRepository;
    private readonly IApiScopeRepository _apiScopeRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ICustomIdentityResourceDataSeeder _customIdentityResourceDataSeeder;
    private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
    private readonly IWeChatResourceDataSeeder _weChatResourceDataSeeder;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTenant _currentTenant;

    public IdentityServerDataSeedContributor(
        IClientRepository clientRepository,
        IApiScopeRepository apiScopeRepository,
        IPermissionDataSeeder permissionDataSeeder,
        IApiResourceRepository apiResourceRepository,
        IWeChatResourceDataSeeder weChatResourceDataSeeder,
        IIdentityResourceDataSeeder identityResourceDataSeeder,
        ICustomIdentityResourceDataSeeder customIdentityResourceDataSeeder,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
        _clientRepository = clientRepository;
        _permissionDataSeeder = permissionDataSeeder;
        _apiScopeRepository = apiScopeRepository;
        _apiResourceRepository = apiResourceRepository;
        _weChatResourceDataSeeder = weChatResourceDataSeeder;
        _identityResourceDataSeeder = identityResourceDataSeeder;
        _customIdentityResourceDataSeeder = customIdentityResourceDataSeeder;
        _guidGenerator = guidGenerator;
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        _configuration = configuration;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _identityResourceDataSeeder.CreateStandardResourcesAsync();
            await _customIdentityResourceDataSeeder.CreateCustomResourcesAsync();
            await CreateWeChatClaimTypeAsync();
            await CreateApiResourcesAsync();
            await CreateApiScopesAsync();
            await CreateClientsAsync();
        }
    }

    private async Task CreateWeChatClaimTypeAsync()
    {
        await _weChatResourceDataSeeder.CreateStandardResourcesAsync();
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

        //Web Client
        var webClientId = configurationSection["AuthManagement:ClientId"];
        if (!webClientId.IsNullOrWhiteSpace())
        {
            var webClientRootUrl = configurationSection["AuthManagement:RootUrl"].EnsureEndsWith('/');
            await CreateClientAsync(
                webClientId,
                commonScopes.Union(new[] { "lingyun-abp-application" }),
                new[] { "hybrid" },
                commonSecret,
                redirectUri: $"{webClientRootUrl}signin-oidc",
                postLogoutRedirectUri: $"{webClientRootUrl}signout-callback-oidc",
                corsOrigins: configurationSection["CorsOrigins"]
            );
        }

        //Console Test Client
        var consoleClientId = configurationSection["AuthVueAdmin:ClientId"];
        if (!consoleClientId.IsNullOrWhiteSpace())
        {
            await CreateClientAsync(
                consoleClientId,
                commonScopes.Union(new[] { "lingyun-abp-application" }),
                new[] { "password", "client_credentials" },
                commonSecret
            );
        }

        //ApiGateway
        var apigatewayClientId = configurationSection["AuthApiGateway:ClientId"];
        if (!apigatewayClientId.IsNullOrWhiteSpace())
        {
            var apigatewayPermissions = new string[8]
            {
                    "ApiGateway.Global", "ApiGateway.Global.Export",
                    "ApiGateway.Route", "ApiGateway.Route.Export",
                    "ApiGateway.DynamicRoute", "ApiGateway.DynamicRoute.Export",
                    "ApiGateway.AggregateRoute", "ApiGateway.AggregateRoute.Export",
            };
            await CreateClientAsync(
                apigatewayClientId,
                commonScopes.Union(new[] { "lingyun-abp-application" }),
                new[] { "client_credentials" },
                commonSecret,
                permissions: apigatewayPermissions
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
}
