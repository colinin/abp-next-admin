using LINGYUN.Abp.Account;
using LINGYUN.Abp.Account.Templates;
using LINGYUN.Abp.Aliyun.SettingManagement;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.Mvc.Idempotent.Wrapper;
using LINGYUN.Abp.AspNetCore.Mvc.Localization;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.Auditing;
using LINGYUN.Abp.AuditLogging.EntityFrameworkCore;
using LINGYUN.Abp.Authentication.QQ;
using LINGYUN.Abp.Authentication.WeChat;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using LINGYUN.Abp.CachingManagement;
using LINGYUN.Abp.CachingManagement.StackExchangeRedis;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.DataProtectionManagement;
using LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore;
using LINGYUN.Abp.ExceptionHandling;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.Exporter.MiniExcel;
using LINGYUN.Abp.FeatureManagement;
using LINGYUN.Abp.FeatureManagement.HttpApi;
using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.Features.LimitValidation.Redis.Client;
using LINGYUN.Abp.Http.Client.Wrapper;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.AspNetCore.Session;
using LINGYUN.Abp.Identity.EntityFrameworkCore;
using LINGYUN.Abp.Identity.Notifications;
using LINGYUN.Abp.Identity.OrganizaztionUnits;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.Identity.WeChat;
using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.IM.SignalR;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.Localization.Persistence;
using LINGYUN.Abp.LocalizationManagement;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.MessageService;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.MultiTenancy.Editions;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.Notifications.Common;
using LINGYUN.Abp.Notifications.Emailing;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using LINGYUN.Abp.Notifications.SignalR;
using LINGYUN.Abp.Notifications.WeChat.MiniProgram;
using LINGYUN.Abp.OpenApi.Authorization;
using LINGYUN.Abp.OpenIddict;
using LINGYUN.Abp.OpenIddict.AspNetCore;
using LINGYUN.Abp.OpenIddict.AspNetCore.Session;
using LINGYUN.Abp.OpenIddict.Portal;
using LINGYUN.Abp.OpenIddict.Sms;
using LINGYUN.Abp.OpenIddict.WeChat;
using LINGYUN.Abp.OpenIddict.WeChat.Work;
using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.FileSystem;
using LINGYUN.Abp.OssManagement.SettingManagement;
using LINGYUN.Abp.PermissionManagement;
using LINGYUN.Abp.PermissionManagement.HttpApi;
using LINGYUN.Abp.PermissionManagement.OrganizationUnits;
using LINGYUN.Abp.Saas;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.Sms.Aliyun;
using LINGYUN.Abp.Tencent.QQ;
using LINGYUN.Abp.Tencent.SettingManagement;
using LINGYUN.Abp.TextTemplating;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using LINGYUN.Abp.UI.Navigation;
using LINGYUN.Abp.UI.Navigation.VueVbenAdmin;
using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.Webhooks.EventBus;
using LINGYUN.Abp.Webhooks.Identity;
using LINGYUN.Abp.Webhooks.Saas;
using LINGYUN.Abp.WebhooksManagement;
using LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;
using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.Official;
using LINGYUN.Abp.WeChat.Official.Handlers;
using LINGYUN.Abp.WeChat.SettingManagement;
using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Handlers;
using LINGYUN.Platform;
using LINGYUN.Platform.EntityFrameworkCore;
using LINGYUN.Platform.HttpApi;
using LINGYUN.Platform.Settings.VueVbenAdmin;
using LINGYUN.Platform.Theme.VueVbenAdmin;
using PackageName.CompanyName.ProjectName.EntityFrameworkCore;
using PackageName.CompanyName.ProjectName.SettingManagement;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.BackgroundWorkers.Hangfire;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.EventBus;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Imaging;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Threading;
// using LINGYUN.Abp.Demo;
// using LINGYUN.Abp.Demo.EntityFrameworkCore;
// using LINGYUN.Abp.OssManagement.Imaging;

namespace PackageName.CompanyName.ProjectName.AIO.Host;

[DependsOn(
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAuditingApplicationModule),
    typeof(AbpAuditingHttpApiModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpCachingManagementStackExchangeRedisModule),
    typeof(AbpCachingManagementApplicationModule),
    typeof(AbpCachingManagementHttpApiModule),
    typeof(AbpIdentityAspNetCoreSessionModule),
    typeof(AbpIdentitySessionAspNetCoreModule),
    typeof(AbpIdentityNotificationsModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementDomainModule),
    typeof(AbpLocalizationManagementApplicationModule),
    typeof(AbpLocalizationManagementHttpApiModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpMessageServiceDomainModule),
    typeof(AbpMessageServiceApplicationModule),
    typeof(AbpMessageServiceHttpApiModule),
    typeof(AbpMessageServiceEntityFrameworkCoreModule),
    typeof(AbpNotificationsDomainModule),
    typeof(AbpNotificationsApplicationModule),
    typeof(AbpNotificationsHttpApiModule),
    typeof(AbpNotificationsEntityFrameworkCoreModule),
    typeof(AbpOpenIddictAspNetCoreModule),
    typeof(AbpOpenIddictAspNetCoreSessionModule),
    typeof(AbpOpenIddictApplicationModule),
    typeof(AbpOpenIddictHttpApiModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpOpenIddictSmsModule),
    typeof(AbpOpenIddictPortalModule),
    typeof(AbpOpenIddictWeChatModule),
    typeof(AbpOpenIddictWeChatWorkModule),

    typeof(AbpBackgroundWorkersHangfireModule),
    typeof(AbpBackgroundJobsHangfireModule),

    //typeof(AbpOssManagementMinioModule), // 取消注释以使用Minio
    typeof(AbpOssManagementFileSystemModule),
    // typeof(AbpOssManagementImagingModule),
    typeof(AbpOssManagementDomainModule),
    typeof(AbpOssManagementApplicationModule),
    typeof(AbpOssManagementHttpApiModule),
    typeof(AbpOssManagementSettingManagementModule),
    typeof(AbpImagingImageSharpModule),

    typeof(PlatformDomainModule),
    typeof(PlatformApplicationModule),
    typeof(PlatformHttpApiModule),
    typeof(PlatformEntityFrameworkCoreModule),
    typeof(PlatformSettingsVueVbenAdminModule),
    typeof(PlatformThemeVueVbenAdminModule),
    typeof(AbpUINavigationVueVbenAdminModule),

    typeof(AbpSaasDomainModule),
    typeof(AbpSaasApplicationModule),
    typeof(AbpSaasHttpApiModule),
    typeof(AbpSaasEntityFrameworkCoreModule),

    typeof(AbpTextTemplatingDomainModule),
    typeof(AbpTextTemplatingApplicationModule),
    typeof(AbpTextTemplatingHttpApiModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),

    typeof(AbpWebhooksModule),
    typeof(AbpWebhooksEventBusModule),
    typeof(AbpWebhooksIdentityModule),
    typeof(AbpWebhooksSaasModule),
    typeof(WebhooksManagementDomainModule),
    typeof(WebhooksManagementApplicationModule),
    typeof(WebhooksManagementHttpApiModule),
    typeof(WebhooksManagementEntityFrameworkCoreModule),

    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),

    typeof(AbpSettingManagementDomainModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),

    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    // typeof(AbpPermissionManagementDomainIdentityServerModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementDomainOrganizationUnitsModule), // 组织机构权限管理

    typeof(AbpEntityFrameworkCorePostgreSqlModule),

    typeof(AbpAliyunSmsModule),
    typeof(AbpAliyunSettingManagementModule),

    typeof(AbpAuthenticationQQModule),
    typeof(AbpAuthenticationWeChatModule),
    typeof(AbpAuthorizationOrganizationUnitsModule),
    typeof(AbpIdentityOrganizaztionUnitsModule),

    typeof(AbpDataProtectionManagementApplicationModule),
    typeof(AbpDataProtectionManagementHttpApiModule),
    typeof(AbpDataProtectionManagementEntityFrameworkCoreModule),
    typeof(AbpExceptionHandlingModule),
    typeof(AbpEmailingExceptionHandlingModule),
    typeof(AbpFeaturesLimitValidationModule),
    typeof(AbpFeaturesValidationRedisClientModule),
    typeof(AbpAspNetCoreMvcLocalizationModule),

    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpLocalizationPersistenceModule),

    typeof(AbpOpenApiAuthorizationModule),

    typeof(AbpIMSignalRModule),

    typeof(AbpNotificationsModule),
    typeof(AbpNotificationsCommonModule),
    typeof(AbpNotificationsSignalRModule),
    typeof(AbpNotificationsEmailingModule),
    typeof(AbpMultiTenancyEditionsModule),

    typeof(AbpTencentQQModule),
    typeof(AbpTencentCloudSettingManagementModule),

    typeof(AbpIdentityWeChatModule),
    typeof(AbpNotificationsWeChatMiniProgramModule),
    typeof(AbpWeChatMiniProgramModule),
    typeof(AbpWeChatOfficialModule),
    typeof(AbpWeChatOfficialApplicationModule),
    typeof(AbpWeChatOfficialHttpApiModule),
    typeof(AbpWeChatWorkModule),
    typeof(AbpWeChatWorkApplicationModule),
    typeof(AbpWeChatWorkHttpApiModule),
    typeof(AbpWeChatOfficialHandlersModule),
    typeof(AbpWeChatWorkHandlersModule),
    typeof(AbpWeChatSettingManagementModule),

    typeof(AbpDataDbMigratorModule),
    typeof(AbpIdGeneratorModule),
    typeof(AbpUINavigationModule),
    typeof(AbpAccountTemplatesModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpExporterMiniExcelModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpHttpClientWrapperModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAspNetCoreMvcIdempotentWrapperModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpEventBusModule),
    typeof(AbpAutofacModule),

    typeof(ProjectNameApplicationModule),
    typeof(ProjectNameHttpApiModule),
    typeof(ProjectNameEntityFrameworkCoreModule),
    typeof(ProjectNameSettingManagementModule)
    )]
public partial class MicroServiceApplicationsSingleModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        PreConfigureWrapper();
        PreConfigureFeature();
        PreConfigureIdentity();
        PreConfigureApp(configuration);
        PreConfigureAuthServer(configuration);
        PreConfigureElsa(context.Services, configuration);
        PreConfigureCertificate(configuration, hostingEnvironment);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureWeChat();
        ConfigureWrapper();
        ConfigureExporter();
        ConfigureAuditing();
        ConfigureDbContext();
        ConfigureIdempotent();
        ConfigureMvcUiTheme();
        ConfigureDataSeeder();
        ConfigureLocalization();
        ConfigureKestrelServer();
        ConfigureHangfire(context.Services, configuration);
        ConfigureExceptionHandling();
        ConfigureVirtualFileSystem();
        ConfigureEntityDataProtected();
        ConfigureUrls(configuration);
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureIdentity(configuration);
        ConfigureAuthServer(configuration);
        ConfigureSwagger(context.Services);
        ConfigureEndpoints(context.Services);
        ConfigureBlobStoring(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureJsonSerializer(configuration);
        ConfigureTextTemplating(configuration);
        ConfigureFeatureManagement(configuration);
        ConfigureSettingManagement(configuration);
        ConfigureWebhooksManagement(configuration);
        ConfigurePermissionManagement(configuration);
        ConfigureNotificationManagement(configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureDistributedLock(context.Services, configuration);
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () => await OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(); ;
    }
}
