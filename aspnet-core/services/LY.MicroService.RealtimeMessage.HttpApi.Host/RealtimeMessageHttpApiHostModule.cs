using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.Mvc.Localization;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using LINGYUN.Abp.BackgroundTasks.DistributedLocking;
using LINGYUN.Abp.BackgroundTasks.ExceptionHandling;
using LINGYUN.Abp.BackgroundTasks.Quartz;
using LINGYUN.Abp.Claims.Mapping;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling.Notifications;
using LINGYUN.Abp.Features.LimitValidation.Redis;
using LINGYUN.Abp.Identity.EntityFrameworkCore;
using LINGYUN.Abp.Identity.Notifications;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.Identity.WeChat;
using LINGYUN.Abp.Identity.WeChat.Work;
using LINGYUN.Abp.IM.SignalR;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.MessageService;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.Notifications.Common;
using LINGYUN.Abp.Notifications.Emailing;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using LINGYUN.Abp.Notifications.Jobs;
using LINGYUN.Abp.Notifications.SignalR;
using LINGYUN.Abp.Notifications.Sms;
using LINGYUN.Abp.Notifications.WeChat.MiniProgram;
using LINGYUN.Abp.Notifications.WeChat.Work;
using LINGYUN.Abp.Notifications.WxPusher;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.Scriban;
using LINGYUN.Abp.WeChat.Official.Handlers;
using LINGYUN.Abp.WeChat.Work.Handlers;
using LY.MicroService.RealtimeMessage.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Http.Client;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LY.MicroService.RealtimeMessage;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAuditLoggingElasticsearchModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreMvcLocalizationModule),
    typeof(AbpMessageServiceApplicationModule),
    typeof(AbpMessageServiceHttpApiModule),
    typeof(AbpNotificationsApplicationModule),
    typeof(AbpNotificationsHttpApiModule),
    typeof(AbpIdentityWeChatModule),
    typeof(AbpIdentityWeChatWorkModule),
    typeof(AbpBackgroundTasksQuartzModule),
    typeof(AbpBackgroundTasksDistributedLockingModule),
    typeof(AbpBackgroundTasksExceptionHandlingModule),
    typeof(TaskManagementEntityFrameworkCoreModule),
    typeof(AbpMessageServiceEntityFrameworkCoreModule),
    typeof(AbpNotificationsEntityFrameworkCoreModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(RealtimeMessageMigrationsEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAuthorizationOrganizationUnitsModule),
    typeof(AbpBackgroundWorkersModule),
    typeof(AbpIMSignalRModule),
    typeof(AbpNotificationsJobsModule),
    typeof(AbpNotificationsCommonModule),
    typeof(AbpNotificationsSmsModule),
    typeof(AbpNotificationsEmailingModule),
    typeof(AbpNotificationsSignalRModule),
    typeof(AbpNotificationsWxPusherModule),
    typeof(AbpNotificationsWeChatMiniProgramModule),
    typeof(AbpNotificationsWeChatWorkModule),
    typeof(AbpNotificationsExceptionHandlingModule),
    typeof(AbpWeChatWorkHandlersModule),
    typeof(AbpWeChatOfficialHandlersModule),
    typeof(AbpIdentityNotificationsModule),

    // 重写模板引擎支持外部本地化
    typeof(AbpTextTemplatingScribanModule),

    typeof(AbpCAPEventBusModule),
    typeof(AbpFeaturesValidationRedisModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpIdentitySessionAspNetCoreModule),
    typeof(AbpHttpClientModule),
    typeof(AbpMailKitModule),
    typeof(AbpClaimsMappingModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpAutofacModule)
    )]
public partial class RealtimeMessageHttpApiHostModule : AbpModule
{
    private const string DefaultCorsPolicyName = "Default";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigureWrapper();
        PreConfigureFeature();
        PreForwardedHeaders();
        PreConfigureApp(configuration);
        PreConfigureCAP(configuration);
        PreConfigureQuartz(configuration);
        PreConfigureSignalR(configuration);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureWrapper();
        ConfigureDbContext();
        ConfigureLocalization();
        ConfigureNotifications();
        ConfigureTextTemplating();
        ConfigureExceptionHandling();
        ConfigureVirtualFileSystem();
        ConfigureFeatureManagement();
        ConfigureTiming(configuration);
        ConfigureCaching(configuration);
        ConfigureAuditing(configuration);
        ConfigureIdentity(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureJsonSerializer(configuration);
        ConfigureBackgroundTasks(configuration);
        ConfigureSwagger(context.Services);
        ConfigureMvc(context.Services, configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureOpenTelemetry(context.Services, configuration);
        ConfigureDistributedLocking(context.Services, configuration);
        ConfigureSeedWorker(context.Services, hostingEnvironment.IsDevelopment());
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
        app.UseStaticFiles();
        // 路由
        app.UseRouting();
        // 跨域
        app.UseCors(DefaultCorsPolicyName);
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
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Realtime Message API");
        });
        // 审计日志
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        // 路由
        app.UseConfiguredEndpoints();
    }
}
