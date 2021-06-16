using DotNetCore.CAP;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.Identity.EntityFrameworkCore;
using LINGYUN.Abp.IdentityServer;
using LINGYUN.Abp.IdentityServer.EntityFrameworkCore;
using LINGYUN.Abp.IdentityServer.WeChat;
using LINGYUN.Abp.MultiTenancy.DbFinder;
using LINGYUN.Abp.PermissionManagement.Identity;
using LINGYUN.Abp.Sms.Aliyun;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Jwt;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Security.Encryption;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace AuthServer.Host
{
    [DependsOn(
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpIdentityServerSmsValidatorModule),
        typeof(AbpIdentityServerWeChatModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpAspNetCoreHttpOverridesModule),
        typeof(AbpDbFinderMultiTenancyModule),
        typeof(AbpCAPEventBusModule),
        typeof(AbpAliyunSmsModule)
        )]
    public class AuthIdentityServerModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            PreConfigure<CapOptions>(options =>
            {
                options
                .UseMySql(configuration.GetConnectionString("Default"))
                .UseRabbitMQ(rabbitMQOptions =>
                {
                    configuration.GetSection("CAP:RabbitMQ").Bind(rabbitMQOptions);
                })
                .UseDashboard();
            });

            var cerConfig = configuration.GetSection("Certificates");
            if (hostingEnvironment.IsProduction() &&
                cerConfig.Exists())
            {
                // 开发环境下存在证书配置
                // 且证书文件存在则使用自定义的证书文件来启动Ids服务器
                var cerPath = Path.Combine(hostingEnvironment.ContentRootPath, cerConfig["CerPath"]);
                if (File.Exists(cerPath))
                {
                    PreConfigure<AbpIdentityServerBuilderOptions>(options =>
                    {
                        options.AddDeveloperSigningCredential = false;
                    });

                    var cer = new X509Certificate2(cerPath, cerConfig["Password"]);

                    PreConfigure<IIdentityServerBuilder>(builder =>
                    {
                        builder.AddSigningCredential(cer);
                    });
                }
            }
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });

            // 中文序列化的编码问题
            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });

            // 加解密
            Configure<AbpStringEncryptionOptions>(options =>
            {
                var encryptionConfiguration = configuration.GetSection("Encryption");
                if (encryptionConfiguration.Exists())
                {
                    options.DefaultPassPhrase = encryptionConfiguration["PassPhrase"] ?? options.DefaultPassPhrase;
                    options.DefaultSalt = encryptionConfiguration.GetSection("Salt").Exists()
                        ? Encoding.ASCII.GetBytes(encryptionConfiguration["Salt"])
                        : options.DefaultSalt;
                    options.InitVectorBytes = encryptionConfiguration.GetSection("InitVector").Exists() 
                        ? Encoding.ASCII.GetBytes(encryptionConfiguration["InitVector"])
                        : options.InitVectorBytes;

                    var keySizeConfig = encryptionConfiguration.GetSection("Keysize");
                    options.Keysize = keySizeConfig.Exists()
                        ? keySizeConfig.Get<int>()
                        : options.Keysize;
                }
            });

            Configure<AbpDistributedCacheOptions>(options =>
            {
                // 最好统一命名,不然某个缓存变动其他应用服务有例外发生
                options.KeyPrefix = "LINGYUN.Abp.Application";
                // 滑动过期30天
                options.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromDays(30);
                // 绝对过期60天
                options.GlobalCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
            });

            Configure<RedisCacheOptions>(options =>
            {
                var redisConfig = ConfigurationOptions.Parse(options.Configuration);
                options.ConfigurationOptions = redisConfig;
                options.InstanceName = configuration["Redis:InstanceName"];
            });

            // 增加配置文件定义,在新建租户时需要
            Configure<IdentityOptions>(options =>
            {
                var identityConfiguration = configuration.GetSection("Identity");
                if (identityConfiguration.Exists())
                {
                    identityConfiguration.Bind(options);
                }
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AuthIdentityServerModule>("AuthServer");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

                options.Resources
                    .Get<AccountResource>()
                    .AddVirtualJson("/Localization/Resources");
            });

            Configure<AbpAuditingOptions>(options =>
            {
                // options.IsEnabledForGetRequests = true;
                options.ApplicationName = "Identity-Server-STS";
            });

            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                // 邮件登录地址
                options.Applications["MVC"].Urls["EmailVerifyLogin"] = "Account/VerifyCode";
            });

            context.Services.ConfigureNonBreakingSameSiteCookies();

            // context.Services.AddAuthentication();
            context.Services.AddAuthentication()
                    .AddJwtBearer(options =>
                    {
                        options.Authority = configuration["AuthServer:Authority"];
                        options.RequireHttpsMetadata = false;
                        options.Audience = configuration["AuthServer:ApiName"];
                    });

            

            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "AuthServer-Protection-Keys");
            }

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

            context.Services.AddCors(options =>
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
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 需要实现一个错误页面
                app.UseErrorPage();
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseWeChatSignature();
            app.UseMultiTenancy();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();
            app.UseAbpRequestLocalization();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseAuditing();
            app.UseConfiguredEndpoints();

            if (context.GetEnvironment().IsDevelopment())
            {
                SeedData(context);
            }
        }

        private void SeedData(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(async () =>
            {
                using var scope = context.ServiceProvider.CreateScope();
                await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync();
            });
        }
    }
}
