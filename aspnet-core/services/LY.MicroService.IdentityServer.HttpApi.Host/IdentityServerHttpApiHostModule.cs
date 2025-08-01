using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.Mvc.Localization;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using LINGYUN.Abp.Claims.Mapping;
using LINGYUN.Abp.Emailing.Platform;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.Exporter.MiniExcel;
using LINGYUN.Abp.Gdpr;
using LINGYUN.Abp.Gdpr.EntityFrameworkCore;
using LINGYUN.Abp.Gdpr.Identity;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.Sms.Platform;
using LINGYUN.Abp.Telemetry.SkyWalking;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;

namespace LY.MicroService.IdentityServer;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreMvcLocalizationModule),
    typeof(AbpGdprApplicationModule),
    typeof(AbpGdprHttpApiModule),
    typeof(AbpGdprDomainIdentityModule),
    typeof(AbpGdprEntityFrameworkCoreModule),
    typeof(LINGYUN.Abp.Account.AbpAccountApplicationModule),
    typeof(LINGYUN.Abp.Account.AbpAccountHttpApiModule),
    typeof(LINGYUN.Abp.Identity.AbpIdentityApplicationModule),
    typeof(LINGYUN.Abp.Identity.AbpIdentityHttpApiModule),
    typeof(LINGYUN.Abp.IdentityServer.AbpIdentityServerApplicationModule),
    typeof(LINGYUN.Abp.IdentityServer.AbpIdentityServerHttpApiModule),
    typeof(LINGYUN.Abp.Identity.EntityFrameworkCore.AbpIdentityEntityFrameworkCoreModule),
    typeof(LINGYUN.Abp.IdentityServer.EntityFrameworkCore.AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAuthorizationOrganizationUnitsModule),
    typeof(AbpAuditLoggingElasticsearchModule),
    typeof(AbpEmailingExceptionHandlingModule),
    typeof(AbpEmailingPlatformModule),
    typeof(AbpSmsPlatformModule),
    typeof(AbpCAPEventBusModule),
    typeof(AbpHttpClientModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpIdentitySessionAspNetCoreModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpTelemetrySkyWalkingModule),
    typeof(AbpExporterMiniExcelModule),
    typeof(AbpClaimsMappingModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAutofacModule)
    )]
public partial class IdentityServerHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigureWrapper();
        PreConfigureFeature();
        PreForwardedHeaders();
        PreConfigureApp(configuration);
        PreConfigureCAP(configuration);
        PreConfigureIdentity();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureWrapper();
        ConfigureIdentity();
        ConfigureDbContext();
        ConfigureLocalization();
        ConfigureExceptionHandling();
        ConfigureVirtualFileSystem();
        ConfigureFeatureManagement();
        ConfigurePermissionManagement();
        ConfigureUrls(configuration);
        ConfigureTiming(configuration);
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureJsonSerializer(configuration);
        ConfigureMvc(context.Services, configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureSwagger(context.Services, configuration);
        ConfigureDistributedLocking(context.Services, configuration);
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
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
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Identity Service API");

            var configuration = context.GetConfiguration();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthScopes(configuration["AuthServer:Audience"]);
        });
        // 审计日志
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        // 路由
        app.UseConfiguredEndpoints();
    }
}
