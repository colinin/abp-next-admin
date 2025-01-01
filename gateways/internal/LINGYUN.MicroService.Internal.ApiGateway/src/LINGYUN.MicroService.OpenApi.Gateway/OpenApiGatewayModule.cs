﻿using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.OpenApi.Authorization;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Yarp.ReverseProxy.Configuration;

namespace LINGYUN.MicroService.OpenApi.Gateway;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAutofacModule),
    typeof(AbpDataModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpOpenApiAuthorizationModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpIdentitySessionAspNetCoreModule)
)]
public class OpenApiGatewayModule : AbpModule
{
    public static string ApplicationName { get; set; } = "Services.OpenApi.GateWay";
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AbpSerilogEnrichersConsts.ApplicationName = ApplicationName;

        PreConfigure<AbpSerilogEnrichersUniqueIdOptions>(options =>
        {
            // 以开放端口区别
            options.SnowflakeIdOptions.WorkerId = 0;
            options.SnowflakeIdOptions.WorkerIdBits = 5;
            options.SnowflakeIdOptions.DatacenterId = 1;
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        Configure<AbpWrapperOptions>(options =>
        {
            options.IsEnabled = true;
        });
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ControllersToRemove.Add(typeof(AbpApiDefinitionController));
            options.ControllersToRemove.Add(typeof(AbpApplicationLocalizationController));
            options.ControllersToRemove.Add(typeof(AbpApplicationConfigurationController));
            options.ControllersToRemove.Add(typeof(AbpApplicationConfigurationScriptController));
        });

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

        context.Services.AddAbpSwaggerGenWithOAuth(
            authority: configuration["AuthServer:Authority"],
            scopes: new Dictionary<string, string>
            {
                {"Account", "Account API"},
                {"Identity", "Identity API"},
                {"IdentityServer", "Identity Server API"},
                {"BackendAdmin", "Backend Admin API"},
                {"Localization", "Localization API"},
                {"Platform", "Platform API"},
                {"RealtimeMessage", "RealtimeMessage API"},
                {"TaskManagement", "Task Management API"},
                {"Webhooks", "Webhooks API"},
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGateway", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    configuration.GetSection("AuthServer").Bind(options);
                });

        if (hostingEnvironment.IsProduction())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            context.Services
                .AddDataProtection()
                .SetApplicationName("LINGYUN.Abp.Application")
                .PersistKeysToStackExchangeRedis(redis, "LINGYUN.Abp.Application:DataProtection:Protection-Keys");
        }

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
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
        context.Services.AddWebSockets(options =>
        {

        });
        context.Services.AddHttpForwarder();
        context.Services.AddTelemetryListeners();

        context.Services
            .AddReverseProxy()
            .ConfigureHttpClient((context, handler) =>
            {
                handler.ActivityHeadersPropagator = null;
            })
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseCorrelationId();
        app.UseCors();

        // api签名
        app.UseOpenApiAuthorization();
        // 认证
        app.UseAuthentication();
        // 会话
        app.UseAbpSession();
        // 令牌
        app.UseDynamicClaims();
        // 授权
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Open API Document");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = context.ServiceProvider.GetRequiredService<ILogger<ApplicationInitializationContext>>();
            var proxyConfigProvider = context.ServiceProvider.GetRequiredService<IProxyConfigProvider>();
            var yarpConfig = proxyConfigProvider.GetConfig();

            var routedClusters = yarpConfig.Clusters
                .SelectMany(t => t.Destinations,
                    (clusterId, destination) => new { clusterId.ClusterId, destination.Value });

            var groupedClusters = routedClusters
                .GroupBy(q => q.Value.Address)
                .Select(t => t.First())
                .Distinct()
                .ToList();

            foreach (var clusterGroup in groupedClusters)
            {
                var routeConfig = yarpConfig.Routes.FirstOrDefault(q =>
                    q.ClusterId == clusterGroup.ClusterId);
                if (routeConfig == null)
                {
                    logger.LogWarning($"Swagger UI: Couldn't find route configuration for {clusterGroup.ClusterId}...");
                    continue;
                }

                var swaggerEndpoint = clusterGroup.Value.Address;
                if (clusterGroup.Value.Metadata != null &&
                    clusterGroup.Value.Metadata.TryGetValue("SwaggerEndpoint", out var address) &&
                    !address.IsNullOrWhiteSpace())
                {
                    swaggerEndpoint = address;
                }

                options.SwaggerEndpoint($"{swaggerEndpoint}/swagger/v1/swagger.json", $"{routeConfig.RouteId} API");
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            }
        });

        // app.UseRewriter(new RewriteOptions().AddRedirect("^(|\\|\\s+)$", "/swagger"));

        app.UseRouting();
        app.UseAuditing();
        app.UseWebSockets();
        app.UseWebSocketsTelemetry();
        app.UseConfiguredEndpoints(endpoints =>
        {
            endpoints.MapReverseProxy();
        });
        app.UseAbpSerilogEnrichers();
    }
}
