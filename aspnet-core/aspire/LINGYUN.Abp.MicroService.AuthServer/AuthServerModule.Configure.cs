using DotNetCore.CAP;
using LINGYUN.Abp.AspNetCore.MultiTenancy;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.LocalizationManagement;
using LINGYUN.Abp.MicroService.AuthServer.Ui.Branding;
using LINGYUN.Abp.OpenIddict.AspNetCore.Session;
using LINGYUN.Abp.OpenIddict.LinkUser;
using LINGYUN.Abp.OpenIddict.Portal;
using LINGYUN.Abp.OpenIddict.Sms;
using LINGYUN.Abp.OpenIddict.WeChat;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.Wrapper;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching;
using Volo.Abp.FeatureManagement;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Http.Client;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.WildcardDomains;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.MicroService.AuthServer;

public partial class AuthServerModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();

    private void PreConfigureFeature()
    {
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.Editions().EnableAll();
        });
    }

    private void PreForwardedHeaders()
    {
    }

    private void PreConfigureApp(IConfiguration configuration)
    {
        PreConfigure<AbpSerilogEnrichersUniqueIdOptions>(options =>
        {
            // 以开放端口区别，应在0-31之间
            options.SnowflakeIdOptions.WorkerId = 1;
            options.SnowflakeIdOptions.WorkerIdBits = 5;
            options.SnowflakeIdOptions.DatacenterId = 1;
        });

        if (configuration.GetValue<bool>("App:ShowPii"))
        {
            IdentityModelEventSource.ShowPII = true;
        }
    }

    private void PreConfigureCAP(IConfiguration configuration)
    {
        PreConfigure<CapOptions>(options =>
        {
            options
                .UsePostgreSql(mySqlOptions =>
                {
                    configuration.GetSection("CAP:PostgreSql").Bind(mySqlOptions);
                })
                .UseRabbitMQ(rabbitMQOptions =>
                {
                    configuration.GetSection("CAP:RabbitMQ").Bind(rabbitMQOptions);
                })
                .UseDashboard();
        });
    }

    private void PreConfigureAuthServer()
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                //options.AddAudiences("lingyun-abp-application");

                options.UseLocalServer();

                options.UseAspNetCore();

                options.UseDataProtection();
            });
        });
    }

    private void PreConfigureCertificate(IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(builder =>
            {
                builder.AddProductionEncryptionAndSigningCertificate(configuration["App:SslFile"], configuration["App:SslPassword"]);
            });
        }

        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            // builder.UseDataProtection();

            // 禁用https
            builder.UseAspNetCore()
                .DisableTransportSecurityRequirement();
        });

        var wildcardDomains = configuration.GetSection("App:WildcardDomains").Get<List<string>>();
        if (wildcardDomains?.Count > 0)
        {
            PreConfigure<AbpOpenIddictWildcardDomainOptions>(options =>
            {
                options.EnableWildcardDomainSupport = true;
                options.WildcardDomainsFormat.AddIfNotContains(wildcardDomains);
            });
        }
    }

    private void ConfigureMvc(IServiceCollection services, IConfiguration configuration)
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ExposeIntegrationServices = true;
        });
    }

    private void ConfigureDataSeeder()
    {

    }

    private void ConfigureFeatureManagement()
    {
        Configure<FeatureManagementOptions>(options =>
        {
            options.IsDynamicFeatureStoreEnabled = true;
        });
    }

    private void ConfigurePermissionManagement()
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.SaveStaticPermissionsToDatabase = false;
        });
    }

    private void ConfigureSettingManagement()
    {
        Configure<SettingManagementOptions>(options =>
        {
            options.IsDynamicSettingStoreEnabled = true;
        });
    }

    private void ConfigureJsonSerializer(IConfiguration configuration)
    {
        // 统一时间日期格式
        Configure<AbpJsonOptions>(options =>
        {
            var jsonConfiguration = configuration.GetSection("Json");
            if (jsonConfiguration.Exists())
            {
                jsonConfiguration.Bind(options);
            }
        });
        // 中文序列化的编码问题
        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        });
    }

    private void ConfigureDistributedLocking(IServiceCollection services, IConfiguration configuration)
    {
        var distributedLockEnabled = configuration["DistributedLock:IsEnabled"];
        if (distributedLockEnabled.IsNullOrEmpty() || bool.Parse(distributedLockEnabled))
        {
            services.AddSingleton<IDistributedLockProvider>(sp =>
            {
                var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return new RedisDistributedSynchronizationProvider(connectionMultiplexer.GetDatabase());
            });
        }
    }

    private void ConfigureBranding(IConfiguration configuration)
    {
        Configure<AccountBrandingOptions>(options =>
        {
            configuration.GetSection("App:Branding").Bind(options);
        });
    }

    private void ConfigureCaching(IConfiguration configuration)
    {
        Configure<AbpDistributedCacheOptions>(options =>
        {
            configuration.GetSection("DistributedCache").Bind(options);
        });

        Configure<RedisCacheOptions>(options =>
        {
            var redisConfig = ConfigurationOptions.Parse(options.Configuration);
            options.ConfigurationOptions = redisConfig;
            options.InstanceName = configuration["Redis:InstanceName"];
        });
    }
    private void ConfigureIdentity(IConfiguration configuration)
    {
        // 增加配置文件定义,在新建租户时需要
        Configure<IdentityOptions>(options =>
        {
            var identityConfiguration = configuration.GetSection("Identity");
            if (identityConfiguration.Exists())
            {
                identityConfiguration.Bind(options);
            }
        });
        Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
            options.IsRemoteRefreshEnabled = false;
        });

        Configure<AbpOpenIddictAspNetCoreSessionOptions>(options =>
        {
            options.PersistentSessionGrantTypes.Add(SmsTokenExtensionGrantConsts.GrantType);
            options.PersistentSessionGrantTypes.Add(PortalTokenExtensionGrantConsts.GrantType);
            options.PersistentSessionGrantTypes.Add(LinkUserTokenExtensionGrantConsts.GrantType);
            options.PersistentSessionGrantTypes.Add(WeChatTokenExtensionGrantConsts.OfficialGrantType);
            options.PersistentSessionGrantTypes.Add(WeChatTokenExtensionGrantConsts.MiniProgramGrantType);
            options.PersistentSessionGrantTypes.Add(AbpWeChatWorkGlobalConsts.GrantType);
        });
    }
    private void ConfigureVirtualFileSystem()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AuthServerModule>("LINGYUN.Abp.MicroService.AuthServer");
        });
    }
    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
        });

        Configure<AbpLocalizationCultureMapOptions>(options =>
        {
            var zhHansCultureMapInfo = new CultureMapInfo
            {
                TargetCulture = "zh-Hans",
                SourceCultures = new string[] { "zh", "zh_CN", "zh-CN" }
            };

            options.CulturesMaps.Add(zhHansCultureMapInfo);
            options.UiCulturesMaps.Add(zhHansCultureMapInfo);
        });

        Configure<AbpLocalizationManagementOptions>(options =>
        {
            options.SaveStaticLocalizationsToDatabase = true;
        });
    }
    private void ConfigureTiming(IConfiguration configuration)
    {
        Configure<AbpClockOptions>(options =>
        {
            configuration.GetSection("Clock").Bind(options);
        });
    }

    private void ConfigureAuditing(IConfiguration configuration)
    {
        Configure<AbpAuditingOptions>(options =>
        {
            // options.IsEnabledForGetRequests = true;
            // 是否启用实体变更记录
            var allEntitiesSelectorIsEnabled = configuration["Auditing:AllEntitiesSelector"];
            if (allEntitiesSelectorIsEnabled.IsNullOrWhiteSpace() ||
                (bool.TryParse(allEntitiesSelectorIsEnabled, out var enabled) && enabled))
            {
                options.EntityHistorySelectors.AddAllEntities();
            }
        });
    }
    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            var applicationConfiguration = configuration.GetSection("App:Urls:Applications");
            foreach (var appConfig in applicationConfiguration.GetChildren())
            {
                options.Applications[appConfig.Key].RootUrl = appConfig["RootUrl"];
                foreach (var urlsConfig in appConfig.GetSection("Urls").GetChildren())
                {
                    options.Applications[appConfig.Key].Urls[urlsConfig.Key] = urlsConfig.Value;
                }
            }
        });
    }

    private void ConfigureAuthServer(IConfiguration configuration)
    {
        Configure<OpenIddictServerAspNetCoreBuilder>(builder =>
        {
            builder.DisableTransportSecurityRequirement();
        });

        Configure<OpenIddictServerAspNetCoreOptions>(options =>
        {
            options.DisableTransportSecurityRequirement = true;
        });

        Configure<OpenIddictServerOptions>(options =>
        {
            var lifetime = configuration.GetSection("OpenIddict:Lifetime");
            options.AuthorizationCodeLifetime = lifetime.GetValue("AuthorizationCode", options.AuthorizationCodeLifetime);
            options.AccessTokenLifetime = lifetime.GetValue("AccessToken", options.AccessTokenLifetime);
            options.DeviceCodeLifetime = lifetime.GetValue("DeviceCode", options.DeviceCodeLifetime);
            options.IdentityTokenLifetime = lifetime.GetValue("IdentityToken", options.IdentityTokenLifetime);
            options.RefreshTokenLifetime = lifetime.GetValue("RefreshToken", options.RefreshTokenLifetime);
            options.RefreshTokenReuseLeeway = lifetime.GetValue("RefreshTokenReuseLeeway", options.RefreshTokenReuseLeeway);
            options.UserCodeLifetime = lifetime.GetValue("UserCode", options.UserCodeLifetime);
        });
    }
    private void ConfigureSecurity(IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
    {
        services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        services
            .AddAuthentication()
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
            })
            .AddJwtBearer(options =>
            {
                configuration.GetSection("AuthServer").Bind(options);

                var validIssuers = configuration.GetSection("AuthServer:ValidIssuers").Get<List<string>>();
                if (validIssuers?.Count > 0)
                {
                    options.TokenValidationParameters.ValidIssuers = validIssuers;
                    options.TokenValidationParameters.IssuerValidator = TokenWildcardIssuerValidator.IssuerValidator;
                }
            });
        //.AddWeChatWork(options =>
        //{
        //    options.SignInScheme = IdentityConstants.ExternalScheme;
        //});

        services
            .AddDataProtection()
            .SetApplicationName("LINGYUN.Abp.Application")
            .PersistKeysToStackExchangeRedis(() =>
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);

                return redis.GetDatabase();
            },
            "LINGYUN.Abp.Application:DataProtection:Protection-Keys");

        services.AddSameSiteCookiePolicy();
    }
    private void ConfigureMultiTenancy(IConfiguration configuration)
    {
        // 多租户
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });

        var tenantResolveCfg = configuration.GetSection("App:Domains");
        if (tenantResolveCfg.Exists())
        {
            Configure<AbpTenantResolveOptions>(options =>
            {
                var domains = tenantResolveCfg.Get<string[]>();
                foreach (var domain in domains)
                {
                    options.AddOnlyDomainTenantResolver(domain);
                }
            });
        }
    }
    private void ConfigureCors(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                var corsOrigins = configuration.GetSection("App:CorsOrigins").Get<List<string>>();
                if (corsOrigins == null || corsOrigins.Count == 0)
                {
                    corsOrigins = configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToList() ?? new List<string>();
                }
                builder
                    .WithOrigins(corsOrigins
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .WithAbpWrapExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void PreConfigureWrapper()
    {
        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            // http服务间调用发送不需要包装结果的请求头
            options.ProxyClientActions.Add(
                (_, _, client) =>
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(AbpHttpWrapConsts.AbpDontWrapResult, "true");
                });
        });
    }
}
