using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PackageName.CompanyName.ProjectName.EntityFrameworkCore;
using PackageName.CompanyName.ProjectName.SettingManagement;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.Identity.Session.AspNetCore;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAuditLoggingElasticsearchModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(ProjectNameApplicationModule),
    typeof(ProjectNameHttpApiModule),
    typeof(ProjectNameEntityFrameworkCoreModule),
    typeof(ProjectNameSettingManagementModule),
    typeof(AbpEmailingExceptionHandlingModule),
    typeof(AbpCAPEventBusModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpDistributedLockingModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpIdentitySessionAspNetCoreModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAutofacModule)
    )]
public partial class ProjectNameHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigureApp();
        PreConfigureWrapper();
        PreConfigureFeature();
        PreConfigureCAP(configuration);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureWrapper();
        ConfigureLocalization();
        ConfigureExceptionHandling();
        ConfigureVirtualFileSystem();
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureIdentity(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureJsonSerializer(configuration);
        ConfigureSwagger(context.Services);
        ConfigureMvc(context.Services, configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureOpenTelemetry(context.Services, configuration);
        ConfigureDistributedLock(context.Services, configuration);
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseForwardedHeaders();
        app.UseMapRequestLocalization();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();
        app.UseMultiTenancy();
        app.UseAbpSession();
        app.UseDynamicClaims();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support ProjectName API");

            var configuration = context.GetConfiguration();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            options.OAuthScopes("ProjectName");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
