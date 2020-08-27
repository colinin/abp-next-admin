using DotNetCore.CAP;
using Hangfire;
using IdentityModel;
using LINGYUN.Abp.BackgroundJobs.Hangfire;
using LINGYUN.Abp.Domain.Entities.Events;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling;
using LINGYUN.Abp.ExceptionHandling.Notifications;
using LINGYUN.Abp.Hangfire.Storage.MySql;
using LINGYUN.Abp.IM.SignalR;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.MessageService.MultiTenancy;
using LINGYUN.Abp.MultiTenancy.DbFinder;
using LINGYUN.Abp.Notifications.SignalR;
using LINGYUN.Abp.Notifications.WeChat.WeApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Text;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore;
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
        typeof(AbpIMSignalRModule),
        typeof(AbpNotificationsSignalRModule),
        typeof(AbpNotificationsWeChatWeAppModule),
        typeof(AbpNotificationsExceptionHandlingModule),
        typeof(AbpCAPEventBusModule),
        typeof(AbpBackgroundJobsHangfireModule),
        typeof(AbpHangfireMySqlStorageModule),
        typeof(AbpDbFinderMultiTenancyModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpDddDomainEntitesEventsModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpMessageServiceHttpApiHostModule : AbpModule
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

            // 加解密
            Configure<AbpStringEncryptionOptions>(options =>
            {
                options.DefaultPassPhrase = "s46c5q55nxpeS8Ra";
                options.InitVectorBytes = Encoding.ASCII.GetBytes("s83ng0abvd02js84");
                options.DefaultSalt = Encoding.ASCII.GetBytes("sf&5)s3#");
            });

            Configure<AbpExceptionHandlingOptions>(options =>
            {
                //  加入需要处理的异常类型
                // options.Handlers.Add<Volo.Abp.Authorization.AbpAuthorizationException>();
            });

            Configure<AbpDistributedCacheOptions>(options =>
            {
                // 滑动过期30天
                options.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromDays(30);
                // 绝对过期60天
                options.GlobalCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
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

            Configure<AbpTenantResolveOptions>(options =>
            {
                options.TenantResolvers.Insert(0, new AuthorizationTenantResolveContributor());
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
                // 单独一个缓存数据库
                var databaseConfig = configuration.GetSection("Redis:DefaultDatabase");
                if (databaseConfig.Exists())
                {
                    redisConfig.DefaultDatabase = databaseConfig.Get<int>();
                }
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

            // 支持本地化语言类型
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

                options.Resources
                       .Get<MessageServiceResource>()
                       .AddVirtualJson("/Localization/HttpApiHost");
            });

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = configuration["AuthServer:ApiName"];
                    AbpClaimTypes.UserId = JwtClaimTypes.Subject;
                    AbpClaimTypes.UserName = JwtClaimTypes.Name;
                    AbpClaimTypes.Role = JwtClaimTypes.Role;
                    AbpClaimTypes.Email = JwtClaimTypes.Email;
                });

            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "MessageService-Protection-Keys");
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
            // http调用链
            app.UseCorrelationId();
            // 虚拟文件系统
            app.UseVirtualFiles();
            // 本地化
            app.UseAbpRequestLocalization();
            //路由
            app.UseRouting();
            // 加入自定义中间件
            app.UseSignalRJwtToken();
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
