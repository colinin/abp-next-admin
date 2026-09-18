using Elsa.Extensions;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.MicroService.WorkflowService;
using LINGYUN.Abp.MicroService.WorkflowService.Components;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.IO;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.IO;
using Volo.Abp.Modularity.PlugIns;

Log.Information("Starting WorkflowService Host...");

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();
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
    }, writeToProviders: true);

builder.AddServiceDefaults();

await builder.AddApplicationAsync<WorkflowServiceModule>(options =>
{
    var applicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME") ?? "WorkflowService";
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
app.UseCors();
app.UseCorrelationId();
app.UseMapRequestLocalization();
app.UseRouting();
app.UseStaticFiles();
app.MapAbpStaticAssets();
app.UseAuthentication();
app.UseJwtTokenMiddleware();
app.UseMultiTenancy();
app.UseAbpSession();
app.UseDynamicClaims();
app.UseAuthorization();
app.UseAntiforgery();
app.UseSwagger();
app.UseAbpSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");

    var configuration = app.Configuration;
    options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
    options.OAuthScopes(configuration["AuthServer:Audience"]);
});
app.UseWorkflowsApi(); // Use Elsa API endpoints.
app.UseWorkflows(); // Use Elsa middleware for HTTP Endpoint activities.
app.UseAuditing();
app.UseAbpSerilogEnrichers();
app.UseConfiguredEndpoints(builder =>
{
    builder.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddAdditionalAssemblies(builder.ServiceProvider.GetRequiredService<IOptions<AbpRouterOptions>>().Value.AdditionalAssemblies.ToArray());
});

await app.RunAsync();