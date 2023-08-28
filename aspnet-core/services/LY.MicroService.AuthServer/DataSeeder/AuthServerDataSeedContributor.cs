using LINGYUN.Abp.Identity;
using LINGYUN.Abp.OpenIddict.LinkUser;
using LINGYUN.Abp.OpenIddict.Sms;
using LINGYUN.Abp.OpenIddict.WeChat;
using OpenIddict.Abstractions;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace LY.MicroService.AuthServer.DataSeeder;

public class ServerDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly IIdentityClaimTypeRepository _claimTypeRepository;

    public ServerDataSeedContributor(
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IOpenIddictScopeManager scopeManager,
        IOpenIddictApplicationManager applicationManager,
        IIdentityClaimTypeRepository identityClaimTypeRepository)
    {
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _scopeManager = scopeManager;
        _applicationManager = applicationManager;
        _claimTypeRepository = identityClaimTypeRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (!await _claimTypeRepository.AnyAsync(IdentityConsts.ClaimType.Avatar.Name))
        {
            await _claimTypeRepository.InsertAsync(
                new IdentityClaimType(
                    _guidGenerator.Create(),
                    IdentityConsts.ClaimType.Avatar.Name,
                    isStatic: true
                )
            );
        }

        if (await _scopeManager.FindByNameAsync("lingyun-abp-application") == null)
        {
            await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor()
            {
                Name = "lingyun-abp-application",
                DisplayName = "lingyun-abp-application",
                DisplayNames =
                {
                    [CultureInfo.GetCultureInfo("en")] = "abp application",
                    [CultureInfo.GetCultureInfo("zh-Hans")] = "abp application",
                },
                Resources =
                {
                    "lingyun-abp-application"
                }
            });
        }

        if (await _applicationManager.FindByClientIdAsync("vue-admin-client") == null)
        {
            await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "vue-admin-client",
                ClientSecret = "1q2w3e*",
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "Vue Vben Admin Abp Application",
                PostLogoutRedirectUris =
                {
                    new Uri("https://127.0.0.1:3100/signout-callback-oidc"),
                    new Uri("http://127.0.0.1:3100")
                },
                RedirectUris =
                {
                    new Uri("https://127.0.0.1:3100/signin-oidc"),
                    new Uri("http://127.0.0.1:3100")
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
                    OpenIddictConstants.Permissions.Prefixes.GrantType + WeChatTokenExtensionGrantConsts.OfficialGrantType,
                    OpenIddictConstants.Permissions.Prefixes.GrantType + WeChatTokenExtensionGrantConsts.MiniProgramGrantType,
                    OpenIddictConstants.Permissions.Prefixes.GrantType + SmsTokenExtensionGrantConsts.GrantType,
                    OpenIddictConstants.Permissions.Prefixes.GrantType + LinkUserTokenExtensionGrantConsts.GrantType,

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
                    OpenIddictConstants.Permissions.Prefixes.Scope + WeChatTokenExtensionGrantConsts.ProfileKey,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "lingyun-abp-application"
                }
            });
        }

        if (await _applicationManager.FindByClientIdAsync("InternalServiceClient") == null)
        {
            await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "InternalServiceClient",
                ClientSecret = "1q2w3e*",
                Type = OpenIddictConstants.ClientTypes.Confidential,
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "Internal Service Client",
                PostLogoutRedirectUris = {},
                RedirectUris = {},
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

                    OpenIddictConstants.Permissions.Prefixes.Scope + "lingyun-abp-application"
                }
            });
        }
    }
}
