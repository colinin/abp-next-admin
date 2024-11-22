using Elsa;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.Mvc.Localization;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using LINGYUN.Abp.BackgroundTasks.DistributedLocking;
using LINGYUN.Abp.BackgroundTasks.ExceptionHandling;
using LINGYUN.Abp.BackgroundTasks.Quartz;
using LINGYUN.Abp.BlobStoring.OssManagement;
using LINGYUN.Abp.Claims.Mapping;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Elsa;
using LINGYUN.Abp.Elsa.Activities;
using LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;
using LINGYUN.Abp.Elsa.Notifications;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.Http.Client.Wrapper;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.NewtonsoftJson;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TextTemplating.Scriban;

namespace LY.MicroService.WorkflowManagement;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAuditLoggingElasticsearchModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpBlobStoringOssManagementModule),
    typeof(AbpElsaModule),
    typeof(AbpElsaServerModule),
    typeof(AbpElsaActivitiesModule),
    typeof(AbpElsaNotificationsModule),
    typeof(AbpEmailingExceptionHandlingModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreMvcLocalizationModule),
    typeof(AbpBackgroundTasksQuartzModule),
    typeof(AbpBackgroundTasksDistributedLockingModule),
    typeof(AbpBackgroundTasksExceptionHandlingModule),
    typeof(TaskManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpElsaEntityFrameworkCoreMySqlModule),
    typeof(AbpAuthorizationOrganizationUnitsModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpTextTemplatingScribanModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpCAPEventBusModule),
    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpHttpClientWrapperModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpMailKitModule),
    typeof(AbpClaimsMappingModule),
    typeof(AbpAspNetCoreMvcNewtonsoftModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpIdentitySessionAspNetCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class WorkflowManagementHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        PreConfigureFeature();
        PreConfigureForwardedHeaders();
        PreConfigureApp(configuration);
        PreConfigureCAP(configuration);
        PreConfigureQuartz(configuration);
        PreConfigureElsa(context.Services, configuration);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureDbContext();
        ConfigureLocalization();
        ConfigureExceptionHandling();
        ConfigureVirtualFileSystem();
        ConfigureTiming(configuration);
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureIdentity(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureBackgroundTasks(configuration);
        ConfigureSwagger(context.Services);
        ConfigureEndpoints(context.Services);
        ConfigureMvc(context.Services, configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureBlobStoring(context.Services, configuration);
        ConfigureOpenTelemetry(context.Services, configuration);
        ConfigureDistributedLock(context.Services, configuration);
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());

        context.Services.AddRazorPages();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        app.UseForwardedHeaders();
        // 本地化
        app.UseMapRequestLocalization();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(DefaultCorsPolicyName);
        app.UseElsaFeatures();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();
        app.UseMultiTenancy();
        app.UseAbpSession();
        app.UseDynamicClaims();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");

            var configuration = context.GetConfiguration();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            options.OAuthScopes(configuration["AuthServer:Scopes"]);
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
