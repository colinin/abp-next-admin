using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.MicroService.AuthServer.DataSeeds;
public class OpenIddictDataSeeder : OpenIddictDataSeedContributorBase, ITransientDependency
{
    public ILogger<OpenIddictDataSeeder> Logger { protected get; set; }
    protected IPermissionDataSeeder PermissionDataSeeder { get; }
    public OpenIddictDataSeeder(
        IConfiguration configuration,
        IOpenIddictApplicationRepository openIddictApplicationRepository,
        IAbpApplicationManager applicationManager,
        IOpenIddictScopeRepository openIddictScopeRepository,
        IOpenIddictScopeManager scopeManager,
        IPermissionDataSeeder permissionDataSeeder)
        : base(configuration, openIddictApplicationRepository, applicationManager, openIddictScopeRepository, scopeManager)
    {
        PermissionDataSeeder = permissionDataSeeder;

        Logger = NullLogger<OpenIddictDataSeeder>.Instance;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        if (context.TenantId.HasValue)
        {
            return;
        }
        var applicationScope = "lingyun-abp-application";

        Logger.LogInformation("Seeding the default scope...");
        await CreateDefaultScopeAsync();
        await CreateApiScopesAsync(applicationScope);

        Logger.LogInformation("Seeding the default applications...");
        await CreateApplicationAsync(applicationScope);

        Logger.LogInformation("Seeding default applications completed.");
    }

    private async Task CreateDefaultScopeAsync()
    {
        // OpenId Connect
        await CreateServiceScopeAsync(
            OpenIddictConstants.Scopes.OpenId,
            "OpenId Connect",
            "身份认证",
            "OpenId Connect",
            "OpenId Connect协议进行身份验证",
            "允许应用程序使用您的身份信息进行登录",
            "Allow the application to authenticate you using OpenID Connect");

        // Profile
        await CreateServiceScopeAsync(
            OpenIddictConstants.Scopes.Profile,
            "个人资料",
            "个人资料",
            "Profile",
            "访问您的基本个人资料信息",
            "允许应用程序访问您的姓名、头像等基本信息",
            "Allow the application to access your basic profile information like name and picture");

        // Email
        await CreateServiceScopeAsync(
            OpenIddictConstants.Scopes.Email,
            "电子邮件",
            "电子邮件",
            "Email",
            "访问您的电子邮件地址",
            "允许应用程序访问您的电子邮件地址",
            "Allow the application to access your email address");

        // Phone
        await CreateServiceScopeAsync(
            OpenIddictConstants.Scopes.Phone,
            "电话号码",
            "电话号码",
            "Phone",
            "访问您的电话号码",
            "允许应用程序访问您的电话号码",
            "Allow the application to access your phone number");

        // Address
        await CreateServiceScopeAsync(
            OpenIddictConstants.Scopes.Address,
            "地址信息",
            "地址信息",
            "Address",
            "访问您的地址信息",
            "允许应用程序访问您的地址信息",
            "Allow the application to access your address information");

        // Roles
        await CreateServiceScopeAsync(
            OpenIddictConstants.Scopes.Roles,
            "角色信息",
            "角色信息",
            "Roles",
            "访问您的角色信息",
            "允许应用程序访问您的角色和权限信息",
            "Allow the application to access your roles and permissions");

        // OfflineAccess
        await CreateServiceScopeAsync(
            OpenIddictConstants.Scopes.OfflineAccess,
            "离线访问",
            "离线访问",
            "Offline access",
            "在您未登录时访问您的信息",
            "允许应用程序在您未登录时访问您的信息",
            "Allow the application to access your information while you are offline");
    }

    private async Task CreateApiScopesAsync(string scope)
    {
        // 前端汇总授权范围
        await CreateServiceScopeAsync(
            scope,
            "微服务访问授权",
            "微服务访问授权",
            "MicroService applications access",
            "适用于微服务体系的接口授权",
            "允许应用程序使用各微服务模块的接口",
            "Allow the application to use the interfaces of each microservice module",
            [
                "api-gateway",
                "auth-server",
                "admin-service",
                "ai-service",
                "identity-service",
                "localization-service",
                "message-service",
                "platform-service",
                "task-service",
                "webhook-service",
                "wechat-service",
                "workflow-service"
             ]);
        // ApiGateway Swagger
        await CreateServiceScopeAsync(
            "api-gateway",
            "Api Gateway",
            "应用程序接口网关",
            "Api Gateway",
            "适用于应用程序接口网关Swagger授权",
            "适用于应用程序接口网关Swagger授权",
            "Applicable to application Programming interface gateway Swagger authorization",
            ["api-gateway"]);
        // Admin Service
        await CreateServiceScopeAsync(
            "admin-service",
            "Admin Service",
            "后台管理服务",
            "Admin Service",
            "适用于后台管理服务Swagger授权",
            "适用于后台管理服务Swagger授权",
            "Applicable to the back-end management service Swagger authorization",
            ["admin-service"]);
        // Auth Server
        await CreateServiceScopeAsync(
            "auth-server",
            "Auth Server",
            "身份认证服务器",
            "Auth Server",
            "适用于身份认证服务器Swagger授权",
            "适用于身份认证服务器Swagger授权",
            "Applicable to the auth server Swagger authorization",
            ["auth-server"]);
        // Identity Service
        await CreateServiceScopeAsync(
            "identity-service",
            "Identity Service",
            "身份认证服务",
            "Identity Service",
            "适用于身份认证服务Swagger授权",
            "适用于身份认证服务Swagger授权",
            "Applicable to the identity service Swagger authorization",
            ["identity-service"]);
        // Localization Service
        await CreateServiceScopeAsync(
            "localization-service",
            "Localization Service",
            "本地化管理服务",
            "Localization Service",
            "适用于本地化管理服务Swagger授权",
            "适用于本地化管理服务Swagger授权",
            "Applicable to the Localization service Swagger authorization",
            ["localization-service"]);
        // Message Service
        await CreateServiceScopeAsync(
            "message-service",
            "Message Service",
            "消息管理服务",
            "Message Service",
            "适用于消息管理服务Swagger授权",
            "适用于消息管理服务Swagger授权",
            "Applicable to the Message service Swagger authorization",
            ["message-service"]);
        // Platform Service
        await CreateServiceScopeAsync(
            "platform-service",
            "Platform Service",
            "平台管理服务",
            "Platform Service",
            "适用于平台管理服务Swagger授权",
            "适用于平台管理服务Swagger授权",
            "Applicable to the Platform service Swagger authorization",
            ["platform-service"]);
        // Task Service
        await CreateServiceScopeAsync(
            "task-service",
            "Task Service",
            "任务管理服务",
            "Task Service",
            "适用于任务管理服务Swagger授权",
            "适用于任务管理服务Swagger授权",
            "Applicable to the Task service Swagger authorization",
            ["task-service"]);
        // Webhook Service
        await CreateServiceScopeAsync(
            "webhook-service",
            "Webhook Service",
            "Webhook管理服务",
            "Webhook Service",
            "适用于Webhook管理服务Swagger授权",
            "适用于Webhook管理服务Swagger授权",
            "Applicable to the Webhook service Swagger authorization",
            ["webhook-service"]);
        // Wechat Service
        await CreateServiceScopeAsync(
            "wechat-service",
            "Wechat Service",
            "微信管理服务",
            "Wechat Service",
            "适用于微信管理服务Swagger授权",
            "适用于微信管理服务Swagger授权",
            "Applicable to the Wechat service Swagger authorization",
            ["wechat-service"]);
        // Workflow Service
        await CreateServiceScopeAsync(
            "workflow-service",
            "Workflow Service",
            "工作流管理服务",
            "Workflow Service",
            "适用于工作流管理服务Swagger授权",
            "适用于工作流管理服务Swagger授权",
            "Applicable to the Workflow service Swagger authorization",
            ["workflow-service"]);
        // AI Service
        await CreateServiceScopeAsync(
            "ai-service",
            "Artificial Intelligence Service",
            "AI管理服务",
            "Artificial Intelligence Service",
            "适用于AI管理服务Swagger授权",
            "适用于AI管理服务Swagger授权",
            "Applicable to the Artificial Intelligence service Swagger authorization",
            ["ai-service"]);
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
                OpenIddictConstants.GrantTypes.Password,
                OpenIddictConstants.GrantTypes.RefreshToken,
                // TODO: 引用项目?
                "link_user",
                "impersonation"],
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
                OpenIddictConstants.ApplicationTypes.Web,
                internalServiceClientId,
                OpenIddictConstants.ClientTypes.Confidential,
                OpenIddictConstants.ConsentTypes.Implicit,
                "Abp Internal Service Client",
                configurationSection["InternalService:ClientSecret"] ?? "1q2w3e*",
                [OpenIddictConstants.GrantTypes.ClientCredentials],
                [scope]);
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
                 OpenIddictConstants.GrantTypes.RefreshToken,
                "link_user",
                "impersonation"],
                [OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Email,
                OpenIddictConstants.Scopes.Roles,
                OpenIddictConstants.Scopes.Address,
                OpenIddictConstants.Scopes.Phone,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OfflineAccess,
                 scope,
                 "api-gateway",
                "auth-server",
                "admin-service",
                "ai-service",
                "identity-service",
                "localization-service",
                "message-service",
                "platform-service",
                "task-service",
                "webhook-service",
                "wechat-service",
                "workflow-service"],
                oauthClientRedirectUrls,
                oauthClientPostLogoutRedirectUrls);

            var oauthClientPermissions = new string[1]
            {
                "AbpIdentity.UserLookup"
            };
            await PermissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, oauthClientId, oauthClientPermissions);
        }
    }

    private async Task CreateServiceScopeAsync(
        string name, 
        string displayNameDefault,
        string displayNameZh, 
        string displayNameEn,
        string descriptionDefault,
        string descriptionZh, 
        string descriptionEn,
        List<string>? resources = null)
    {
        var descriptor = new OpenIddictScopeDescriptor
        {
            Name = name,
            DisplayName = displayNameDefault,
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = displayNameZh,
                [CultureInfo.GetCultureInfo("en")] = displayNameEn
            },
            Description = descriptionDefault,
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = descriptionZh,
                [CultureInfo.GetCultureInfo("en")] = descriptionEn
            },
        };
        if (resources is { Count: > 0 })
        {
            descriptor.Resources.AddIfNotContains(resources);
        }

        await CreateScopesAsync(descriptor);
    }
}
