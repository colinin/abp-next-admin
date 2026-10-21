using LINGYUN.Abp.MicroService.AllInOne;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddAppSettingsSecretsJson()
    .UseAutofac()
    .ConfigureAppConfiguration((context, builder) =>
    {
        var env = context.HostingEnvironment.EnvironmentName;
        var dbProvider = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER");
        if (!dbProvider.IsNullOrWhiteSpace() && !env.IsNullOrWhiteSpace())
        {
            builder.AddJsonFile($"appsettings.{env}.{dbProvider}.json", optional: true);
        }

        if (context.Configuration.GetValue("AgileConfig:IsEnabled", false))
        {
            builder.AddAgileConfig(new AgileConfig.Client.ConfigClient(context.Configuration));
        }
    })
    .UseSerilog((context, provider, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration);
    });

await builder.AddApplicationAsync<AllInOneModule>(options =>
{
    AllInOneModule.ApplicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME")
                    ?? AllInOneModule.ApplicationName;
    options.ApplicationName = AllInOneModule.ApplicationName;
    options.Configuration.UserSecretsId = Environment.GetEnvironmentVariable("APPLICATION_USER_SECRETS_ID");
    options.Configuration.UserSecretsAssembly = typeof(AllInOneModule).Assembly;
    var pluginFolder = Path.Combine(Directory.GetCurrentDirectory(), "Modules");
    DirectoryHelper.CreateIfNotExists(pluginFolder);
    options.PlugInSources.AddFolder(pluginFolder,SearchOption.AllDirectories);
});

var app = builder.Build();

await app.InitializeApplicationAsync();

app.UseForwardedHeaders();
app.UseMapRequestLocalization();
app.UseCookiePolicy();
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
app.UseDynamicClaims();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseAbpSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Single APP API");

        options.OAuthClientId(app.Configuration["AuthServer:SwaggerClientId"]);
        options.OAuthScopes(app.Configuration["AuthServer:Audience"]);
    });
}
app.UseAuditing();
app.UseAbpSerilogEnrichers();
app.UseConfiguredEndpoints();
// app.UseHttpActivities();

await app.RunAsync();
