using LY.MicroService.Applications.Single;
using Microsoft.AspNetCore.Cors;
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
    .UseSerilog((context, provider, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration);
    });

await builder.AddApplicationAsync<MicroServiceApplicationsSingleModule>(options =>
{
    // 从环境变量取用户机密配置, 适用于容器测试
    options.Configuration.UserSecretsId = Environment.GetEnvironmentVariable("APPLICATION_USER_SECRETS_ID");
    // 如果容器没有指定用户机密, 从项目读取
    options.Configuration.UserSecretsAssembly = typeof(MicroServiceApplicationsSingleModule).Assembly;
    // 搜索 Modules 目录下所有文件作为插件
    // 取消显示引用所有其他项目的模块，改为通过插件的形式引用
    var pluginFolder = Path.Combine(
            Directory.GetCurrentDirectory(), "Modules");
    DirectoryHelper.CreateIfNotExists(pluginFolder);
    options.PlugInSources.AddFolder(
        pluginFolder,
        SearchOption.AllDirectories);
});

var app = builder.Build();

await app.InitializeApplicationAsync();

app.UseForwardedHeaders();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCookiePolicy();
app.UseMapRequestLocalization();
app.UseCorrelationId();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAbpClaimsMap();
app.UseDynamicClaims();
app.UseAbpOpenIddictValidation();
app.UseMultiTenancy();
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
