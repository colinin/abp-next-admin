using DotNetCore.CAP;
using Elsa.Agents;
using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Persistence.EFCore.Extensions;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;
using Elsa.Secrets.Persistence.EFCore.Extensions;
using Elsa.Secrets.Persistence.EFCore.Sqlite.Extensions;
using Elsa.Studio.Authentication.OpenIdConnect.HttpMessageHandlers;
using Elsa.Studio.Workflows.Designer.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.LocalizationManagement;
using LINGYUN.Abp.MicroService.WorkflowService.Extensions;
using LINGYUN.Abp.MicroService.WorkflowService.Navigation;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using Localization.Resources.AbpUi;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Auditing;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Roles;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.MicroService.WorkflowService;

public partial class WorkflowServiceModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    private void PreConfigureFeature()
    {
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.Editions().EnableAll();
        });
    }

    private void PreConfigureForwardedHeaders()
    {
    }

    private void PreConfigureApp(IConfiguration configuration)
    {
        PreConfigure<AbpSerilogEnrichersUniqueIdOptions>(options =>
        {
            // 以开放端口区别
            options.SnowflakeIdOptions.WorkerId = 29;
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

    private void PreConfigureQuartz(IConfiguration configuration)
    {
        //PreConfigure<AbpQuartzOptions>(options =>
        //{
        //    // 如果使用持久化存储, 则配置quartz持久层
        //    if (configuration.GetSection("Quartz:UsePersistentStore").Get<bool>())
        //    {
        //        var settings = configuration.GetSection("Quartz:Properties").Get<Dictionary<string, string>>();
        //        if (settings != null)
        //        {
        //            foreach (var setting in settings)
        //            {
        //                options.Properties[setting.Key] = setting.Value;
        //            }
        //        }

        //        options.Configurator += (config) =>
        //        {
        //            config.UsePersistentStore(store =>
        //            {
        //                store.UseProperties = false;
        //                store.UseNewtonsoftJsonSerializer();
        //            });
        //        };
        //    }
        //});
    }

    private void ConfigureBackgroundTasks(IServiceCollection services, IConfiguration configuration)
    {
        //Configure<AbpBackgroundTasksOptions>(options =>
        //{
        //    options.NodeName = services.GetApplicationName();
        //});
    }

    private void PreConfigureElsa(IServiceCollection services, IConfiguration configuration)
    {
        PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
        {
            options.IsBlazorWebApp = true;
        });
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseWorkflowManagement(management =>
            {
                management.UseEntityFrameworkCore(ef => ef.UseSqlite());
            });

            elsa.UseWorkflowRuntime(runtime =>
            {
                runtime.UseEntityFrameworkCore(ef => ef.UseSqlite());
            });

            elsa.UseAgentPersistence(agent =>
            {
                agent.UseEntityFrameworkCore(ef => ef.UseSqlite());
            });

            elsa.UseSecrets(secret =>
            {
                secret.UseEntityFrameworkCore(ef => ef.UseSqlite());
            });

            elsa.UseQuartz(quartz =>
            {
                quartz.UseSqlite();
            });

            elsa.UseScheduling(scheduling =>
            {
                scheduling.UseQuartzScheduler();
            });
        });

        PreConfigure<AbpElsaNextStudioBlazorOptions>(options =>
        {
            options.BackendApiConfig.ConfigureHttpClientBuilder = options =>
            {
                options.AuthenticationHandler = typeof(OidcAuthenticatingApiHttpMessageHandler);
                options.ConfigureHttpClient = (_, client) =>
                {
                    // Set a long time out to simplify debugging both Elsa Studio and the Elsa Server backend.
                    client.Timeout = TimeSpan.FromHours(1);
                };
            };
        });

        services.AddRazorComponents()
            .AddInteractiveServerComponents(options =>
            {
                options.RootComponents.RegisterCustomElsaStudioElements();
            });
    }

    private void ConfigureElsa(IServiceCollection services, IConfiguration configuration)
    {
        //Configure<AbpElsaNextShellOptions>(options =>
        //{
        //    options.WithFeature<SqliteWorkflowPersistenceShellFeature>();
        //});

        services.AddOpenIdConnectAuth(options =>
        {
            configuration.GetSection("Authentication:OpenIdConnect").Bind(options);
        });
    }

    private void ConfigureDistributedLock(IServiceCollection services, IConfiguration configuration)
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

    private void ConfigureBlobStoring(IServiceCollection services, IConfiguration configuration)
    {
        var preActions = services.GetPreConfigureActions<AbpBlobStoringOptions>();
        Configure<AbpBlobStoringOptions>(options =>
        {
            preActions.Configure(options);
        });
    }

    private void ConfigureDbContext()
    {
        // 配置Ef
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql();
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

    private void ConfigureAuditing(IConfiguration configuration)
    {
        Configure<AbpAuditingOptions>(options =>
        {
            // 是否启用实体变更记录
            var allEntitiesSelectorIsEnabled = configuration["Auditing:AllEntitiesSelector"];
            if (allEntitiesSelectorIsEnabled.IsNullOrWhiteSpace() ||
                (bool.TryParse(allEntitiesSelectorIsEnabled, out var enabled) && enabled))
            {
                options.EntityHistorySelectors.AddAllEntities();
            }
        });
    }

    private void ConfigureTiming(IConfiguration configuration)
    {
        Configure<AbpClockOptions>(options =>
        {
            configuration.GetSection("Clock").Bind(options);
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

    private void ConfigureMvc(IServiceCollection services, IConfiguration configuration)
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ExposeIntegrationServices = true;
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new UserMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(WorkflowServiceModule).Assembly;
        });
    }

    private void ConfigureVirtualFileSystem()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkflowServiceModule>("LINGYUN.Abp.MicroService.WorkflowService");
        });
    }

    private void ConfigurePermissionManagement()
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.IsDynamicPermissionStoreEnabled = true;
            options.SaveStaticPermissionsToDatabase = true;
        });
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
                    options.AddDomainTenantResolver(domain);
                }
            });
        }
    }

    private void ConfigureIdentity(IConfiguration configuration)
    {
        Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
            options.RemoteRefreshUrl = configuration["App:RefreshClaimsUrl"] + options.RemoteRefreshUrl;
            options.DynamicClaims.Add(Elsa.PermissionNames.ClaimType);
        });
    }

    private void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
    {
        // Swagger
        services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"],
            new Dictionary<string, string>
            {
                { configuration["AuthServer:Audience"], "Workflow Service API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Workflow Service API", Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "colin",
                        Email = "colin.in@foxmail.com",
                        Url = new Uri("https://github.com/colinin")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://github.com/colinin/abp-next-admin/blob/master/LICENSE")
                    }
                });
                options.DocInclusionPredicate((docName, description) =>
                {
                    if (description.TryGetMethodInfo(out var methodInfo))
                    {
                        var controllerNamespace = methodInfo.DeclaringType?.Namespace;
                        if (controllerNamespace?.StartsWith("Elsa") == true)
                        {
                            // TODO: Elsa 2.x 使用 Swashbuckle 6.x版本不兼容, 忽略Swagger文档
                            return false;
                        }
                    }

                    return true;
                });
                options.CustomSchemaIds(type => type.FullName);
                options.DescribeAllParametersInCamelCase();

                var xmlDocFiles = new List<string>();
                xmlDocFiles.AddIfNotContains(Directory.GetFiles(AppContext.BaseDirectory, "LINGYUN.Abp.*.xml"));
                xmlDocFiles.AddIfNotContains(Directory.GetFiles(AppContext.BaseDirectory, "Volo.Abp.*.xml"));

                foreach (var xmlDocFile in xmlDocFiles)
                {
                    options.IncludeXmlComments(xmlDocFile);
                }

                options.SchemaFilter<EnumDescriptionSchemaFilter>();
                options.OperationFilter<TenantHeaderParamter>();
            });
    }

    private void ConfigureLocalization()
    {
        // 支持本地化语言类型
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

            options.Resources.Get<AbpUiResource>()
                .AddVirtualJson("/Localization/Resources");
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

    private void ConfigureSecurity(IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
    {
        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });

        services.AddAuthentication()
            .AddAbpJwtBearer(options =>
            {
                configuration.GetSection("AuthServer").Bind(options);

                var validIssuers = configuration.GetSection("AuthServer:ValidIssuers").Get<List<string>>();
                if (validIssuers?.Count > 0)
                {
                    options.TokenValidationParameters.ValidIssuers = validIssuers;
                    options.TokenValidationParameters.IssuerValidator = TokenWildcardIssuerValidator.IssuerValidator;
                }
                var validAudiences = configuration.GetSection("AuthServer:ValidAudiences").Get<List<string>>();
                if (validAudiences?.Count > 0)
                {
                    options.TokenValidationParameters.ValidAudiences = validAudiences;
                }

                options.Events ??= new JwtBearerEvents();
                var previousOnTokenValidated = options.Events.OnTokenValidated;
                options.Events.OnTokenValidated = async context =>
                {
                    await previousOnTokenValidated(context);

                    if (context.Principal?.Identity?.IsAuthenticated == true)
                    {
                        var roleClaims = context.Principal.FindAll(AbpClaimTypes.Role);
                        if (roleClaims.Any(x => x.Value.Equals(AbpRoleConsts.AdminRoleName, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            context.Principal.AddIdentity(new ClaimsIdentity(
                                new[] { new Claim(Elsa.PermissionNames.ClaimType, Elsa.PermissionNames.All) }));
                        }
                    }
                };
            })
            ;

        services
            .AddDataProtection()
            .SetApplicationName("LINGYUN.Abp.Application")
            .PersistKeysToStackExchangeRedis(() =>
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);

                return redis.GetDatabase();
            },
            "LINGYUN.Abp.Application:DataProtection:Protection-Keys");
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
}
