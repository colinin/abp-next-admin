using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.MicroService.WeChatService;
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

Log.Information("Starting WeChatService Host...");

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

await builder.AddApplicationAsync<WeChatServiceModule>(options =>
{
    var applicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME") ?? "WeChatService";
    AbpSerilogEnrichersConsts.ApplicationName = applicationName;
    options.ApplicationName = applicationName;

    var pluginFolder = Path.Combine(Directory.GetCurrentDirectory(), "Modules");
    DirectoryHelper.CreateIfNotExists(pluginFolder);
    options.PlugInSources.AddFolder(pluginFolder, SearchOption.AllDirectories);
});

var app = builder.Build();

await app.InitializeApplicationAsync();

app.MapDefaultEndpoints();

app.UseForwardedHeaders();
app.UseMapRequestLocalization();
app.UseCorrelationId();
app.MapAbpStaticAssets();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseJwtTokenMiddleware();
app.UseMultiTenancy();
app.UseAbpSession();
app.UseDynamicClaims();
app.UseAuthorization();
app.UseSwagger();
app.UseAbpSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Wechat Service API");

    var configuration = app.Configuration;
    options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
    options.OAuthScopes(configuration["AuthServer:Audience"]);
});
app.UseAuditing();
app.UseAbpSerilogEnrichers();
app.UseConfiguredEndpoints();

await app.RunAsync();