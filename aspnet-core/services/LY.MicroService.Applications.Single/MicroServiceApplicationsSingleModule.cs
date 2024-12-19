using Volo.Abp.MailKit;

namespace LY.MicroService.Applications.Single;

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

    //typeof(AbpIdentityServerSessionModule),
    //typeof(AbpIdentityServerApplicationModule),
    //typeof(AbpIdentityServerHttpApiModule),
    //typeof(AbpIdentityServerEntityFrameworkCoreModule),

    typeof(LINGYUN.Abp.OpenIddict.AspNetCore.AbpOpenIddictAspNetCoreModule),
    typeof(AbpOpenIddictAspNetCoreSessionModule),
    typeof(AbpOpenIddictApplicationModule),
    typeof(AbpOpenIddictHttpApiModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpOpenIddictSmsModule),
    typeof(AbpOpenIddictPortalModule),
    typeof(AbpOpenIddictWeChatModule),
    typeof(AbpOpenIddictWeChatWorkModule),

    //typeof(AbpOssManagementMinioModule), // 取消注释以使用Minio
    typeof(AbpOssManagementFileSystemModule),
    typeof(AbpOssManagementImagingModule),
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

    typeof(LINGYUN.Abp.FeatureManagement.AbpFeatureManagementApplicationModule),
    typeof(LINGYUN.Abp.FeatureManagement.HttpApi.AbpFeatureManagementHttpApiModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),

    typeof(AbpSettingManagementDomainModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),

    typeof(LINGYUN.Abp.PermissionManagement.AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    // typeof(AbpPermissionManagementDomainIdentityServerModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementDomainOrganizationUnitsModule), // 组织机构权限管理

    // typeof(AbpEntityFrameworkCorePostgreSqlModule),
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

    typeof(AbpDataProtectionManagementApplicationModule),
    typeof(AbpDataProtectionManagementHttpApiModule),
    typeof(AbpDataProtectionManagementEntityFrameworkCoreModule),

    typeof(AbpDemoApplicationModule),
    typeof(AbpDemoHttpApiModule),
    typeof(AbpDemoEntityFrameworkCoreModule),

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
    // typeof(AbpElsaModule),
    // typeof(AbpElsaServerModule),
    // typeof(AbpElsaActivitiesModule),
    // typeof(AbpElsaEntityFrameworkCoreModule),
    // typeof(AbpElsaEntityFrameworkCorePostgreSqlModule),
    // typeof(AbpElsaModule),
    // typeof(AbpElsaServerModule),
    // typeof(AbpElsaActivitiesModule),
    // typeof(AbpElsaEntityFrameworkCoreModule),
    // typeof(AbpElsaEntityFrameworkCoreMySqlModule),

    typeof(AbpExporterMiniExcelModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpHttpClientWrapperModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAspNetCoreMvcIdempotentWrapperModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpMailKitModule),
    typeof(AbpEventBusModule),
    typeof(AbpAutofacModule)
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
        PreConfigureQuartz(configuration);
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
        ConfigureBackgroundTasks();
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
