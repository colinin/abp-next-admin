using LINGYUN.Abp.Account;
using LINGYUN.Abp.Account.Templates;
using LINGYUN.Abp.Aliyun.SettingManagement;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.Mvc.Localization;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.Auditing;
using LINGYUN.Abp.AuditLogging.EntityFrameworkCore;
using LINGYUN.Abp.Authentication.QQ;
using LINGYUN.Abp.Authentication.WeChat;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.BackgroundTasks.Activities;
using LINGYUN.Abp.BackgroundTasks.DistributedLocking;
using LINGYUN.Abp.BackgroundTasks.EventBus;
using LINGYUN.Abp.BackgroundTasks.ExceptionHandling;
using LINGYUN.Abp.BackgroundTasks.Jobs;
using LINGYUN.Abp.BackgroundTasks.Notifications;
using LINGYUN.Abp.BackgroundTasks.Quartz;
using LINGYUN.Abp.CachingManagement;
using LINGYUN.Abp.CachingManagement.StackExchangeRedis;
using LINGYUN.Abp.Dapr.Client;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Elsa;
using LINGYUN.Abp.Elsa.Activities;
using LINGYUN.Abp.Elsa.EntityFrameworkCore;
using LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;
using LINGYUN.Abp.ExceptionHandling;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.Features.LimitValidation.Redis.Client;
using LINGYUN.Abp.Http.Client.Wrapper;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.EntityFrameworkCore;
using LINGYUN.Abp.Identity.OrganizaztionUnits;
using LINGYUN.Abp.Identity.WeChat;
using LINGYUN.Abp.IdentityServer;
using LINGYUN.Abp.IdentityServer.EntityFrameworkCore;
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
using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.FileSystem.ImageSharp;
using LINGYUN.Abp.OssManagement.SettingManagement;
using LINGYUN.Abp.PermissionManagement.OrganizationUnits;
using LINGYUN.Abp.Saas;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.Sms.Aliyun;
using LINGYUN.Abp.TaskManagement;
using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
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
using LINGYUN.Abp.WeChat.SettingManagement;
using LINGYUN.Platform;
using LINGYUN.Platform.EntityFrameworkCore;
using LINGYUN.Platform.HttpApi;
using LINGYUN.Platform.Settings.VueVbenAdmin;
using LINGYUN.Platform.Theme.VueVbenAdmin;
using LY.MicroService.Applications.Single.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.EventBus;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LY.MicroService.Applications.Single;

[DependsOn(
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAuditingApplicationModule),
    typeof(AbpAuditingHttpApiModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpCachingManagementStackExchangeRedisModule),
    typeof(AbpCachingManagementApplicationModule),
    typeof(AbpCachingManagementHttpApiModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityServerDomainModule),
    typeof(AbpIdentityServerApplicationModule),
    typeof(AbpIdentityServerHttpApiModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
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
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpOpenIddictApplicationModule),
    typeof(AbpOpenIddictHttpApiModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpOssManagementDomainModule),
    typeof(AbpOssManagementApplicationModule),
    typeof(AbpOssManagementHttpApiModule),
    typeof(AbpOssManagementFileSystemImageSharpModule),
    typeof(AbpOssManagementSettingManagementModule),
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
    typeof(TaskManagementDomainModule),
    typeof(TaskManagementApplicationModule),
    typeof(TaskManagementHttpApiModule),
    typeof(TaskManagementEntityFrameworkCoreModule),
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
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementDomainModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainIdentityServerModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementDomainOrganizationUnitsModule), // 组织机构权限管理
    typeof(SingleMigrationsEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpAliyunSmsModule),
    typeof(AbpAliyunSettingManagementModule),
    typeof(AbpAuthenticationQQModule),
    typeof(AbpAuthenticationWeChatModule),
    typeof(AbpAuthorizationOrganizationUnitsModule),
    typeof(AbpIdentityOrganizaztionUnitsModule),
    typeof(AbpBackgroundTasksModule),
    typeof(AbpBackgroundTasksActivitiesModule),
    typeof(AbpBackgroundTasksDistributedLockingModule),
    typeof(AbpBackgroundTasksEventBusModule),
    typeof(AbpBackgroundTasksExceptionHandlingModule),
    typeof(AbpBackgroundTasksJobsModule),
    typeof(AbpBackgroundTasksNotificationsModule),
    typeof(AbpBackgroundTasksQuartzModule),
    typeof(AbpDaprClientModule),
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
    typeof(AbpWeChatSettingManagementModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpIdGeneratorModule),
    typeof(AbpUINavigationModule),
    typeof(AbpAccountTemplatesModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpElsaModule),
    typeof(AbpElsaServerModule),
    typeof(AbpElsaActivitiesModule),
    typeof(AbpElsaEntityFrameworkCoreModule),
    typeof(AbpElsaEntityFrameworkCoreMySqlModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpHttpClientWrapperModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpEventBusModule),
    typeof(AbpAutofacModule)
    )]
public partial class MicroServiceApplicationsSingleModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        PreConfigureApp();
        PreConfigureFeature();
        PreConfigureQuartz(configuration);
        PreConfigureAuthServer(configuration);
        PreConfigureElsa(context.Services, configuration);
        PreConfigureCertificate(configuration, hostingEnvironment);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureAuditing();
        ConfigureDbContext();
        ConfigureMvcUiTheme();
        ConfigureDataSeeder();
        ConfigureAuthServer();
        ConfigureBlobStoring();
        ConfigureLocalization();
        ConfigureKestrelServer();
        ConfigureJsonSerializer();
        ConfigureTextTemplating();
        ConfigureBackgroundTasks();
        ConfigureFeatureManagement();
        ConfigurePermissionManagement();
        ConfigureUrls(configuration);
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureIdentity(configuration);
        ConfigureSwagger(context.Services);
        ConfigureEndpoints(context.Services);
        ConfigureMultiTenancy(configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureDistributedLock(context.Services, configuration);
        ConfigureSeedWorker(context.Services, hostingEnvironment.IsDevelopment());
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
    }

    //public override void OnApplicationInitialization(ApplicationInitializationContext context)
    //{
    //    var app = context.GetApplicationBuilder();
    //    var configuration = context.GetConfiguration();

    //    app.UseCookiePolicy();
    //    // 本地化
    //    app.UseMapRequestLocalization();
    //    // http调用链
    //    app.UseCorrelationId();
    //    // 虚拟文件系统
    //    app.UseStaticFiles();
    //    // 路由
    //    app.UseRouting();
    //    // 跨域
    //    app.UseCors(DefaultCorsPolicyName);
    //    // 认证
    //    app.UseAuthentication();
    //    if (configuration.GetValue<bool>("AuthServer:UseOpenIddict"))
    //    {
    //        app.UseAbpOpenIddictValidation();
    //    }
    //    else
    //    {
    //        // jwt
    //        app.UseJwtTokenMiddleware();
    //        app.UseIdentityServer();
    //    }
    //    // 多租户
    //    app.UseMultiTenancy();
    //    // 授权
    //    app.UseAuthorization();
    //    // Swagger
    //    app.UseSwagger();
    //    // Swagger可视化界面
    //    app.UseSwaggerUI(options =>
    //    {
    //        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support App API");
    //    });
    //    // 审计日志
    //    app.UseAuditing();
    //    app.UseAbpSerilogEnrichers();
    //    // 路由
    //    app.UseConfiguredEndpoints();
    //}
}
