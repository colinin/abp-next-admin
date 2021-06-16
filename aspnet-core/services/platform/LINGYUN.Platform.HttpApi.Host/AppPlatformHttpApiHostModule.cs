using DotNetCore.CAP;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.Features.LimitValidation.Redis;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.MultiTenancy.DbFinder;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.FileSystem;
using LINGYUN.Abp.OssManagement.FileSystem.ImageSharp;
using LINGYUN.Abp.OssManagement.SettingManagement;
using LINGYUN.Platform.EntityFrameworkCore;
using LINGYUN.Platform.HttpApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Identity;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Security.Encryption;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Platform
{
    [DependsOn(
        // typeof(AbpOssManagementAliyunModule),
        typeof(AbpOssManagementFileSystemModule),           // 本地文件系统提供者模块
        typeof(AbpOssManagementFileSystemImageSharpModule), // 本地文件系统图形处理模块
        typeof(AbpOssManagementApplicationModule),
        typeof(AbpOssManagementHttpApiModule),
        typeof(AbpOssManagementSettingManagementModule),
        typeof(PlatformApplicationModule),
        typeof(PlatformHttpApiModule),
        typeof(PlatformEntityFrameworkCoreModule),
        typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelWebModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpNotificationModule),
        typeof(AbpEmailingExceptionHandlingModule),
        typeof(AbpCAPEventBusModule),
        typeof(AbpFeaturesValidationRedisModule),
        // typeof(AbpFeaturesClientModule),// 当需要客户端特性限制时取消注释此模块
        // typeof(AbpFeaturesValidationRedisClientModule),// 当需要客户端特性限制时取消注释此模块
        typeof(AbpDbFinderMultiTenancyModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAspNetCoreHttpOverridesModule),
        typeof(AbpAutofacModule)
        )]
    public class AppPlatformHttpApiHostModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

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
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = hostingEnvironment.BuildConfiguration();

            // 配置Ef
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });

            //// 中文序列化的编码问题
            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });

            Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = null;
                options.Limits.MaxRequestBufferSize = null;
            });

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = Path.Combine(Directory.GetCurrentDirectory(), "file-blob-storing");
                    });
                });
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
                }
            });

            // 自定义需要处理的异常
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                //  加入需要处理的异常类型
                options.Handlers.Add<Volo.Abp.Data.AbpDbConcurrencyException>();
                options.Handlers.Add<AbpInitializationException>();
                options.Handlers.Add<ObjectDisposedException>();
                options.Handlers.Add<StackOverflowException>();
                options.Handlers.Add<OutOfMemoryException>();
                options.Handlers.Add<System.Data.Common.DbException>();
                options.Handlers.Add<Microsoft.EntityFrameworkCore.DbUpdateException>();
                options.Handlers.Add<System.Data.DBConcurrencyException>();
            });
            // 自定义需要发送邮件通知的异常类型
            Configure<AbpEmailExceptionHandlingOptions>(options =>
            {
                // 是否发送堆栈信息
                options.SendStackTrace = true;
                // 未指定异常接收者的默认接收邮件
                // 指定自己的邮件地址
                // options.DefaultReceiveEmail = "colin.in@foxmail.com";
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

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AppPlatformHttpApiHostModule>("LINGYUN.Platform");
            });

            // 多租户
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = true;
            });

            Configure<AbpAuditingOptions>(options =>
            {
                options.ApplicationName = "Platform";
                // 是否启用实体变更记录
                var entitiesChangedConfig = configuration.GetSection("App:TrackingEntitiesChanged");
                if (entitiesChangedConfig.Exists() && entitiesChangedConfig.Get<bool>())
                {
                    options
                    .EntityHistorySelectors
                    .AddAllEntities();
                }
            });

            // Swagger
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Platform API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "bearer",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT"
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            new string[] { }
                        }
                    });
                });

            // 支持本地化语言类型
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

                options.Resources.AddDynamic();
            });

            Configure<AbpClaimsMapOptions>(options =>
            {
                options.Maps.TryAdd("name", () => AbpClaimTypes.UserName);
            });

            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                    .PersistKeysToStackExchangeRedis(redis, "Platform-Protection-Keys");
            }
        }

        //public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        //{
        //    var backgroundJobManager = context.ServiceProvider.GetRequiredService<IBackgroundJobManager>();
        //    // 五分钟执行一次的定时任务
        //    AsyncHelper.RunSync(async () => await
        //        backgroundJobManager.EnqueueAsync(CronGenerator.Minute(5), new NotificationCleanupExpritionJobArgs(200)));
        //}

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            // http调用链
            app.UseCorrelationId();
            // 虚拟文件系统
            app.UseStaticFiles();
            // 本地化
            app.UseAbpRequestLocalization();
            // 多租户
            app.UseMultiTenancy();
            //路由
            app.UseRouting();
            // 认证
            app.UseAuthentication();
            // jwt
            app.UseJwtTokenMiddleware();
            // 授权
            app.UseAuthorization();
            // Swagger
            app.UseSwagger();
            // Swagger可视化界面
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Platform API");
            });
            // 审计日志
            app.UseAuditing();
            // 路由
            app.UseConfiguredEndpoints();

            if (env.IsDevelopment())
            {
                AsyncHelper.RunSync(async () => 
                    await app.ApplicationServices.GetRequiredService<IDataSeeder>()
                        .SeedAsync());
            }
        }
    }
}
