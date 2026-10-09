using LINGYUN.Abp.AI.Ollama;
using LINGYUN.Abp.AI.Tools;
using LINGYUN.Abp.AI.Tools.Http;
using LINGYUN.Abp.AI.Tools.Mcp;
using LINGYUN.Abp.AIManagement;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.AspNetCore.Session;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.AspNetCore.Authentication;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Dynamic.Definitions;
using LINGYUN.Abp.Emailing.Platform;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.Logging.Serilog.Elasticsearch;
using LINGYUN.Abp.MicroService.AIService.EntityFrameworkCore;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.Sms.Platform;
using LINGYUN.Abp.TextTemplating.Scriban;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.Swashbuckle;

namespace LINGYUN.Abp.MicroService.AIService;

[DependsOn(
    typeof(AbpCAPEventBusModule),
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpLoggingSerilogElasticsearchModule),
    typeof(AbpAuditLoggingElasticsearchModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    typeof(AbpTextTemplatingScribanModule),
    typeof(AbpAIOllamaModule),
    typeof(AbpAIToolsModule),
    typeof(AbpAIToolsHttpModule),
    typeof(AbpAIToolsMcpModule),
    typeof(AbpAIManagementApplicationModule),
    typeof(AbpAIManagementHttpApiModule),
    typeof(AIServiceMigrationsEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpEmailingExceptionHandlingModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpSmsPlatformModule),
    typeof(AbpEmailingPlatformModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpAspNetCoreSessionModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpDynamicDefinitionsModule),
    typeof(AbpAspNetCoreAuthenticationModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAutofacModule)
    )]
public partial class AIServiceModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigureWrapper();
        PreConfigureFeature();
        PreConfigureApp(configuration);
        PreConfigureCAP(configuration);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureWrapper();
        ConfigureLocalization();
        ConfigureVirtualFileSystem();
        ConfigureTextTemplating();
        ConfigureAIManagement();
        ConfigureSettingManagement();
        ConfigureFeatureManagement();
        ConfigurePermissionManagement();
        ConfigureIdentity(configuration);
        ConfigureTiming(configuration);
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureJsonSerializer(configuration);
        ConfigureMvc(context.Services, configuration);
        ConfigureCors(context.Services, configuration);
        if (hostingEnvironment.IsDevelopment())
        {
            ConfigureSwagger(context.Services, configuration);
        }
        ConfigureDistributedLocking(context.Services, configuration);
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var configuration = context.GetConfiguration();
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseForwardedHeaders();
        // 本地化
        app.UseMapRequestLocalization();
        // http调用链
        app.UseCorrelationId();
        // 文件系统
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
        // jwt
        app.UseDynamicClaims();
        // 授权
        app.UseAuthorization();

        if (env.IsDevelopment())
        {
            // Swagger
            app.UseSwagger();
            // Swagger可视化界面
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support AI Service API");

                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthScopes(configuration["AuthServer:Audience"]);
            });
        }
        // 审计日志
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        // 路由
        app.UseConfiguredEndpoints();
    }
}

