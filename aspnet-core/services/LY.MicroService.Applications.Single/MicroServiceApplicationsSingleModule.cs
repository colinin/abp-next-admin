using LINGYUN.Abp.AuditLogging.IP.Location;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.IP2Region;
using Volo.Abp.MailKit;

namespace LY.MicroService.Applications.Single;

[DependsOn(

    // 账户模块 应用服务
    typeof(AbpAccountApplicationModule),
    // 账户模块 控制器
    typeof(AbpAccountHttpApiModule),
    // 账户模块 OpenIddict集成
    typeof(AbpAccountWebOpenIddictModule),
    // 账户模块 模板
    typeof(AbpAccountTemplatesModule),

    // 审计日志模块 应用服务
    typeof(AbpAuditingApplicationModule),
    // 审计日志模块 控制器
    typeof(AbpAuditingHttpApiModule),
    // 审计日志模块 IP 地址定位
    typeof(AbpAuditLoggingIPLocationModule),
    // 审计日志模块 实体框架
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),

    // 缓存模块 Redis集成
    typeof(AbpCachingStackExchangeRedisModule),
    // 缓存管理模块 Redis集成
    typeof(AbpCachingManagementStackExchangeRedisModule),
    // 缓存管理模块 应用服务
    typeof(AbpCachingManagementApplicationModule),
    // 缓存管理模块 控制器
    typeof(AbpCachingManagementHttpApiModule),

    // 身份认证模块 会话管理集成
    typeof(AbpIdentityAspNetCoreSessionModule),
    // 身份认证模块 会话中间件
    typeof(AbpIdentitySessionAspNetCoreModule),
    // 身份认证模块 通知集成
    typeof(AbpIdentityNotificationsModule),
    // 身份认证模块 组织机构集成
    typeof(AbpIdentityOrganizaztionUnitsModule),
    // 身份认证模块 微信身份标识
    typeof(AbpIdentityWeChatModule),
    // 身份认证模块 领域服务
    typeof(AbpIdentityDomainModule),
    // 身份认证模块 应用服务
    typeof(AbpIdentityApplicationModule),
    // 身份认证模块 控制器
    typeof(AbpIdentityHttpApiModule),
    // 身份认证模块 实体框架
    typeof(AbpIdentityEntityFrameworkCoreModule),

    // 多语言管理模块 领域服务
    typeof(AbpLocalizationManagementDomainModule),
    // 多语言管理模块 应用服务
    typeof(AbpLocalizationManagementApplicationModule),
    // 多语言管理模块 控制器
    typeof(AbpLocalizationManagementHttpApiModule),
    // 多语言管理模块 实体框架
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),

    // Serilog扩展模块 应用程序信息
    typeof(AbpSerilogEnrichersApplicationModule),
    // Serilog扩展模块 全局唯一Id
    typeof(AbpSerilogEnrichersUniqueIdModule),

    // 消息模块 领域服务
    typeof(AbpMessageServiceDomainModule),
    // 消息模块 应用服务
    typeof(AbpMessageServiceApplicationModule),
    // 消息模块 控制器
    typeof(AbpMessageServiceHttpApiModule),
    // 消息模块 实体框架
    typeof(AbpMessageServiceEntityFrameworkCoreModule),
    // 通知模块 领域服务
    typeof(AbpNotificationsDomainModule),
    // 通知模块 应用服务
    typeof(AbpNotificationsApplicationModule),
    // 通知模块 控制器
    typeof(AbpNotificationsHttpApiModule),
    // 通知模块 实体框架
    typeof(AbpNotificationsEntityFrameworkCoreModule),

    //typeof(AbpIdentityServerSessionModule),
    //typeof(AbpIdentityServerApplicationModule),
    //typeof(AbpIdentityServerHttpApiModule),
    //typeof(AbpIdentityServerEntityFrameworkCoreModule),

    // OpenIddict扩展模块 自定义身份标识
    typeof(LINGYUN.Abp.OpenIddict.AspNetCore.AbpOpenIddictAspNetCoreModule),
    // OpenIddict扩展模块 会话
    typeof(AbpOpenIddictAspNetCoreSessionModule),
    // OpenIddict扩展模块 应用服务
    typeof(AbpOpenIddictApplicationModule),
    // OpenIddict扩展模块 控制器
    typeof(AbpOpenIddictHttpApiModule),
    // OpenIddict扩展模块 实体框架
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    // OpenIddict扩展模块 短信认证
    typeof(AbpOpenIddictSmsModule),
    // OpenIddict扩展模块 平台认证
    typeof(AbpOpenIddictPortalModule),
    // OpenIddict扩展模块 微信认证
    typeof(AbpOpenIddictWeChatModule),
    // OpenIddict扩展模块 企业微信认证
    typeof(AbpOpenIddictWeChatWorkModule),

    //typeof(AbpOssManagementMinioModule), // 取消注释以使用Minio
    // 对象存储模块 文件系统
    typeof(AbpOssManagementFileSystemModule),
    // 对象存储模块 图片处理
    typeof(AbpOssManagementImagingModule),
    // 对象存储模块 应用服务
    typeof(AbpOssManagementDomainModule),
    // 对象存储模块 控制器
    typeof(AbpOssManagementApplicationModule),
    // 对象存储模块 控制器
    typeof(AbpOssManagementHttpApiModule),
    // 对象存储模块 设置管理
    typeof(AbpOssManagementSettingManagementModule),
    // 图形处理模块
    typeof(AbpImagingImageSharpModule),

    // 平台模块 领域服务
    typeof(PlatformDomainModule),
    // 平台模块 应用服务
    typeof(PlatformApplicationModule),
    // 平台模块 控制器
    typeof(PlatformHttpApiModule),
    // 平台模块 实体框架
    typeof(PlatformEntityFrameworkCoreModule),
    // 平台模块 VueVbenAdmin设置
    typeof(PlatformSettingsVueVbenAdminModule),
    // 平台模块 VueVbenAdmin主题
    typeof(PlatformThemeVueVbenAdminModule),
    // 平台模块 VueVbenAdmin导航
    typeof(AbpUINavigationVueVbenAdminModule),

    // Saas模块 领域服务
    typeof(AbpSaasDomainModule),
    // Saas模块 应用服务
    typeof(AbpSaasApplicationModule),
    // Saas模块 控制器
    typeof(AbpSaasHttpApiModule),
    // Saas模块 实体框架
    typeof(AbpSaasEntityFrameworkCoreModule),

    // 任务管理模块 领域服务
    typeof(TaskManagementDomainModule),
    // 任务管理模块 应用服务
    typeof(TaskManagementApplicationModule),
    // 任务管理模块 控制器
    typeof(TaskManagementHttpApiModule),
    // 任务管理模块 实体框架
    typeof(TaskManagementEntityFrameworkCoreModule),

    // 文本模板模块 领域服务
    typeof(AbpTextTemplatingDomainModule),
    // 文本模板模块 应用服务
    typeof(AbpTextTemplatingApplicationModule),
    // 文本模板模块 控制器
    typeof(AbpTextTemplatingHttpApiModule),
    // 文本模板模块 实体框架
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),

    // Webhooks模块 领域服务
    typeof(AbpWebhooksModule),
    // Webhooks模块 事件总线
    typeof(AbpWebhooksEventBusModule),
    // Webhooks模块 身份认证事件
    typeof(AbpWebhooksIdentityModule),
    // Webhooks模块 Saas事件
    typeof(AbpWebhooksSaasModule),
    // Webhooks模块 应用服务
    typeof(WebhooksManagementDomainModule),
    // Webhooks模块 控制器
    typeof(WebhooksManagementApplicationModule),
    // Webhooks模块 控制器
    typeof(WebhooksManagementHttpApiModule),
    // Webhooks模块 实体框架
    typeof(WebhooksManagementEntityFrameworkCoreModule),

    // 功能管理模块 应用服务
    typeof(LINGYUN.Abp.FeatureManagement.AbpFeatureManagementApplicationModule),
    // 功能管理模块 控制器
    typeof(LINGYUN.Abp.FeatureManagement.HttpApi.AbpFeatureManagementHttpApiModule),
    // 功能管理模块 实体框架
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),

    // 设置管理模块 领域服务
    typeof(AbpSettingManagementDomainModule),
    // 设置管理模块 应用服务
    typeof(AbpSettingManagementApplicationModule),
    // 设置管理模块 控制器
    typeof(AbpSettingManagementHttpApiModule),
    // 设置管理模块 实体框架
    typeof(AbpSettingManagementEntityFrameworkCoreModule),

    // 权限管理模块 应用服务
    typeof(LINGYUN.Abp.PermissionManagement.AbpPermissionManagementApplicationModule),
    // 权限管理模块 控制器
    typeof(AbpPermissionManagementHttpApiModule),
    // 权限管理模块 身份认证集成
    typeof(AbpPermissionManagementDomainIdentityModule),
    // 权限管理模块 OpenIddict集成
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    // typeof(AbpPermissionManagementDomainIdentityServerModule),
    // 权限管理模块 实体框架
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    // 权限管理模块 组织机构集成
    typeof(AbpPermissionManagementDomainOrganizationUnitsModule), // 组织机构权限管理

    // 单体服务实体框架迁移模块
    typeof(SingleMigrationsEntityFrameworkCoreModule),
    // 实体框架MySQL集成
    typeof(AbpEntityFrameworkCoreMySQLModule),

    // 短信模块 阿里云集成
    typeof(AbpAliyunSmsModule),
    // 阿里云模块 设置管理
    typeof(AbpAliyunSettingManagementModule),

    // 认证模块 腾讯QQ集成
    typeof(AbpAuthenticationQQModule),
    // 认证模块 微信集成
    typeof(AbpAuthenticationWeChatModule),
    // 认证模块 JWT认证
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    // 授权模块 组织机构集成
    typeof(AbpAuthorizationOrganizationUnitsModule),

    // 后台任务模块
    typeof(AbpBackgroundTasksModule),
    // 后台任务模块 行为处理
    typeof(AbpBackgroundTasksActivitiesModule),
    // 后台任务模块 分布式锁
    typeof(AbpBackgroundTasksDistributedLockingModule),
    // 后台任务模块 事件总线
    typeof(AbpBackgroundTasksEventBusModule),
    // 后台任务模块 异常处理
    typeof(AbpBackgroundTasksExceptionHandlingModule),
    // 后台任务模块 默认作业
    typeof(AbpBackgroundTasksJobsModule),
    // 后台任务模块 通知
    typeof(AbpBackgroundTasksNotificationsModule),
    // 后台任务模块 Quartz集成
    typeof(AbpBackgroundTasksQuartzModule),

    // 数据审计模块 应用服务
    typeof(AbpDataProtectionManagementApplicationModule),
    // 数据审计模块 控制器
    typeof(AbpDataProtectionManagementHttpApiModule),
    // 数据审计模块 实体框架
    typeof(AbpDataProtectionManagementEntityFrameworkCoreModule),

    // Demo模块 应用服务
    typeof(AbpDemoApplicationModule),
    // Demo模块 控制器
    typeof(AbpDemoHttpApiModule),
    // Demo模块 实体框架
    typeof(AbpDemoEntityFrameworkCoreModule),

    // Dapr模块 客户端
    typeof(AbpDaprClientModule),
    // 异常处理模块
    typeof(AbpExceptionHandlingModule),
    // 异常处理模块 邮件通知
    typeof(AbpEmailingExceptionHandlingModule),

    // 功能限制模块
    typeof(AbpFeaturesLimitValidationModule),
    // 客户端功能限制模块 Redis集成
    typeof(AbpFeaturesValidationRedisClientModule),
    // 多语言模块
    typeof(AbpAspNetCoreMvcLocalizationModule),
    // 多语言模块 语言映射
    typeof(AbpLocalizationCultureMapModule),
    // 多语言模块 持久化
    typeof(AbpLocalizationPersistenceModule),

    // OpenApi模块 授权
    typeof(AbpOpenApiAuthorizationModule),

    // 消息模块 实时框架
    typeof(AbpIMSignalRModule),

    // 通知模块
    typeof(AbpNotificationsModule),
    // 通知模块 默认通知
    typeof(AbpNotificationsCommonModule),
    // 通知模块 实时框架
    typeof(AbpNotificationsSignalRModule),
    // 通知模块 邮件通知
    typeof(AbpNotificationsEmailingModule),
    // 通知模块 微信小程序
    typeof(AbpNotificationsWeChatMiniProgramModule),
    // 多租户模块 版本
    typeof(AbpMultiTenancyEditionsModule),

    // 腾讯QQ模块
    typeof(AbpTencentQQModule),
    // 腾讯云模块 设置管理
    typeof(AbpTencentCloudSettingManagementModule),

    // 微信模块 微信小程序
    typeof(AbpWeChatMiniProgramModule),
    // 微信模块 微信公众号
    typeof(AbpWeChatOfficialModule),
    // 微信模块 微信公众号 应用服务
    typeof(AbpWeChatOfficialApplicationModule),
    // 微信模块 微信公众号 控制器
    typeof(AbpWeChatOfficialHttpApiModule),
    // 微信模块 企业微信
    typeof(AbpWeChatWorkModule),
    // 微信模块 企业微信 应用服务
    typeof(AbpWeChatWorkApplicationModule),
    // 微信模块 企业微信 控制器
    typeof(AbpWeChatWorkHttpApiModule),
    // 微信模块 微信公众号 事件处理
    typeof(AbpWeChatOfficialHandlersModule),
    // 微信模块 企业微信 事件处理
    typeof(AbpWeChatWorkHandlersModule),
    // 微信模块 设置管理
    typeof(AbpWeChatSettingManagementModule),

    // 数据迁移模块
    typeof(AbpDataDbMigratorModule),
    // IP解析模块 IP2Region集成
    typeof(AbpIP2RegionModule),
    // 分布式Id生成器模块
    typeof(AbpIdGeneratorModule),
    // 自定义导航模块
    typeof(AbpUINavigationModule),

    // Elsa工作流模块
    typeof(AbpElsaModule),
    // Elsa工作流模块 工作流服务器
    typeof(AbpElsaServerModule),
    // Elsa工作流模块 活动
    typeof(AbpElsaActivitiesModule),
    // Elsa工作流模块 实体框架
    typeof(AbpElsaEntityFrameworkCoreModule),
    // Elsa工作流模块 MySql集成
    typeof(AbpElsaEntityFrameworkCoreMySqlModule),

    // CAP事件总线模块
    typeof(AbpCAPEventBusModule),

    // 数据导出模块 MiniExcel集成
    typeof(AbpExporterMiniExcelModule),

    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpHttpClientWrapperModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAspNetCoreMvcIdempotentWrapperModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpMailKitModule),
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
        PreConfigureCAP(configuration);
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
        ConfigureDbContext(configuration);
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
