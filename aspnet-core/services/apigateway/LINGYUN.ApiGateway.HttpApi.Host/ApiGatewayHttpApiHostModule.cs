using DotNetCore.CAP.Messages;
using IdentityModel;
using LINGYUN.ApiGateway.EntityFrameworkCore;
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
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace LINGYUN.ApiGateway
{
    [DependsOn(
        typeof(ApiGatewayApplicationModule),
        typeof(ApiGatewayEntityFrameworkCoreModule),
        typeof(ApiGatewayHttpApiModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpAutofacModule)
        )]
    public class ApiGatewayHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = hostingEnvironment.BuildConfiguration();
            // 配置Ef
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });

            // 多租户
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = false;
            });

            //Configure<AbpTenantResolveOptions>(options =>
            //{
            //    options.TenantResolvers.Insert(0, new AuthorizationTenantResolveContributor());
            //});

            // Swagger
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGateway API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });

            // 支持本地化语言类型
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Host"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = configuration["AuthServer:ApiName"];
                    options.ApiSecret = configuration["AuthServer:ApiSecret"];
                    AbpClaimTypes.UserId = JwtClaimTypes.Subject;
                    AbpClaimTypes.UserName = JwtClaimTypes.Name;
                    AbpClaimTypes.Role = JwtClaimTypes.Role;
                    AbpClaimTypes.Email = JwtClaimTypes.Email;
                });

            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisCache:ConnectString"];
                var instanceName = configuration["RedisCache:RedisPrefix"];
                options.InstanceName = instanceName.IsNullOrEmpty() ? "ApiGateway_Cache" : instanceName;
            });

            context.Services.AddCap(x =>
            {
                x.UseEntityFramework<ApiGatewayDbContext>();

                x.UseDashboard();

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = configuration["CAP:RabbitMQ:Connect:Host"];
                    cfg.VirtualHost = configuration["CAP:RabbitMQ:Connect:VirtualHost"];
                    cfg.UserName = configuration["CAP:RabbitMQ:Connect:UserName"];
                    cfg.Password = configuration["CAP:RabbitMQ:Connect:Password"];
                    cfg.ExchangeName = configuration["CAP:RabbitMQ:Connect:ExchangeName"];
                    cfg.Port = configuration.GetValue<int>("CAP:RabbitMQ:Connect:Port");
                });

                x.FailedRetryCount = 5;

                x.FailedThresholdCallback = (type, message) =>
                {
                    // 是订阅者的错误
                    if (type == MessageType.Subscribe)
                    {
                        Console.WriteLine(message);
                    }
                };
            });

            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["RedisCache:ConnectString"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "ApiGateway-Protection-Keys");
            }

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<ApiGatewayHttpApiHostAutoMapperProfile>(validate: true);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            // http调用链
            app.UseCorrelationId();
            // 虚拟文件系统
            app.UseVirtualFiles();
            //路由
            app.UseRouting();
            // 认证
            app.UseAuthentication();
            // 多租户
            // app.UseMultiTenancy();
            // 本地化
            app.UseAbpRequestLocalization();
            // Swagger
            app.UseSwagger();
            // Swagger可视化界面
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support ApiGateway API");
            });
            // 审计日志
            app.UseAuditing();
            // 路由
            app.UseConfiguredEndpoints();
        }
    }
}
