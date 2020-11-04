using LINGYUN.Abp.IdentityServer;
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
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace AuthServer.DataSeeder
{
    public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
        private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IWeChatResourceDataSeeder _weChatResourceDataSeeder;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IConfiguration _configuration;

        public IdentityServerDataSeedContributor(
            IClientRepository clientRepository,
            IPermissionDataSeeder permissionDataSeeder,
            IApiResourceRepository apiResourceRepository,
            IWeChatResourceDataSeeder weChatResourceDataSeeder,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IIdentityClaimTypeRepository identityClaimTypeRepository,
            IGuidGenerator guidGenerator)
        {
            _clientRepository = clientRepository;
            _permissionDataSeeder = permissionDataSeeder;
            _apiResourceRepository = apiResourceRepository;
            _identityClaimTypeRepository = identityClaimTypeRepository;
            _weChatResourceDataSeeder = weChatResourceDataSeeder;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
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
            await _identityResourceDataSeeder.CreateStandardResourcesAsync();
            await CreateApiResourcesAsync();
            await CreateClientsAsync();
            await CreateWeChatClaimTypeAsync();
        }

        private async Task CreateWeChatClaimTypeAsync()
        {
            await _weChatResourceDataSeeder.CreateStandardResourcesAsync();
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

            await CreateApiResourceAsync("auth-service", commonApiUserClaims);

            var apigatewaySecret = new[]
            {
                "defj98734htgrb90365D23"
            };
            await CreateApiResourceAsync("apigateway-service", commonApiUserClaims.Union(new[] { "apigateway-service" }), apigatewaySecret);
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

        private async Task CreateClientsAsync()
        {
            const string commonSecret = "E5Xd4yMqjP5kjWFKrYgySBju6JVfCzMyFp7n2QmMrME=";

            var commonScopes = new[]
            {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address",
                "offline_access" // 加上刷新
            };

            var configurationSection = _configuration.GetSection("IdentityServer:Clients");

            //Web Client
            var webClientId = configurationSection["AuthManagement:ClientId"];
            if (!webClientId.IsNullOrWhiteSpace())
            {
                var webClientRootUrl = configurationSection["AuthManagement:RootUrl"].EnsureEndsWith('/');
                await CreateClientAsync(
                    webClientId,
                    commonScopes.Union(new[] { "auth-service" }),
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
                    commonScopes.Union(new[] { "auth-service", "apigateway-service" }),
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
                    commonScopes.Union(new[] { "apigateway-service" }),
                    new[] { "client_credentials" },
                    commonSecret,
                    permissions: apigatewayPermissions
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
            var client = await _clientRepository.FindByCliendIdAsync(name);
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

            if(permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, name, permissions);
            }

            return await _clientRepository.UpdateAsync(client);
        }
    }
}
