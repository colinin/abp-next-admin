using LINGYUN.Abp.AspNetCore.HttpOverrides;
using LINGYUN.Abp.EventBus.CAP;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Extenssions;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace LINGYUN.ApiGateway
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSerilogEnrichersApplicationModule),
        typeof(AbpHttpClientIdentityModelModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAutoMapperModule),
        typeof(ApiGatewayHttpApiClientModule),
        typeof(AbpCAPEventBusModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreHttpOverridesModule)
        )]
    public partial class ApiGatewayHostModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            PreConfigureApp();
            PreConfigureCAP(configuration);
            PreConfigureApiGateway(context.Services, configuration);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureLocalization();
            ConfigureAbpAutoMapper();
            ConfigureJsonSerializer();
            ConfigureVirtualFileSystem();

            ConfigureCaching(configuration);
            ConfigureApiGateway(configuration);

            ConfigureKestrelServer(configuration, hostingEnvironment.IsDevelopment());
            ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());

            ConfigureMvc(context.Services);
            ConfigureSwagger(context.Services);
            ConfigureOcelot(context.Services);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseForwardedHeaders();
            app.UseAuditing();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAbpClaimsMap();
            app.MapWhen(
                ctx => ctx.Request.Path.ToString().StartsWith("/api/ApiGateway/Basic/"),
                appNext =>
                {
                    // 仅针对属于网关自己的控制器进入MVC管道
                    appNext.UseRouting();
                    appNext.UseConfiguredEndpoints();
                });
            UseSwagger(app);
            // 启用ws协议
            app.UseWebSockets();
            app.UseAbpSerilogEnrichers();
            app.UseOcelot().Wait();
        }
    }
}
