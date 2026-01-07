using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.MicroService.AuthServer;
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
    Log.Information("Starting AuthServer Host...");

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

    await builder.AddApplicationAsync<AuthServerModule>(options =>
    {
        var applicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME") ?? "AuthServer";
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
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // app.UseErrorPage();
        app.UseHsts();
    }
    // app.UseHttpsRedirection();
    app.UseCookiePolicy();
    app.UseCorrelationId();
    app.MapAbpStaticAssets();
    app.UseRouting();
    app.UseCors();
    app.UseAuthentication();
    app.UseAbpOpenIddictValidation();
    app.UseMultiTenancy();
    app.UseAbpSession();
    app.UseUnitOfWork();
    app.UseDynamicClaims();
    app.UseAuthorization();
    app.UseAuditing();
    app.UseAbpSerilogEnrichers();
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