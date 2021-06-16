using DotNetCore.CAP;
using Hangfire;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.SignalR.Protocol.Json;
using LINGYUN.Abp.BackgroundJobs.Hangfire;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling;
using LINGYUN.Abp.ExceptionHandling.Notifications;
using LINGYUN.Abp.Hangfire.Storage.MySql;
using LINGYUN.Abp.IM.SignalR;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.MultiTenancy.DbFinder;
using LINGYUN.Abp.Notifications.SignalR;
using LINGYUN.Abp.Notifications.Sms;
using LINGYUN.Abp.Notifications.WeChat.MiniProgram;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore;
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
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.MessageService
{
    [DependsOn(
        typeof(AbpMessageServiceApplicationModule),
        typeof(AbpMessageServiceHttpApiModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpMessageServiceEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpHangfireMySqlStorageModule),
        typeof(AbpBackgroundJobsHangfireModule),
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpIMSignalRModule),
        typeof(AbpNotificationsSmsModule),
        typeof(AbpNotificationsSignalRModule),
        typeof(AbpNotificationsWeChatMiniProgramModule),
        typeof(AbpNotificationsExceptionHandlingModule),
        typeof(AbpAspNetCoreSignalRProtocolJsonModule),
        typeof(AbpCAPEventBusModule),
        typeof(AbpDbFinderMultiTenancyModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAspNetCoreHttpOverridesModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpMessageServiceHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

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

            Configure<AbpAspNetCoreAuditingOptions>(options =>
            {
                options.IgnoredUrls.AddIfNotContains("/hangfire");
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
                }
            });

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

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpMessageServiceHttpApiHostModule>("LINGYUN.Abp.MessageService");
            });

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

            // Swagger
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MessageService API", Version = "v1" });
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

            // 支持本地化语言类型
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

                options.Resources
                       .Get<MessageServiceResource>()
                       .AddVirtualJson("/Localization/HttpApiHost");
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
                    .PersistKeysToStackExchangeRedis(redis, "MessageService-Protection-Keys");
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            // http调用链
            app.UseCorrelationId();
            // 虚拟文件系统
            app.UseStaticFiles();
            // 本地化
            app.UseAbpRequestLocalization();
            //路由
            app.UseRouting();
            // 跨域
            app.UseCors(DefaultCorsPolicyName);
            // 加入自定义中间件
            app.UseSignalRJwtToken();
            // TODO: 还有没有其他方法在iframe中传递身份令牌?
            app.UseHangfireAuthorication();
            // 认证
            app.UseAuthentication();
            // jwt
            app.UseJwtTokenMiddleware();
            // 授权
            app.UseAuthorization();
            // 多租户
            app.UseMultiTenancy();
            // Swagger
            app.UseSwagger();
            // Swagger可视化界面
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support MessageService API");
            });
            // 审计日志
            app.UseAuditing();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            // 路由
            app.UseConfiguredEndpoints();
        }
    }
}
