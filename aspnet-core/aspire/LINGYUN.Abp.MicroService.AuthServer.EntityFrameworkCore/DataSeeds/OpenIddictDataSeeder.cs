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
        var scope = "lingyun-abp-application";

        Logger.LogInformation("Seeding the default scope...");
        await CreateDefaultScopeAsync();
        await CreateApiScopeAsync(scope);

        Logger.LogInformation("Seeding the default applications...");
        await CreateApplicationAsync(scope);

        Logger.LogInformation("Seeding default applications completed.");
    }

    private async Task CreateDefaultScopeAsync()
    {
        // OpenId Connect
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = OpenIddictConstants.Scopes.OpenId,
            DisplayName = "OpenId Connect",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "身份认证",
                [CultureInfo.GetCultureInfo("en")] = "OpenId Connect"
            },
            Description = "OpenId Connect协议进行身份验证",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序使用您的身份信息进行登录",
                [CultureInfo.GetCultureInfo("en")] = "Allow the application to authenticate you using OpenID Connect"
            }
        });

        // Profile
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

        // Email
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

        // Phone
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

        // Address
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

        // Roles
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

        // OfflineAccess
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

    private async Task CreateApiScopeAsync(string scope)
    {
        // 前端汇总授权范围
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = scope,
            DisplayName = "微服务访问授权",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "微服务访问授权",
                [CultureInfo.GetCultureInfo("en")] = "Single Applications"
            },
            Description = "适用于微服务体系的接口授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "允许应用程序使用各微服务模块的接口",
                [CultureInfo.GetCultureInfo("en")] = "Allow the application to use the interfaces of each microservice module"
            },
            Resources =
            {
                "api-gateway",
                "auth-server",
                "admin-service",
                "identity-service",
                "localization-service",
                "message-service",
                "platform-service",
                "task-service",
                "webhook-service",
                "wechat-service",
                "workflow-service",
            }
        });
        // ApiGateway Swagger
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "api-gateway",
            DisplayName = "Api Gateway",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "应用程序接口网关",
                [CultureInfo.GetCultureInfo("en")] = "Api Gateway"
            },
            Description = "适用于应用程序接口网关Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于应用程序接口网关Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to application Programming interface gateway Swagger authorization"
            },
            Resources =
            {
                "api-gateway",
            }
        });
        // Admin Service
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "admin-service",
            DisplayName = "Admin Service",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "后台管理服务",
                [CultureInfo.GetCultureInfo("en")] = "Admin Service"
            },
            Description = "适用于后台管理服务Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于后台管理服务Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to the back-end management service Swagger authorization"
            },
            Resources =
            {
                "admin-service",
            }
        });
        // Identity Service
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "identity-service",
            DisplayName = "Identity Service",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "身份认证服务",
                [CultureInfo.GetCultureInfo("en")] = "Identity Service"
            },
            Description = "适用于身份认证服务Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于身份认证服务Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to the identity service Swagger authorization"
            },
            Resources =
            {
                "identity-service",
            }
        });
        // Localization Service
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "localization-service",
            DisplayName = "Localization Service",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "本地化管理服务",
                [CultureInfo.GetCultureInfo("en")] = "Localization Service"
            },
            Description = "适用于本地化管理服务Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于本地化管理服务Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to the Localization service Swagger authorization"
            },
            Resources =
            {
                "localization-service",
            }
        });
        // Message Service
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "message-service",
            DisplayName = "Message Service",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "消息管理服务",
                [CultureInfo.GetCultureInfo("en")] = "Message Service"
            },
            Description = "适用于消息管理服务Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于消息管理服务Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to the Message service Swagger authorization"
            },
            Resources =
            {
                "message-service",
            }
        });
        // Platform Service
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "platform-service",
            DisplayName = "Platform Service",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "平台管理服务",
                [CultureInfo.GetCultureInfo("en")] = "Platform Service"
            },
            Description = "适用于平台管理服务Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于平台管理服务Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to the Platform service Swagger authorization"
            },
            Resources =
            {
                "platform-service",
            }
        });
        // Task Service
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "task-service",
            DisplayName = "Task Service",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "任务管理服务",
                [CultureInfo.GetCultureInfo("en")] = "Task Service"
            },
            Description = "适用于任务管理服务Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于任务管理服务Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to the Task service Swagger authorization"
            },
            Resources =
            {
                "task-service",
            }
        });
        // Webhook Service
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "webhook-service",
            DisplayName = "Webhook Service",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "Webhook管理服务",
                [CultureInfo.GetCultureInfo("en")] = "Webhook Service"
            },
            Description = "适用于Webhook管理服务Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于Webhook管理服务Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to the Webhook service Swagger authorization"
            },
            Resources =
            {
                "webhook-service",
            }
        });
        // Wechat Service
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "wechat-service",
            DisplayName = "Wechat Service",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "微信管理服务",
                [CultureInfo.GetCultureInfo("en")] = "Wechat Service"
            },
            Description = "适用于微信管理服务Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于微信管理服务Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to the Wechat service Swagger authorization"
            },
            Resources =
            {
                "wechat-service",
            }
        });
        // Workflow Service
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "workflow-service",
            DisplayName = "Workflow Service",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "工作流管理服务",
                [CultureInfo.GetCultureInfo("en")] = "Workflow Service"
            },
            Description = "适用于工作流管理服务Swagger授权",
            Descriptions =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "适用于工作流管理服务Swagger授权",
                [CultureInfo.GetCultureInfo("en")] = "Applicable to the Workflow service Swagger authorization"
            },
            Resources =
            {
                "workflow-service",
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
                 scope,
                 "api-gateway",
                "auth-server",
                "admin-service",
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
}
