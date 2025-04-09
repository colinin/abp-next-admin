using LY.MicroService.Applications.Single;
using Serilog;
using Volo.Abp.IO;
using Volo.Abp.Modularity.PlugIns;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration["App:CorsOrigins"]
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
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
builder.Host.AddAppSettingsSecretsJson()
    .UseAutofac()
    .ConfigureAppConfiguration((context, config) =>
    {
        var dbProvider = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER");
        if (!dbProvider.IsNullOrWhiteSpace())
        {
            config.AddJsonFile($"appsettings.{dbProvider}.json", optional: true);
        }

        var configuration = config.Build();
        if (configuration.GetValue("AgileConfig:IsEnabled", false))
        {
            config.AddAgileConfig(new AgileConfig.Client.ConfigClient(configuration));
        }
    })
    .UseSerilog((context, provider, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration);
    });

await builder.AddApplicationAsync<MicroServiceApplicationsSingleModule>(options =>
{
    MicroServiceApplicationsSingleModule.ApplicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME")
                    ?? MicroServiceApplicationsSingleModule.ApplicationName;
    options.ApplicationName = MicroServiceApplicationsSingleModule.ApplicationName;
    options.Configuration.UserSecretsId = Environment.GetEnvironmentVariable("APPLICATION_USER_SECRETS_ID");
    options.Configuration.UserSecretsAssembly = typeof(MicroServiceApplicationsSingleModule).Assembly;
    var pluginFolder = Path.Combine(Directory.GetCurrentDirectory(), "Modules");
    DirectoryHelper.CreateIfNotExists(pluginFolder);
    options.PlugInSources.AddFolder(pluginFolder,SearchOption.AllDirectories);
});

var app = builder.Build();

await app.InitializeApplicationAsync();

app.UseMapRequestLocalization();

app.UseCookiePolicy();
app.UseForwardedHeaders();
app.UseAbpSecurityHeaders();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
// app.UseAbpExceptionHandling();
app.UseCorrelationId();
app.MapAbpStaticAssets();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAbpOpenIddictValidation();
app.UseMultiTenancy();
app.UseUnitOfWork();
app.UseAbpSession();
app.UseDynamicClaims();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support App API");
});
app.UseAuditing();
app.UseAbpSerilogEnrichers();
app.UseConfiguredEndpoints();

await app.RunAsync();
