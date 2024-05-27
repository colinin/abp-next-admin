using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Yarp.ReverseProxy.Configuration;

namespace LY.MicroService.ApiGateway;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpDataModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAspNetCoreMvcWrapperModule)
)]
public class InternalApiGatewayModule : AbpModule
{
    protected const string ApplicationName = "Services.ApiGateWay";
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AbpSerilogEnrichersConsts.ApplicationName = ApplicationName;

        PreConfigure<AbpSerilogEnrichersUniqueIdOptions>(options =>
        {
            // 以开放端口区别
            options.SnowflakeIdOptions.WorkerId = 19;
            options.SnowflakeIdOptions.WorkerIdBits = 5;
            options.SnowflakeIdOptions.DatacenterId = 1;
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        var showPii = configuration.GetValue<bool>("App:ShowPii");
        IdentityModelEventSource.ShowPII = showPii;


        Configure<InternalApiGatewayOptions>(options =>
        {
            options.Aggregator.ConfigurationUrl.ClientName = "_Abp_Application_Configuration";
            options.Aggregator.ConfigurationUrl.GetUrls.AddIfNotContains(new RequestUrl("http://10.21.15.28:30010"));
            options.Aggregator.ConfigurationUrl.GetUrls.AddIfNotContains(new RequestUrl("http://10.21.15.28:30015"));
            options.Aggregator.ConfigurationUrl.GetUrls.AddIfNotContains(new RequestUrl("http://10.21.15.28:30020"));
            options.Aggregator.ConfigurationUrl.GetUrls.AddIfNotContains(new RequestUrl("http://10.21.15.28:30025"));
            options.Aggregator.ConfigurationUrl.GetUrls.AddIfNotContains(new RequestUrl("http://10.21.15.28:30030"));
            options.Aggregator.ConfigurationUrl.GetUrls.AddIfNotContains(new RequestUrl("http://10.21.15.28:30035"));
            options.Aggregator.ConfigurationUrl.GetUrls.AddIfNotContains(new RequestUrl("http://10.21.15.28:30040"));
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

        context.Services
            .AddReverseProxy()
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
        app.UseAbpSerilogEnrichers();
        app.UseCors();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
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

                options.SwaggerEndpoint($"{clusterGroup.Value.Address}/swagger/v1/swagger.json", $"{routeConfig.RouteId} API");
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            }
        });

        app.UseRewriter(new RewriteOptions().AddRedirect("^(|\\|\\s+)$", "/swagger"));

        app.UseRouting();
        app.UseConfiguredEndpoints(endpoints =>
        {
            endpoints.MapReverseProxy(options =>
                options.UseLoadBalancing());
        });
    }
}
