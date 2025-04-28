using Elsa.Extensions;
using Elsa.Studio.Contracts;
using Elsa.Studio.Localization.BlazorServer.Extensions;
using LY.MicroService.WorkflowManagement.Next;
using Serilog;
using Volo.Abp.IO;
using Volo.Abp.Modularity.PlugIns;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddAppSettingsSecretsJson()
    .UseAutofac()
    .ConfigureAppConfiguration((context, config) =>
    {
        var agileConfigEnabled = context.Configuration["AgileConfig:IsEnabled"];
        if (context.Configuration.GetSection("AgileConfig").Exists() &&
            (agileConfigEnabled.IsNullOrWhiteSpace() || bool.Parse(agileConfigEnabled)))
        {
            config.AddAgileConfig(new AgileConfig.Client.ConfigClient(context.Configuration));
        }
    })
    .UseSerilog((context, provider, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration);
    });

await builder.AddApplicationAsync<WorkflowManagementNextHttpApiHostModule>(options =>
{
    options.ApplicationName = "AbpElsaNextWebModule";
    // 从环境变量取用户机密配置, 适用于容器测试
    options.Configuration.UserSecretsId = Environment.GetEnvironmentVariable("APPLICATION_USER_SECRETS_ID");
    // 如果容器没有指定用户机密, 从项目读取
    options.Configuration.UserSecretsAssembly = typeof(WorkflowManagementNextHttpApiHostModule).Assembly;
    // 搜索 Modules 目录下所有文件作为插件
    // 取消显示引用所有其他项目的模块，改为通过插件的形式引用
    var pluginFolder = Path.Combine(Directory.GetCurrentDirectory(), "Modules");

    DirectoryHelper.CreateIfNotExists(pluginFolder);

    options.PlugInSources.AddFolder(pluginFolder, SearchOption.AllDirectories);
});

// Build the web application.
var app = builder.Build();

await app.InitializeApplicationAsync();

// Run each startup task.
var startupTaskRunner = app.Services.GetRequiredService<IStartupTaskRunner>();
await startupTaskRunner.RunStartupTasksAsync();

// Configure web application's middleware pipeline.
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression();

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors();
app.UseForwardedHeaders();
app.UseCorrelationId();
app.UseElsaLocalization();
app.UseStaticFiles();
app.UseResponseCompression();
app.UseRouting(); // Required for SignalR.
app.UseAuthentication();
app.UseJwtTokenMiddleware();
app.UseMultiTenancy();
app.UseDynamicClaims();
app.UseAuthorization();
app.MapBlazorHub();
app.UseWorkflowsApi(); // Use Elsa API endpoints.
app.UseWorkflows(); // Use Elsa middleware to handle HTTP requests mapped to HTTP Endpoint activities.
app.UseWorkflowsSignalRHubs(); // Optional SignalR integration. Elsa Studio uses SignalR to receive real-time updates from the server. 
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");
});

app.UseAuditing();
app.UseAbpSerilogEnrichers();
app.UseConfiguredEndpoints();

app.MapFallbackToPage("/_Host");

await app.RunAsync();
