using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.BackgroundTasks.ExceptionHandling;
using LINGYUN.Abp.BackgroundTasks.Jobs;
using LINGYUN.Abp.BackgroundTasks.Quartz;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.MultiTenancy.DbFinder;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.TaskManagement;
using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace LY.MicroService.TaskManagement;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAuditLoggingElasticsearchModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpDistributedLockingModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpEmailingExceptionHandlingModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpDbFinderMultiTenancyModule),
    typeof(AbpBackgroundTasksJobsModule),
    typeof(AbpBackgroundTasksQuartzModule),
    typeof(AbpBackgroundTasksExceptionHandlingModule),
    typeof(TaskManagementApplicationModule),
    typeof(TaskManagementHttpApiModule),
    typeof(TaskManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAutofacModule)
    )]
public partial class TaskManagementHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigureApp();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureDbContext();
        ConfigureLocalization();
        ConfigureJsonSerializer();
        ConfigureExceptionHandling();
        ConfigureVirtualFileSystem();
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureSwagger(context.Services);
        ConfigureDistributedLock(context.Services, configuration);
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());

        // 开发取消权限检查
        // context.Services.AddAlwaysAllowAuthorization();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseStaticFiles();
        app.UseCorrelationId();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();
        app.UseMultiTenancy();
        app.UseAbpRequestLocalization(options => options.SetDefaultCulture(CultureInfo.CurrentCulture.Name));
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Task Management API");

            var configuration = context.GetConfiguration();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            options.OAuthScopes("TaskManagement");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
