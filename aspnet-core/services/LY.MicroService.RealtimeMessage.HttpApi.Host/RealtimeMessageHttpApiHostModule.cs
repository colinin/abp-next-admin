using Hangfire;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.BackgroundJobs.Hangfire;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling.Notifications;
using LINGYUN.Abp.Hangfire.Storage.MySql;
using LINGYUN.Abp.Identity.WeChat;
using LINGYUN.Abp.IM.SignalR;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.MessageService;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.MultiTenancy.DbFinder;
using LINGYUN.Abp.Notifications.SignalR;
using LINGYUN.Abp.Notifications.Sms;
using LINGYUN.Abp.Notifications.WeChat.MiniProgram;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
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
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace LY.MicroService.RealtimeMessage
{
    [DependsOn(
        typeof(AbpSerilogEnrichersApplicationModule),
        typeof(AbpSerilogEnrichersUniqueIdModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAuditLoggingElasticsearchModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpMessageServiceApplicationModule),
        typeof(AbpMessageServiceHttpApiModule),
        typeof(AbpIdentityWeChatModule),
        typeof(AbpMessageServiceEntityFrameworkCoreModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
        typeof(AbpDataDbMigratorModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpHangfireMySqlStorageModule),
        typeof(AbpBackgroundJobsHangfireModule),
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpIMSignalRModule),
        typeof(AbpNotificationsSmsModule),
        typeof(AbpNotificationsSignalRModule),
        typeof(AbpNotificationsWeChatMiniProgramModule),
        typeof(AbpNotificationsExceptionHandlingModule),
        typeof(AbpCAPEventBusModule),
        typeof(AbpDbFinderMultiTenancyModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAspNetCoreHttpOverridesModule),
        typeof(AbpLocalizationCultureMapModule),
        typeof(AbpAutofacModule)
        )]
    public partial class RealtimeMessageHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            PreConfigureApp();
            PreCongifureHangfire();
            PreConfigureCAP(configuration);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureDbContext();
            ConfigureLocalization();
            ConfigureJsonSerializer();
            ConfigreExceptionHandling();
            ConfigureVirtualFileSystem();
            ConfigureCaching(configuration);
            ConfigureAuditing(configuration);
            ConfigureSwagger(context.Services);
            ConfigureMultiTenancy(configuration);
            ConfigureHangfireServer(context.Services);
            ConfigureCors(context.Services, configuration);
            ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
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
            app.UseAbpClaimsMap();
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Realtime Message API");
            });
            // 审计日志
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            // 将在 2.0.0版本移除
            // app.UseHangfireServer();
            app.UseHangfireDashboard();
            // 路由
            app.UseConfiguredEndpoints();

            SeedData(context);
        }
    }
}
