using DotNetCore.CAP;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using Medallion.Threading.Redis;
using Medallion.Threading;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Validation.AspNetCore;
using StackExchange.Redis;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp.Account.Localization;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using LINGYUN.Abp.Account;
using Microsoft.IdentityModel.Logging;
using TencentCloud.Emr.V20190103.Models;
using LINGYUN.Abp.AspNetCore.HttpOverrides.Forwarded;
using Microsoft.AspNetCore.HttpOverrides;

namespace LY.MicroService.AuthServer;

public partial class AuthServerModule
{
    private const string ApplicationName = "AuthServer";
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
        PreConfigure<AbpForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
    }

    private void PreConfigureApp(IConfiguration configuration)
    {
        AbpSerilogEnrichersConsts.ApplicationName = ApplicationName;

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
            .UseMySql(mySqlOptions =>
            {
                configuration.GetSection("CAP:MySql").Bind(mySqlOptions);
            })
            .UseRabbitMQ(rabbitMQOptions =>
            {
                configuration.GetSection("CAP:RabbitMQ").Bind(rabbitMQOptions);
            })
            .UseDashboard();
        });
    }

    private void PreConfigureAuth()
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                //options.AddAudiences("lingyun-abp-application");

                options.UseLocalServer();

                options.UseAspNetCore();
            });
        });
    }

    private void PreConfigureCertificate(IConfiguration configuration, IWebHostEnvironment environment)
    {
        var cerConfig = configuration.GetSection("Certificates");
        if (environment.IsProduction() &&
            cerConfig.Exists())
        {
            // 开发环境下存在证书配置
            // 且证书文件存在则使用自定义的证书文件来启动Ids服务器
            var cerPath = Path.Combine(environment.ContentRootPath, cerConfig["CerPath"]);
            if (File.Exists(cerPath))
            {
                PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
                {
                    //https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                    options.AddDevelopmentEncryptionAndSigningCertificate = false;
                });

                var cer = new X509Certificate2(cerPath, cerConfig["Password"]);

                PreConfigure<OpenIddictServerBuilder>(builder =>
                {
                    //https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html

                    builder.AddSigningCertificate(cer);

                    builder.AddEncryptionCertificate(cer);
                });
            }
        }
        else
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                //https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(builder =>
            {
                //https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                using (var algorithm = RSA.Create(keySizeInBits: 2048))
                {
                    var subject = new X500DistinguishedName("CN=Fabrikam Encryption Certificate");
                    var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));
                    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
                    builder.AddSigningCertificate(certificate);
                }

                using (var algorithm = RSA.Create(keySizeInBits: 2048))
                {
                    var subject = new X500DistinguishedName("CN=Fabrikam Signing Certificate");
                    var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));
                    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
                    builder.AddEncryptionCertificate(certificate);
                }

                // 禁用https
                builder.UseAspNetCore()
                    .DisableTransportSecurityRequirement();
            });
        }
    }

    private void ConfigureDbContext()
    {
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
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
            var redis = ConnectionMultiplexer.Connect(configuration["DistributedLock:Redis:Configuration"]);
            services.AddSingleton<IDistributedLockProvider>(_ => new RedisDistributedSynchronizationProvider(redis.GetDatabase()));
        }
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
    }
    private void ConfigureVirtualFileSystem()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AuthServerModule>("LY.MicroService.AuthServer");
        });
    }
    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

            options.Resources
                .Get<AccountResource>()
                .AddVirtualJson("/Localization/Resources");

            options.UsePersistence<AccountResource>();
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
    }
    private void ConfigureAuditing(IConfiguration configuration)
    {
        Configure<AbpAuditingOptions>(options =>
        {
            // options.IsEnabledForGetRequests = true;
            options.ApplicationName = ApplicationName;
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
    private void ConfigureSecurity(IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
    {
        //services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        //services.AddAuthentication()
        //        .AddJwtBearer(options =>
        //        {
        //            options.Authority = configuration["AuthServer:Authority"];
        //            options.RequireHttpsMetadata = false;
        //            options.Audience = configuration["AuthServer:ApiName"];
        //        });

        if (isDevelopment)
        {
            // services.AddAlwaysAllowAuthorization();
        }

        if (!isDevelopment)
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            services
                .AddDataProtection()
                .SetApplicationName("LINGYUN.Abp.Application")
                .PersistKeysToStackExchangeRedis(redis, "LINGYUN.Abp.Application:DataProtection:Protection-Keys");
        }

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
                    options.AddDomainTenantResolver(domain);
                }
            });
        }
    }
    private void ConfigureCors(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicyName, builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    // 引用 LINGYUN.Abp.AspNetCore.Mvc.Wrapper 包时可替换为 WithAbpWrapExposedHeaders
                    .WithExposedHeaders("_AbpWrapResult", "_AbpDontWrapResult")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }
}
