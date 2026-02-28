using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.MicroService.LocalizationService;
using LINGYUN.Abp.MicroService.ServiceDefaults;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using Volo.Abp.IO;
using Volo.Abp.Modularity.PlugIns;

try
{
    Log.Information("Starting LocalizationService Host...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.AddAppSettingsSecretsJson()
        .UseAutofac()
        .ConfigureAppConfiguration((context, config) =>
        {
            if (context.Configuration.GetValue("AgileConfig:IsEnabled", false))
            {
                config.AddAgileConfig(new AgileConfig.Client.ConfigClient(context.Configuration));
            }
        })
        .UseSerilog((context, provider, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration);
        });

    builder.AddServiceDefaults();
    builder.AddCustomHealthChecks<ServiceHealthCheck>("Service");

    await builder.AddApplicationAsync<LocalizationServiceModule>(options =>
    {
        var applicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME") ?? "LocalizationService";
        AbpSerilogEnrichersConsts.ApplicationName = applicationName;
        options.ApplicationName = applicationName;

        var pluginFolder = Path.Combine(Directory.GetCurrentDirectory(), "Modules");
        DirectoryHelper.CreateIfNotExists(pluginFolder);
        options.PlugInSources.AddFolder(pluginFolder, SearchOption.AllDirectories);
    });

    var app = builder.Build();

    await app.InitializeApplicationAsync();

    app.MapDefaultEndpoints();
    app.MapCustomHealthChecks("/health/service");

    app.UseForwardedHeaders();
    // 本地化
    app.UseMapRequestLocalization();
    // http调用链
    app.UseCorrelationId();
    // 虚拟文件系统
    app.MapAbpStaticAssets();
    // 路由
    app.UseRouting();
    // 跨域
    app.UseCors();
    // 认证
    app.UseAuthentication();
    app.UseJwtTokenMiddleware();
    // 多租户
    app.UseMultiTenancy();
    // 会话
    app.UseAbpSession();
    app.UseDynamicClaims();
    // 授权
    app.UseAuthorization();
    // Swagger
    app.UseSwagger();
    // Swagger可视化界面
    app.UseAbpSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Localization Service API");

        var configuration = app.Configuration;
        options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
        options.OAuthScopes(configuration["AuthServer:Audience"]);
    });
    // 审计日志
    app.UseAuditing();
    app.UseAbpSerilogEnrichers();
    // 路由
    app.UseConfiguredEndpoints();

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