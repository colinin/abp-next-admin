using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using LINGYUN.Abp.BlobStoring.BlobManagement;
using LINGYUN.Abp.Claims.Mapping;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Dynamic.Definitions;
using LINGYUN.Abp.ElsaNext.Agents.Blazor;
using LINGYUN.Abp.ElsaNext.BlobStoring;
using LINGYUN.Abp.ElsaNext.Email;
using LINGYUN.Abp.ElsaNext.Labels;
using LINGYUN.Abp.ElsaNext.Notifications;
using LINGYUN.Abp.ElsaNext.Secrets.Blazor;
using LINGYUN.Abp.ElsaNext.Server;
using LINGYUN.Abp.ElsaNext.Studio.AI.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Alterations.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Diagnostics.OpenTelemetry.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Diagnostics.StructuredLogs.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Identity.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Labels.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Notifications.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Saas.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Webhooks.Blazor;
using LINGYUN.Abp.ElsaNext.Webhooks;
using LINGYUN.Abp.Emailing.Platform;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.Http.Client.Wrapper;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.Sms.Platform;
using LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TextTemplating.Scriban;

namespace LINGYUN.Abp.MicroService.WorkflowService;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAuditLoggingElasticsearchModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpBlobStoringBlobManagementModule),
    typeof(AbpElsaNextBlobStoringModule),
    typeof(AbpElsaNextEmailModule),
    typeof(AbpElsaNextLabelsModule),
    typeof(AbpElsaNextNotificationsModule),
    typeof(AbpElsaNextWebhooksModule),
    typeof(AbpElsaNextServerModule),
    typeof(AbpElsaNextStudioBlazorModule),
    typeof(AbpElsaNextAgentsBlazorModule),
    typeof(AbpElsaNextSecretsBlazorModule),
    typeof(AbpElsaNextStudioAIBlazorModule),
    typeof(AbpElsaNextStudioAlterationsBlazorModule),
    typeof(AbpElsaNextStudioDashboardBlazorModule),
    typeof(AbpElsaNextStudioDiagnosticsOpenTelemetryBlazorModule),
    typeof(AbpElsaNextStudioDiagnosticsStructuredLogsBlazorModule),
    typeof(AbpElsaNextStudioIdentityBlazorModule),
    typeof(AbpElsaNextStudioLabelsBlazorModule),
    typeof(AbpElsaNextStudioNotificationsBlazorModule),
    typeof(AbpElsaNextStudioSaasBlazorModule),
    typeof(AbpElsaNextStudioWebhooksBlazorModule),
    typeof(AbpEmailingExceptionHandlingModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    //typeof(AbpBackgroundTasksQuartzModule),
    //typeof(AbpBackgroundTasksDistributedLockingModule),
    //typeof(AbpQuartzPostgresSqlInstallerModule),
    //typeof(TaskManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpNotificationsEntityFrameworkCoreModule),
    typeof(WebhooksManagementEntityFrameworkCoreModule),
    typeof(AbpAuthorizationOrganizationUnitsModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule),
    typeof(AbpTextTemplatingScribanModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreComponentsServerMudBlazorBasicThemeModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpCAPEventBusModule),
    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpHttpClientWrapperModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpSmsPlatformModule),
    typeof(AbpEmailingPlatformModule),
    typeof(AbpClaimsMappingModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpDynamicDefinitionsModule),
    typeof(AbpIdentitySessionAspNetCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class WorkflowServiceModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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
        ConfigureVirtualFileSystem();
        ConfigureNotificationsManagement();
        ConfigurePermissionManagement();
        ConfigureWebhooksManagement();
        ConfigureTiming(configuration);
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureIdentity(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureMvc(context.Services, configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureElsa(context.Services, configuration);
        ConfigureSwagger(context.Services, configuration);
        ConfigureBlobStoring(context.Services, configuration);
        ConfigureDistributedLock(context.Services, configuration);
        ConfigureBackgroundTasks(context.Services, configuration);
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
    }
}
