using Autofac.Core;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.Claims.Mapping;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Yarp.ReverseProxy.Configuration;

namespace LINGYUN.MicroService.Internal.Gateway;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpClaimsMappingModule),
    typeof(AbpAutofacModule),
    typeof(AbpDataModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAspNetCoreMvcWrapperModule)
)]
public class InternalGatewayModule : AbpModule
{
    public static string ApplicationName { get; set; } = "InternalApiGateway";
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

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ControllersToRemove.Add(typeof(AbpApiDefinitionController));
            options.ControllersToRemove.Add(typeof(AbpApplicationLocalizationController));
            options.ControllersToRemove.Add(typeof(AbpApplicationConfigurationController));
            options.ControllersToRemove.Add(typeof(AbpApplicationConfigurationScriptController));
        });

        context.Services.AddAbpSwaggerGenWithOAuth(
            authority: configuration["AuthServer:Authority"],
            scopes: new Dictionary<string, string>
            {
                {"identity-service", "Identity Service API"},
                {"admin-service", "Admin Service API"},
                {"localization-service", "Localization Service API"},
                {"platform-service", "Platform Service API"},
                {"message-service", "Message Service API"},
                {"task-service", "Task Service API"},
                {"webhook-service", "Webhook Service API"},
                {"wechat-service", "WeChat Service API"},
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
            });

        context.Services.AddDataProtection()
            .SetApplicationName("LINGYUN.Abp.Application")
            .PersistKeysToStackExchangeRedis(() =>
            {
                return ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]).GetDatabase();
            },
            "LINGYUN.Abp.Application:DataProtection:Protection-Keys");

        context.Services.AddCors(options =>
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

        // 认证
        app.UseAuthentication();
        // jwt
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
