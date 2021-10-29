using DotNetCore.CAP;
using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.AuditLogging.Elasticsearch;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.Identity.EntityFrameworkCore;
using LINGYUN.Abp.IdentityServer;
using LINGYUN.Abp.IdentityServer.EntityFrameworkCore;
using LINGYUN.Abp.IdentityServer.WeChat;
using LINGYUN.Abp.MultiTenancy.DbFinder;
using LINGYUN.Abp.PermissionManagement.Identity;
using LINGYUN.Abp.Sms.Aliyun;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Jwt;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace AuthServer.Host
{
    [DependsOn(
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAuditLoggingElasticsearchModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpIdentityServerSmsValidatorModule),
        typeof(AbpIdentityServerWeChatModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpAspNetCoreHttpOverridesModule),
        typeof(AbpDbFinderMultiTenancyModule),
        typeof(AbpCAPEventBusModule),
        typeof(AbpAliyunSmsModule)
        )]
    public partial class AuthIdentityServerModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            PreConfigure<CapOptions>(options =>
            {
                options
                .UseMySql(configuration.GetConnectionString("Default"))
                .UseRabbitMQ(rabbitMQOptions =>
                {
                    configuration.GetSection("CAP:RabbitMQ").Bind(rabbitMQOptions);
                })
                .UseDashboard();
            });

            var cerConfig = configuration.GetSection("Certificates");
            if (hostingEnvironment.IsProduction() &&
                cerConfig.Exists())
            {
                // 开发环境下存在证书配置
                // 且证书文件存在则使用自定义的证书文件来启动Ids服务器
                var cerPath = Path.Combine(hostingEnvironment.ContentRootPath, cerConfig["CerPath"]);
                if (File.Exists(cerPath))
                {
                    PreConfigure<AbpIdentityServerBuilderOptions>(options =>
                    {
                        options.AddDeveloperSigningCredential = false;
                    });

                    var cer = new X509Certificate2(cerPath, cerConfig["Password"]);

                    PreConfigure<IIdentityServerBuilder>(builder =>
                    {
                        builder.AddSigningCredential(cer);
                    });
                }
            }
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = hostingEnvironment.BuildConfiguration();

            ConfigureDbContext();
            ConfigureJsonSerializer();
            ConfigureCaching(configuration);
            ConfigureIdentity(configuration);
            ConfigureVirtualFileSystem();
            ConfigureLocalization();
            ConfigureAuditing();
            ConfigureUrls(configuration);
            ConfigureMultiTenancy(configuration);
            ConfigureCors(context.Services, configuration);
            ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());

            context.Services.ConfigureNonBreakingSameSiteCookies();

            
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
                // 需要实现一个错误页面
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
            app.UseMultiTenancy();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();
            app.UseAbpRequestLocalization();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            SeedData(context);
        }
    }
}
