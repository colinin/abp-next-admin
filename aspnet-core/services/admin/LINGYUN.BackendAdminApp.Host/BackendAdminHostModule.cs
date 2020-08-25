using DotNetCore.CAP;
using IdentityModel;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.FileManagement;
using LINGYUN.Abp.Location.Tencent;
using LINGYUN.Abp.MessageService;
using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.TenantManagement;
using LINGYUN.ApiGateway;
using LINGYUN.BackendAdmin.MultiTenancy;
using LINGYUN.Platform;
using LINYUN.Abp.Sms.Aliyun;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Localization;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.Security.Claims;
using Volo.Abp.Security.Encryption;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.BackendAdmin
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AppPlatformApplicationContractModule),
        typeof(ApiGatewayApplicationContractsModule),
        typeof(AbpFileManagementApplicationContractsModule),
        typeof(AbpMessageServiceApplicationContractsModule),
        typeof(LINGYUN.Abp.Identity.AbpIdentityHttpApiModule),
        typeof(LINGYUN.Abp.Identity.AbpIdentityApplicationModule),
        typeof(LINGYUN.Abp.Account.AbpAccountApplicationModule),
        typeof(LINGYUN.Abp.Account.AbpAccountHttpApiModule),
        typeof(LINGYUN.Abp.IdentityServer.AbpIdentityServerApplicationModule),
        typeof(LINGYUN.Abp.IdentityServer.AbpIdentityServerHttpApiModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(LINGYUN.Abp.Identity.EntityFrameworkCore.AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpEmailingExceptionHandlingModule),
        typeof(AbpCAPEventBusModule),
        typeof(AbpAliyunSmsModule),
        typeof(AbpTencentLocationModule),
        typeof(AbpAutofacModule)
        )]
    public class BackendAdminHostModule : AbpModule
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

            PreConfigure<IdentityBuilder>(builder =>
            {
                builder.AddDefaultTokenProviders();
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

            Configure<PermissionManagementOptions>(options =>
            {
                // Rename IdentityServer.Client.ManagePermissions
                // See https://github.com/abpframework/abp/blob/dev/modules/identityserver/src/Volo.Abp.PermissionManagement.Domain.IdentityServer/Volo/Abp/PermissionManagement/IdentityServer/AbpPermissionManagementDomainIdentityServerModule.cs
                options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = 
                    LINGYUN.Abp.IdentityServer.AbpIdentityServerPermissions.Clients.ManagePermissions;
            });

            // 自定义需要处理的异常
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                //  加入需要处理的异常类型
                options.Handlers.Add<AbpException>();
            });
            // 自定义需要发送邮件通知的异常类型
            Configure<AbpEmailExceptionHandlingOptions>(options =>
            {
                // 是否发送堆栈信息
                options.SendStackTrace = true;
                // 未指定异常接收者的默认接收邮件
                options.DefaultReceiveEmail = "colin.in@foxmail.com";
                // 指定某种异常发送到哪个邮件
                options.HandReceivedException<AbpException>("colin.in@foxmail.com");
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
                options.FileSets.AddEmbedded<BackendAdminHostModule>("LINGYUN.BackendAdmin");
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

            // Swagger
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BackendAdmin API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });

            // 支持本地化语言类型
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

                options.Resources
                       .Get<IdentityResource>()
                       .AddVirtualJson("/LINGYUN/BackendAdmin/Identity/Localization");
                options
                    .AddLanguagesMapOrUpdate(
                        "vue-admin-element-ui",
                        new NameValue("zh-Hans", "zh"),
                        new NameValue("en", "en"));
                options
                    .AddLanguageFilesMapOrUpdate(
                        "vue-admin-element-ui",
                        new NameValue("zh-Hans", "zh"),
                        new NameValue("en", "en"));
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

            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisCache:ConnectString"];
                var instanceName = configuration["RedisCache:RedisPrefix"];
                options.InstanceName = instanceName.IsNullOrEmpty() ? "BackendAdmin_Cache" : instanceName;
            });

            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["RedisCache:ConnectString"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "BackendAdmin-Protection-Keys");
            }
        }

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
            // 认证
            app.UseAuthentication();
            // jwt
            app.UseJwtTokenMiddleware();
            // 多租户
            app.UseMultiTenancy();
            // Swagger
            app.UseSwagger();
            // Swagger可视化界面
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support BackendAdmin API");
            });
            // 审计日志
            app.UseAuditing();
            // 处理微信消息
            // app.UseWeChatSignature();
            // 路由
            app.UseConfiguredEndpoints();

            SeedData(context);
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
