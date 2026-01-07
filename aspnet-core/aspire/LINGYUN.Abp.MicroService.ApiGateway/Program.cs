using LINGYUN.Abp.MicroService.ApiGateway;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.IO;
using Volo.Abp.Modularity.PlugIns;
using Yarp.ReverseProxy.Configuration;

Log.Information("Starting ApiGateway Host...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.AddAppSettingsSecretsJson()
        .UseAutofac()
        .ConfigureAppConfiguration((context, config) =>
        {
            if (context.Configuration.GetValue("AgileConfig:IsEnabled", false))
            {
                config.AddAgileConfig(new AgileConfig.Client.ConfigClient(context.Configuration));
            }
            config.AddJsonFile("yarp.json", optional: true, reloadOnChange: true);
        })
        .UseSerilog((context, provider, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration);
        });

    builder.AddServiceDefaults();

    await builder.AddApplicationAsync<ApiGatewayModule>(options =>
    {
        var applicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME") ?? "ApiGateway";
        options.ApplicationName = applicationName;
        AbpSerilogEnrichersConsts.ApplicationName = applicationName;

        var pluginFolder = Path.Combine(Directory.GetCurrentDirectory(), "Modules");
        DirectoryHelper.CreateIfNotExists(pluginFolder);
        options.PlugInSources.AddFolder(pluginFolder, SearchOption.AllDirectories);
    });

    var app = builder.Build();

    await app.InitializeApplicationAsync();

    app.MapDefaultEndpoints();

    if (app.Environment.IsDevelopment())
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

        var configuration = app.Services.GetRequiredService<IConfiguration>();
        var logger = app.Services.GetRequiredService<ILogger<ApplicationInitializationContext>>();
        var proxyConfigProvider = app.Services.GetRequiredService<IProxyConfigProvider>();
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

            if (clusterGroup.Value.Metadata != null &&
                clusterGroup.Value.Metadata.TryGetValue("SwaggerEndpoint", out var address) &&
                !address.IsNullOrWhiteSpace())
            {
                options.SwaggerEndpoint($"{address}/swagger/v1/swagger.json", $"{routeConfig.RouteId} API");
            }
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
        }
    });

    // app.UseRewriter(new RewriteOptions().AddRedirect("^(|\\|\\s+)$", "/swagger"));

    app.UseRouting();
    app.UseAuditing();
    app.UseAbpSerilogEnrichers();
    app.UseWebSockets();
    app.UseWebSocketsTelemetry();
    app.UseConfiguredEndpoints(endpoints =>
    {
        endpoints.MapReverseProxy();
    });

    await app.RunAsync();
}
catch (Exception ex)
{
    if (ex is HostAbortedException)
    {
        throw;
    }

    Log.Fatal(ex, "Host terminated unexpectedly!");
}
finally
{
    await Log.CloseAndFlushAsync();
}