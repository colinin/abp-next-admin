using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.Features.LimitValidation.Redis;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.MultiTenancy.DbFinder;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.FileSystem;
using LINGYUN.Abp.OssManagement.FileSystem.ImageSharp;
using LINGYUN.Abp.OssManagement.SettingManagement;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.UI.Navigation.VueVbenAdmin;
using LINGYUN.Platform;
using LINGYUN.Platform.EntityFrameworkCore;
using LINGYUN.Platform.HttpApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace LY.MicroService.PlatformManagement;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAuditLoggingElasticsearchModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpUINavigationVueVbenAdminModule),
    // typeof(AbpOssManagementAliyunModule),
    typeof(AbpOssManagementFileSystemModule),           // 本地文件系统提供者模块
    typeof(AbpOssManagementFileSystemImageSharpModule), // 本地文件系统图形处理模块
    typeof(AbpOssManagementApplicationModule),
    typeof(AbpOssManagementHttpApiModule),
    typeof(AbpOssManagementSettingManagementModule),
    typeof(PlatformApplicationModule),
    typeof(PlatformHttpApiModule),
    typeof(PlatformEntityFrameworkCoreModule),
    typeof(AbpIdentityHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpNotificationModule),
    typeof(AbpEmailingExceptionHandlingModule),
    typeof(AbpCAPEventBusModule),
    typeof(AbpFeaturesValidationRedisModule),
    // typeof(AbpFeaturesClientModule),// 当需要客户端特性限制时取消注释此模块
    // typeof(AbpFeaturesValidationRedisClientModule),// 当需要客户端特性限制时取消注释此模块
    typeof(AbpDbFinderMultiTenancyModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpAutofacModule)
    )]
public partial class PlatformManagementHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigureApp();
        PreConfigureCAP(configuration);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureDbContext();
        ConfigureBlobStoring();
        ConfigureLocalization();
        ConfigureKestrelServer();
        ConfigureJsonSerializer();
        ConfigreExceptionHandling();
        ConfigureVirtualFileSystem();
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureSwagger(context.Services);
        ConfigureMultiTenancy(configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        // http调用链
        app.UseCorrelationId();
        // 虚拟文件系统
        app.UseStaticFiles();
        // 路由
        app.UseRouting();
        // 跨域
        app.UseCors(DefaultCorsPolicyName);
        // 认证
        app.UseAuthentication();
        // jwt
        app.UseJwtTokenMiddleware();
        // 多租户
        app.UseMultiTenancy();
        // 本地化
        app.UseMapRequestLocalization();
        // 授权
        app.UseAuthorization();
        // Swagger
        app.UseSwagger();
        // Swagger可视化界面
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Platform API");
        });
        // 审计日志
        app.UseAuditing();
        // 记录请求信息
        app.UseAbpSerilogEnrichers();
        // 路由
        app.UseConfiguredEndpoints();

        SeedData(context);
    }
}
