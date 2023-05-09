using DotNetCore.CAP;
using LINGYUN.Abp.Account;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.Authentication.QQ;
using LINGYUN.Abp.Authentication.WeChat;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.Http.Client.Wrapper;
using LINGYUN.Abp.Identity.EntityFrameworkCore;
using LINGYUN.Abp.Identity.OrganizaztionUnits;
using LINGYUN.Abp.IdentityServer;
using LINGYUN.Abp.IdentityServer.EntityFrameworkCore;
using LINGYUN.Abp.IdentityServer.Portal;
using LINGYUN.Abp.Localization.CultureMap;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.Sms.Aliyun;
using LINGYUN.Platform.EntityFrameworkCore;
using LY.MicroService.IdentityServer.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Jwt;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LY.MicroService.IdentityServer;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAccountWebIdentityServerModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityApplicationModule),
    // typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpIdentityServerSmsValidatorModule),
    typeof(AbpIdentityServerPortalModule),
    typeof(AbpAuthenticationWeChatModule),
    typeof(AbpAuthenticationQQModule),
    typeof(AbpIdentityOrganizaztionUnitsModule),
    typeof(PlatformEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(IdentityServerMigrationsEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAuditLoggingElasticsearchModule), // 放在 AbpIdentity 模块之后,避免被覆盖
    typeof(AbpAspNetCoreHttpOverridesModule),
    typeof(AbpLocalizationCultureMapModule),
    typeof(AbpCAPEventBusModule),
    typeof(AbpHttpClientWrapperModule),
    typeof(AbpAspNetCoreMvcWrapperModule),
    typeof(AbpAliyunSmsModule)
    )]
public partial class IdentityServerModule : AbpModule
{
    private const string DefaultCorsPolicyName = "Default";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        PreConfigureApp();
        PreConfigureFeature();
        PreConfigureCAP(configuration);
        PreConfigureCertificate(configuration, hostingEnvironment);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureDbContext();
        ConfigureCaching(configuration);
        ConfigureIdentity(configuration);
        ConfigureVirtualFileSystem();
        ConfigureFeatureManagement();
        ConfigureLocalization();
        ConfigureAuditing();
        ConfigureDataSeeder();
        ConfigureMvcUiTheme();
        ConfigureUrls(configuration);
        ConfigureMultiTenancy(configuration);
        ConfigureJsonSerializer(configuration);
        ConfigureCors(context.Services, configuration);
        ConfigureDistributedLocking(context.Services, configuration);
        ConfigureSeedWorker(context.Services, hostingEnvironment.IsDevelopment());
        ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseErrorPage();
            app.UseHsts();
        }

        // app.UseHttpsRedirection();
        app.UseCookiePolicy();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(DefaultCorsPolicyName);
        app.UseWeChatSignature();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();
        app.UseMultiTenancy();
        app.UseMapRequestLocalization();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
