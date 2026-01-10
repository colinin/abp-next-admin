using LINGYUN.Abp.Account;
using LINGYUN.Abp.Account.Web.OAuth;
using LINGYUN.Abp.Account.Web.OpenIddict;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.MultiTenancy;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.BlobStoring.OssManagement;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Emailing.Platform;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.Exporter.MiniExcel;
using LINGYUN.Abp.Gdpr;
using LINGYUN.Abp.Gdpr.Web;
using LINGYUN.Abp.Identity.AspNetCore.Session;
using LINGYUN.Abp.Identity.OrganizaztionUnits;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.OpenIddict.AspNetCore.Session;
using LINGYUN.Abp.OpenIddict.LinkUser;
using LINGYUN.Abp.OpenIddict.Portal;
using LINGYUN.Abp.OpenIddict.Sms;
using LINGYUN.Abp.OpenIddict.WeChat;
using LINGYUN.Abp.OpenIddict.WeChat.Work;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.Sms.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;

namespace LINGYUN.Abp.MicroService.AuthServer;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAccountWebOAuthModule),
    typeof(AbpBlobStoringOssManagementModule),
    typeof(AbpGdprApplicationModule),
    typeof(AbpGdprHttpApiModule),
    typeof(AbpGdprWebModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpIdentityAspNetCoreSessionModule),
    typeof(AbpOpenIddictAspNetCoreSessionModule),
    typeof(AbpIdentitySessionAspNetCoreModule),
    typeof(AbpOpenIddictSmsModule),
    typeof(AbpOpenIddictWeChatModule),
    typeof(AbpOpenIddictLinkUserModule),
    typeof(AbpOpenIddictPortalModule),
    typeof(AbpOpenIddictWeChatWorkModule),
    typeof(AbpIdentityOrganizaztionUnitsModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AuthServerMigrationsEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAuditLoggingElasticsearchModule), // 放在 AbpIdentity 模块之后,避免被覆盖
    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpExporterMiniExcelModule),
    typeof(AbpEmailingPlatformModule),
    typeof(AbpSmsPlatformModule),
    typeof(AbpCAPEventBusModule)
    )]
public partial class AuthServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        PreConfigureWrapper();
        PreConfigureFeature();
        PreForwardedHeaders();
        PreConfigureAuthServer();
        PreConfigureApp(configuration);
        PreConfigureCAP(configuration);
        PreConfigureCertificate(configuration, hostingEnvironment);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureBranding(configuration);
        ConfigureCaching(configuration);
        ConfigureIdentity(configuration);
        ConfigureVirtualFileSystem();
        ConfigureFeatureManagement();
        ConfigureSettingManagement();
        ConfigurePermissionManagement();
        ConfigureLocalization();
        ConfigureDataSeeder();
        ConfigureUrls(configuration);
        ConfigureTiming(configuration);
        ConfigureAuditing(configuration);
        ConfigureAuthServer(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureJsonSerializer(configuration);
        ConfigureMvc(context.Services, configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureDistributedLocking(context.Services, configuration);
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
    }
}
