using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.PermissionManagement;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.DataSeeder;
public class OpenIddictDataSeederContributor : OpenIddictDataSeedContributorBase, IDataSeedContributor, ITransientDependency
{
    public ILogger<OpenIddictDataSeederContributor> Logger { protected get; set; }
    protected IPermissionDataSeeder PermissionDataSeeder { get; }
    public OpenIddictDataSeederContributor(
        IConfiguration configuration, 
        IOpenIddictApplicationRepository openIddictApplicationRepository, 
        IAbpApplicationManager applicationManager, 
        IOpenIddictScopeRepository openIddictScopeRepository, 
        IOpenIddictScopeManager scopeManager,
        IPermissionDataSeeder permissionDataSeeder) 
        : base(configuration, openIddictApplicationRepository, applicationManager, openIddictScopeRepository, scopeManager)
    {
        PermissionDataSeeder = permissionDataSeeder;

        Logger = NullLogger<OpenIddictDataSeederContributor>.Instance;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        if (context.TenantId.HasValue)
        {
            return;
        }
        var scope = "lingyun-abp-application";

        Logger.LogInformation("Seeding the default scope...");
        await CreateDefaultScopeAsync();
        await CreateApiScopeAsync(scope, "all_in_one");

        Logger.LogInformation("Seeding the default applications...");
        await CreateApplicationAsync(scope);

        Logger.LogInformation("Seeding default applications completed.");
    }

    private async Task CreateDefaultScopeAsync()
    {
        // OpenID Connect核心scope - 用于标识这是一个OIDC请求
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = OpenIddictConstants.Scopes.OpenId,
            DisplayName = "OpenID Connect", // 友好的显示名称
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "身份认证",
                [CultureInfo.GetCultureInfo("en")] = "OpenID Connect"
            },
            Description = "使用OpenID Connect协议进行身份验证",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序使用您的身份信息进行登录",
                [CultureInfo.GetCultureInfo("en")] = "Allow the application to authenticate you using OpenID Connect"
            }
        });

        // Profile scope - 用户基本信息
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = OpenIddictConstants.Scopes.Profile,
            DisplayName = "个人资料",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "个人资料",
                [CultureInfo.GetCultureInfo("en")] = "Profile"
            },
            Description = "访问您的基本个人资料信息",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序访问您的姓名、头像等基本信息",
                [CultureInfo.GetCultureInfo("en")] = "Allow the application to access your basic profile information like name and picture"
            }
        });

        // Email scope
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = OpenIddictConstants.Scopes.Email,
            DisplayName = "电子邮件",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "电子邮件",
                [CultureInfo.GetCultureInfo("en")] = "Email"
            },
            Description = "访问您的电子邮件地址",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序访问您的电子邮件地址",
                [CultureInfo.GetCultureInfo("en")] = "Allow the application to access your email address"
            }
        });

        // Phone scope
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = OpenIddictConstants.Scopes.Phone,
            DisplayName = "电话号码",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "电话号码",
                [CultureInfo.GetCultureInfo("en")] = "Phone"
            },
            Description = "访问您的电话号码",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序访问您的电话号码",
                [CultureInfo.GetCultureInfo("en")] = "Allow the application to access your phone number"
            }
        });

        // Address scope
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = OpenIddictConstants.Scopes.Address,
            DisplayName = "地址信息",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "地址信息",
                [CultureInfo.GetCultureInfo("en")] = "Address"
            },
            Description = "访问您的地址信息",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序访问您的地址信息",
                [CultureInfo.GetCultureInfo("en")] = "Allow the application to access your address information"
            }
        });

        // Roles scope
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = OpenIddictConstants.Scopes.Roles,
            DisplayName = "角色信息",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "角色信息",
                [CultureInfo.GetCultureInfo("en")] = "Roles"
            },
            Description = "访问您的角色信息",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序访问您的角色和权限信息",
                [CultureInfo.GetCultureInfo("en")] = "Allow the application to access your roles and permissions"
            }
        });

        // Offline Access scope - 用于获取刷新令牌
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = OpenIddictConstants.Scopes.OfflineAccess,
            DisplayName = "离线访问",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "离线访问",
                [CultureInfo.GetCultureInfo("en")] = "Offline Access"
            },
            Description = "在您未登录时访问您的信息",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序在您未登录时访问您的信息",
                [CultureInfo.GetCultureInfo("en")] = "Allow the application to access your information while you are offline"
            }
        });
    }

    private async Task CreateApiScopeAsync(string scope, string apiResource)
    {
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = scope,
            DisplayName = "Single Applications",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "单体应用程序",
                [CultureInfo.GetCultureInfo("en")] = "Single Applications"
            },
            Description = "适用于单体应用程序的接口授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序使用单体应用程序的接口",
                [CultureInfo.GetCultureInfo("en")] = "Allow applications to use the interface of monolithic applications"
            },
            Resources =
            {
                apiResource,
                "http://localhost:30000",
            }
        });
    }

    private async Task CreateApplicationAsync(string scope)
    {
        var configurationSection = Configuration.GetSection("OpenIddict:Applications");
        var vueClientId = configurationSection["VueAdmin:ClientId"];
        if (!vueClientId.IsNullOrWhiteSpace())
        {
            Logger.LogInformation("Seeding application {vueClientId}...", vueClientId);

            var vueClientRootUrls = configurationSection.GetSection("VueAdmin:RootUrls").Get<List<string>>() ?? [];

            var vueClientRedirectUrls = new List<string>();
            var vueClientPostLogoutRedirectUrls = new List<string>();
            vueClientRootUrls.ForEach(url =>
            {
                vueClientRedirectUrls.Add(url.EnsureEndsWith('/'));
                vueClientRedirectUrls.Add(url.EnsureEndsWith('/') + "signin-callback");

                vueClientPostLogoutRedirectUrls.Add(url.EnsureEndsWith('/'));
                vueClientPostLogoutRedirectUrls.Add(url.EnsureEndsWith('/') + "signout-callback");
            });

            await CreateOrUpdateApplicationAsync(
                OpenIddictConstants.ApplicationTypes.Web,
                vueClientId,
                OpenIddictConstants.ClientTypes.Confidential,
                OpenIddictConstants.ConsentTypes.Explicit,
                "Abp Vue Admin Client",
                configurationSection["VueAdmin:ClientSecret"] ?? "1q2w3e*",
                [OpenIddictConstants.GrantTypes.AuthorizationCode,
                OpenIddictConstants.GrantTypes.Implicit,
                OpenIddictConstants.GrantTypes.Password,
                OpenIddictConstants.GrantTypes.RefreshToken],
                [OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Email,
                OpenIddictConstants.Scopes.Roles,
                OpenIddictConstants.Scopes.Address,
                OpenIddictConstants.Scopes.Phone,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OfflineAccess,
                 scope],
                vueClientRedirectUrls,
                vueClientPostLogoutRedirectUrls);

            var vueClientPermissions = new string[1]
            {
                "AbpIdentity.UserLookup"
            };
            await PermissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, vueClientId, vueClientPermissions);
        }

        var internalServiceClientId = configurationSection["InternalService:ClientId"];
        if (!internalServiceClientId.IsNullOrWhiteSpace())
        {
            Logger.LogInformation("Seeding application {internalServiceClientId}...", internalServiceClientId);

            await CreateOrUpdateApplicationAsync(
                OpenIddictConstants.ApplicationTypes.Native,
                internalServiceClientId,
                OpenIddictConstants.ClientTypes.Confidential,
                OpenIddictConstants.ConsentTypes.Explicit,
                "Abp Internal Service Client",
                configurationSection["InternalService:ClientSecret"] ?? "1q2w3e*",
                [OpenIddictConstants.GrantTypes.ClientCredentials],
                [OpenIddictConstants.ResponseTypes.Token, scope]);
        }

        var oauthClientId = configurationSection["VueOAuthClient:ClientId"];
        if (!oauthClientId.IsNullOrWhiteSpace())
        {
            Logger.LogInformation("Seeding application {oauthClientId}...", oauthClientId);

            var oauthClientRootUrls = configurationSection.GetSection("VueOAuthClient:RootUrls").Get<List<string>>() ?? [];

            var oauthClientRedirectUrls = new List<string>();
            var oauthClientPostLogoutRedirectUrls = new List<string>();
            oauthClientRootUrls.ForEach(url =>
            {
                oauthClientRedirectUrls.Add(url.EnsureEndsWith('/'));
                oauthClientRedirectUrls.Add(url.EnsureEndsWith('/') + "signin-callback");
                oauthClientRedirectUrls.Add(url.EnsureEndsWith('/') + "swagger/oauth2-redirect.html");

                oauthClientPostLogoutRedirectUrls.Add(url.EnsureEndsWith('/'));
                oauthClientPostLogoutRedirectUrls.Add(url.EnsureEndsWith('/') + "signout-callback");
            });

            await CreateOrUpdateApplicationAsync(
                OpenIddictConstants.ApplicationTypes.Web,
                oauthClientId,
                OpenIddictConstants.ClientTypes.Public,
                OpenIddictConstants.ConsentTypes.Implicit,
                "Abp OAuth Client",
                null,
                [OpenIddictConstants.GrantTypes.AuthorizationCode,
                 OpenIddictConstants.GrantTypes.RefreshToken],
                [OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Email,
                OpenIddictConstants.Scopes.Roles,
                OpenIddictConstants.Scopes.Address,
                OpenIddictConstants.Scopes.Phone,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OfflineAccess,
                 scope],
                oauthClientRedirectUrls,
                oauthClientPostLogoutRedirectUrls);

            var oauthClientPermissions = new string[1]
            {
                "AbpIdentity.UserLookup"
            };
            await PermissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, oauthClientId, oauthClientPermissions);
        }
    }
}
