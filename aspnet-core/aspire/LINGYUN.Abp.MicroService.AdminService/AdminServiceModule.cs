using LINGYUN.Abp.Aliyun.SettingManagement;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.Mvc.Localization;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.Auditing;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.CachingManagement;
using LINGYUN.Abp.CachingManagement.StackExchangeRedis;
using LINGYUN.Abp.Claims.Mapping;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.DataProtectionManagement;
using LINGYUN.Abp.Emailing.Platform;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.ExceptionHandling.Emailing;
using LINGYUN.Abp.FeatureManagement;
using LINGYUN.Abp.FeatureManagement.HttpApi;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.Logging.Serilog.Elasticsearch;
using LINGYUN.Abp.OssManagement.SettingManagement;
using LINGYUN.Abp.PermissionManagement;
using LINGYUN.Abp.PermissionManagement.HttpApi;
using LINGYUN.Abp.PermissionManagement.OrganizationUnits;
using LINGYUN.Abp.Saas;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.Sms.Platform;
using LINGYUN.Abp.Tencent.SettingManagement;
using LINGYUN.Abp.TextTemplating;
using LINGYUN.Abp.TextTemplating.Scriban;
using LINGYUN.Abp.WxPusher.SettingManagement;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.MicroService.AdminService;

[DependsOn(
    typeof(AbpCAPEventBusModule),
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpLoggingSerilogElasticsearchModule),
    typeof(AbpAuditLoggingElasticsearchModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAspNetCoreMvcLocalizationModule),

    // 设置管理
    typeof(AbpAliyunSettingManagementModule),
    typeof(AbpTencentCloudSettingManagementModule),
    // typeof(AbpWeChatSettingManagementModule),
    typeof(AbpWxPusherSettingManagementModule),
    typeof(AbpOssManagementSettingManagementModule),

    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpDataProtectionManagementApplicationModule),
    typeof(AbpDataProtectionManagementHttpApiModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpFeatureManagementClientModule),
    typeof(AbpAuditingApplicationModule),
    typeof(AbpAuditingHttpApiModule),
    typeof(AbpSaasApplicationModule),
    typeof(AbpSaasHttpApiModule),
    typeof(AbpSaasDbCheckerModule),
    typeof(AbpTextTemplatingApplicationModule),
    typeof(AbpTextTemplatingHttpApiModule),
    typeof(AbpCachingManagementApplicationModule),
    typeof(AbpCachingManagementHttpApiModule),
    typeof(AbpCachingManagementStackExchangeRedisModule),
    typeof(AbpPermissionManagementDomainOrganizationUnitsModule), // 组织机构权限管理
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),

    // 重写模板引擎支持外部本地化
    typeof(AbpTextTemplatingScribanModule),

    typeof(AbpIdentitySessionAspNetCoreModule),

    typeof(AdminServiceMigrationsEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpEmailingExceptionHandlingModule),
    typeof(AbpHttpClientModule),
    typeof(AbpSmsPlatformModule),
    typeof(AbpEmailingPlatformModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpClaimsMappingModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAutofacModule)
    )]
public partial class AdminServiceModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigureWrapper();
        PreConfigureFeature();
        PreConfigureApp(configuration);
        PreConfigureCAP(configuration);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureWrapper();
        ConfigureLocalization();
        ConfigureVirtualFileSystem();
        ConfigureTextTemplating();
        ConfigureSettingManagement();
        ConfigureFeatureManagement();
        ConfigurePermissionManagement();
        ConfigureDataProtectedManagement();
        ConfigureIdentity(configuration);
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
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<IDataSeeder>()
            .SeedAsync();
    }
}
